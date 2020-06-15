package co.gov.and.coronapp.bluetrace.services

import android.annotation.SuppressLint
import android.app.NotificationChannel
import android.app.NotificationManager
import android.app.Service
import android.bluetooth.BluetoothAdapter
import android.bluetooth.BluetoothManager
import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import android.os.AsyncTask
import android.os.Build
import android.os.IBinder
import android.os.PowerManager
import android.widget.Toast
import androidx.localbroadcastmanager.content.LocalBroadcastManager
import co.gov.and.coronapp.bluetrace.BluetraceUtils
import co.gov.and.coronapp.bluetrace.BuildConfig
import co.gov.and.coronapp.bluetrace.Preference
import co.gov.and.coronapp.bluetrace.R
import co.gov.and.coronapp.bluetrace.bluetooth.BLEAdvertiser
import co.gov.and.coronapp.bluetrace.bluetooth.gatt.*
import co.gov.and.coronapp.bluetrace.idmanager.TempIDManager
import co.gov.and.coronapp.bluetrace.idmanager.TemporaryID
import co.gov.and.coronapp.bluetrace.idmanager.fromPersistence
import co.gov.and.coronapp.bluetrace.idmanager.persistence.TemporaryIdStorage
import co.gov.and.coronapp.bluetrace.idmanager.toPersistence
import co.gov.and.coronapp.bluetrace.logging.CentralLog
import co.gov.and.coronapp.bluetrace.notifications.NotificationTemplates
import co.gov.and.coronapp.bluetrace.permissions.RequestBluetoothPermission
import co.gov.and.coronapp.bluetrace.permissions.RequestLocationPermission
import co.gov.and.coronapp.bluetrace.status.Status
import co.gov.and.coronapp.bluetrace.status.persistence.StatusRecord
import co.gov.and.coronapp.bluetrace.status.persistence.StatusRecordStorage
import co.gov.and.coronapp.bluetrace.streetpass.*
import co.gov.and.coronapp.bluetrace.streetpass.persistence.StreetPassRecord
import co.gov.and.coronapp.bluetrace.streetpass.persistence.StreetPassRecordStorage
import co.gov.and.coronapp.bluetrace.streetpass.server.StreetPassRequest
import co.gov.and.coronapp.bluetrace.streetpass.server.fromPersistence
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.Job
import kotlinx.coroutines.launch
import pub.devrel.easypermissions.EasyPermissions
import java.lang.ref.WeakReference
import kotlin.coroutines.CoroutineContext

class BluetoothMonitoringService : Service(), CoroutineScope {

    private var mNotificationManager: NotificationManager? = null

    private lateinit var serviceUUID: String

    private var streetPassServer: StreetPassServer? = null
    private var streetPassScanner: StreetPassScanner? = null
    private var advertiser: BLEAdvertiser? = null

    private var worker: StreetPassWorker? = null

    private val streetPassReceiver = StreetPassReceiver()
    private val statusReceiver = StatusReceiver()
    private val temporaryIdReceiver = TemporaryIdReceiver()
    private val bluetoothStatusReceiver = BluetoothStatusReceiver()
    private val messageReceiver = MessageReceiver()

    private lateinit var streetPassRecordStorage: StreetPassRecordStorage
    private lateinit var statusRecordStorage: StatusRecordStorage
    private lateinit var temporaryIdStorage: TemporaryIdStorage

    private var job: Job = Job()

    override val coroutineContext: CoroutineContext
        get() = Dispatchers.Main + job

    private lateinit var commandHandler: CommandHandler

    private lateinit var localBroadcastManager: LocalBroadcastManager

    private var notificationShown: NOTIFICATION_STATE? = null

    override fun onCreate() {
        localBroadcastManager = LocalBroadcastManager.getInstance(this)
        setup()
    }

    private fun setup() {
        val pm = getSystemService(Context.POWER_SERVICE) as PowerManager

        CentralLog.setPowerManager(pm)

        commandHandler = CommandHandler(WeakReference(this))

        CentralLog.d(TAG, "Creating service - BluetoothMonitoringService")
        serviceUUID = BuildConfig.BLE_SSID

        worker = StreetPassWorker(this.applicationContext)

        unregisterReceivers()
        registerReceivers()

        streetPassRecordStorage = StreetPassRecordStorage(this.applicationContext)
        statusRecordStorage = StatusRecordStorage(this.applicationContext)
        temporaryIdStorage = TemporaryIdStorage(this.applicationContext)

        setupNotifications()
        BluetraceUtils.broadcastMessageReceived(this.applicationContext)
    }

    fun teardown() {
        streetPassServer?.tearDown()
        streetPassServer = null

        streetPassScanner?.stopScan()
        streetPassScanner = null

        commandHandler.removeCallbacksAndMessages(null)

        BluetraceUtils.cancelBMUpdateCheck(this.applicationContext)
        BluetraceUtils.cancelNextScan(this.applicationContext)
        BluetraceUtils.cancelNextAdvertise(this.applicationContext)
    }

    private fun setupNotifications() {

        mNotificationManager = getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager

        // Android O requires a Notification Channel.
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val name = CHANNEL_SERVICE
            // Create the channel for the notification
            val mChannel =
                    NotificationChannel(CHANNEL_ID, name, NotificationManager.IMPORTANCE_LOW)
            mChannel.enableLights(false)
            mChannel.enableVibration(true)
            mChannel.vibrationPattern = longArrayOf(0L)
            mChannel.setSound(null, null)
            mChannel.setShowBadge(false)

            // Set the Notification Channel for the Notification Manager.
            mNotificationManager!!.createNotificationChannel(mChannel)
        }
    }

    private fun notifyLackingThings(override: Boolean = false) {
        if (notificationShown != NOTIFICATION_STATE.LACKING_THINGS || override) {
            val notif =
                    NotificationTemplates.lackingThingsNotification(this.applicationContext, CHANNEL_ID)
            startForeground(NOTIFICATION_ID, notif)
            notificationShown = NOTIFICATION_STATE.LACKING_THINGS
        }
    }

    private fun notifyRunning(override: Boolean = false) {
        if (notificationShown != NOTIFICATION_STATE.RUNNING || override) {
            val notif =
                    NotificationTemplates.getRunningNotification(this.applicationContext, CHANNEL_ID)
            startForeground(NOTIFICATION_ID, notif)
            notificationShown = NOTIFICATION_STATE.RUNNING
        }
    }

    private fun hasLocationPermissions(): Boolean {
        val perms = BluetraceUtils.getRequiredPermissions()
        return EasyPermissions.hasPermissions(this.applicationContext, *perms)
    }

    private fun acquireLocationPermission() {
        val intent = Intent(this.applicationContext, RequestLocationPermission::class.java)
        intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK
        startActivity(intent)
    }

    private fun acquireBluetoothPermission() {
        val intent = Intent(this.applicationContext, RequestBluetoothPermission::class.java)
        intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK
        startActivity(intent)
    }

    private fun validateDeviceID() {
        if(deviceID.isEmpty() || deviceID.isBlank()) {
            CentralLog.d(TAG, "[DeviceID] prepare to update")
            BluetraceUtils.setDeviceID(this.applicationContext)
            deviceID = Preference.getDeviceID(this.applicationContext)
        }
        CentralLog.d(TAG, "[DeviceID] $deviceID")
    }

    private fun validateTodayDate() {
        val now = System.currentTimeMillis()
        if(todayDate == 0L || ((todayDate + day) < now)) {
            CentralLog.d(TAG, "[TodayDate] prepare to update")
            BluetraceUtils.setToday(this.applicationContext)
            todayDate = Preference.getTodayDate(this.applicationContext)
        }
        CentralLog.d(TAG, "[TodayDate] ${BluetraceUtils.getDate(todayDate)}")
    }

    private fun isBluetoothEnabled(): Boolean {
        var btOn = false
        val bluetoothAdapter: BluetoothAdapter? by lazy(LazyThreadSafetyMode.NONE) {
            val bluetoothManager = getSystemService(Context.BLUETOOTH_SERVICE) as BluetoothManager
            bluetoothManager.adapter
        }

        bluetoothAdapter?.let {
            btOn = it.isEnabled
        }
        return btOn
    }

    override fun onStartCommand(intent: Intent?, flags: Int, startId: Int): Int {
        CentralLog.i(TAG, "Service onStartCommand")

        //check for permissions
        if (!isBluetoothEnabled()) {
            CentralLog.i(
                    TAG,
                    "bluetooth: ${isBluetoothEnabled()}"
            )
            acquireBluetoothPermission()
            notifyLackingThings()
            return START_STICKY
        }

        if (!hasLocationPermissions()) {
            CentralLog.i(
                    TAG,
                    "location permission: ${hasLocationPermissions()}"
            )
            acquireLocationPermission()
            notifyLackingThings()
            return START_STICKY
        }

        intent?.let {
            val cmd = intent.getIntExtra(COMMAND_KEY, Command.INVALID.index)
            runService(Command.findByValue(cmd))

            return START_STICKY
        }

        if (intent == null) {
            CentralLog.e(TAG, "WTF? Nothing in intent @ onStartCommand")
//            Utils.startBluetoothMonitoringService(applicationContext)
            commandHandler.startBluetoothMonitoringService()
        }

        // Tells the system to not try to recreate the service after it has been killed.
        return START_STICKY
    }

    fun runService(cmd: Command?) {
        CentralLog.i(TAG, "Command is:${cmd?.string}")

        //check for permissions
        if (!isBluetoothEnabled()) {
            CentralLog.i(
                    TAG,
                    "bluetooth: ${isBluetoothEnabled()}"
            )
            acquireBluetoothPermission()
            notifyLackingThings()
            return
        }

        if (!hasLocationPermissions()) {
            CentralLog.i(
                    TAG,
                    "location permission: ${hasLocationPermissions()}"
            )
            acquireLocationPermission()
            notifyLackingThings()
            return
        }

        //show running foreground notification if its not showing that
        notifyRunning()

        when (cmd) {
            Command.ACTION_START -> {
                setupService()
                BluetraceUtils.scheduleNextHealthCheck(this.applicationContext, healthCheckInterval)
                BluetraceUtils.scheduleRepeatingPurge(this.applicationContext, purgeInterval)
                BluetraceUtils.scheduleBMUpdateCheck(this.applicationContext, bmCheckInterval)
                actionStart()
            }

            Command.ACTION_SCAN -> {
                scheduleScan()
                actionScan()
            }

            Command.ACTION_ADVERTISE -> {
                scheduleAdvertisement()
                actionAdvertise()
            }

            Command.ACTION_UPDATE_BM -> {
                BluetraceUtils.scheduleBMUpdateCheck(this.applicationContext, bmCheckInterval)
                actionUpdateBm()
            }

            Command.ACTION_STOP -> {
                actionStop()
            }

            Command.ACTION_SELF_CHECK -> {
                BluetraceUtils.scheduleNextHealthCheck(this.applicationContext, healthCheckInterval)
                actionHealthCheck()
            }

            Command.ACTION_PURGE -> {
                actionPurge()
            }

            else -> CentralLog.i(TAG, "Invalid / ignored command: $cmd. Nothing to do")
        }
    }

    private fun actionStop() {
        stopForeground(true)
        stopSelf()
        CentralLog.w(TAG, "Service Stopping")
    }

    private fun actionHealthCheck() {
        performHealthCheck()
        BluetraceUtils.scheduleRepeatingPurge(this.applicationContext, purgeInterval)
    }

    private fun actionPurge() {
        performPurge()
    }

    private fun actionStart() {
        CentralLog.d(TAG, "Action Start")
        validateDeviceID()
        validateTodayDate()

        if (TempIDManager.needToUpdate(this.applicationContext)) {
            CentralLog.i(TAG, "[TempID] Need to update TemporaryID in actionStart")
            TempIDManager.getTemporaryIDs(this.applicationContext, Command.ACTION_START.string)
        } else if (TempIDManager.needToRollNewTempID(this.applicationContext) || broadcastMessage == null) {
            CentralLog.i(TAG, "[TempID] Need to pull TemporaryID in actionStart")
            BluetraceUtils.broadcastMessageReceived(this.applicationContext, Command.ACTION_START.string)
        } else {
            CentralLog.i(TAG, "[TempID] Don't need to update Temp ID in actionStart")
            setupCycles()
        }
    }

    private fun actionUpdateBm() {
        if (TempIDManager.needToUpdate(this.applicationContext)) {
            CentralLog.i(TAG, "[TempID] Need to update TemporaryID in actionUpdateBM")
            TempIDManager.getTemporaryIDs(this.applicationContext, Command.ACTION_UPDATE_BM.string)
        } else if (TempIDManager.needToRollNewTempID(this.applicationContext) || broadcastMessage == null) {
            CentralLog.i(TAG, "[TempID] Need to pull TemporaryID in actionUpdateBM")
            BluetraceUtils.broadcastMessageReceived(this.applicationContext, Command.ACTION_UPDATE_BM.string)
        } else {
            CentralLog.i(TAG, "[TempID] Don't need to update Temp ID in actionUpdateBM")
        }
    }

    private fun calcPhaseShift(min: Long, max: Long): Long {
        return (min + (Math.random() * (max - min))).toLong()
    }

    private fun actionScan() {
        validateTodayDate()
        if (TempIDManager.needToUpdate(this.applicationContext)) {
            CentralLog.i(TAG, "[TempID] Need to update TemporaryID in actionScan")
            TempIDManager.getTemporaryIDs(this.applicationContext, Command.ACTION_SCAN.string)
        } else if (TempIDManager.needToRollNewTempID(this.applicationContext) || broadcastMessage == null) {
            CentralLog.i(TAG, "[TempID] Need to pull TemporaryID in actionScan")
            BluetraceUtils.broadcastMessageReceived(this.applicationContext, Command.ACTION_SCAN.string)
        } else {
            CentralLog.i(TAG, "[TempID] Don't need to update Temp ID in actionScan")
            performScan()
        }
    }

    private fun actionAdvertise() {
        setupAdvertiser()
        if (isBluetoothEnabled()) {
            advertiser?.startAdvertising(advertisingDuration)
        } else {
            CentralLog.w(TAG, "Unable to start advertising, bluetooth is off")
        }
    }

    private fun setupService() {
        streetPassServer =
                streetPassServer ?: StreetPassServer(this.applicationContext, serviceUUID)
        setupScanner()
        setupAdvertiser()
    }

    private fun setupScanner() {
        streetPassScanner = streetPassScanner ?: StreetPassScanner(
                this,
                serviceUUID,
                scanDuration
        )
    }

    private fun setupAdvertiser() {
        advertiser = advertiser ?: BLEAdvertiser(serviceUUID)
    }

    fun setupCycles() {
        setupScanCycles()
        setupAdvertisingCycles()
    }

    private fun setupScanCycles() {
        commandHandler.scheduleNextScan(0)
    }

    private fun setupAdvertisingCycles() {
        commandHandler.scheduleNextAdvertise(0)
    }

    fun performScan() {
        setupScanner()
        startScan()
    }

    private fun scheduleScan() {
        commandHandler.scheduleNextScan(
                scanDuration + calcPhaseShift(
                        minScanInterval,
                        maxScanInterval
                )
        )
    }

    private fun scheduleAdvertisement() {
        commandHandler.scheduleNextAdvertise(advertisingDuration + advertisingGap)
    }

    private fun startScan() {

        if (isBluetoothEnabled()) {

            streetPassScanner?.let { scanner ->
                if (!scanner.isScanning()) {
                    scanner.startScan()
                } else {
                    CentralLog.e(TAG, "Already scanning!")
                }
            }
        } else {
            CentralLog.w(TAG, "Unable to start scan - bluetooth is off")
        }
    }

    private fun performHealthCheck() {

        CentralLog.i(TAG, "Performing self diagnosis")

        if (!hasLocationPermissions() || !isBluetoothEnabled()) {
            CentralLog.i(TAG, "no location permission")
            notifyLackingThings(true)
            return
        }

        notifyRunning(true)

        //ensure our service is there
        setupService()

        if (!commandHandler.hasScanScheduled()) {
            CentralLog.w(TAG, "Missing Scan Schedule - rectifying")
            commandHandler.scheduleNextScan(100)
        } else {
            CentralLog.w(TAG, "Scan Schedule present")
        }

        if (!commandHandler.hasAdvertiseScheduled()) {
            CentralLog.w(TAG, "Missing Advertise Schedule - rectifying")
//                setupAdvertisingCycles()
            commandHandler.scheduleNextAdvertise(100)
        } else {
            CentralLog.w(
                    TAG,
                    "Advertise Schedule present. Should be advertising?:  ${advertiser?.shouldBeAdvertising
                            ?: false}. Is Advertising?: ${advertiser?.isAdvertising ?: false}"
            )
        }


    }

    private fun performPurge() {
        val context = this
        launch {
            val before = System.currentTimeMillis() - purgeTTL
            val beforeTempID = System.currentTimeMillis() - (purgeTTL * 1000)
            CentralLog.i(TAG, "Coroutine - Purging of data before epoch time $before")

            streetPassRecordStorage.purgeOldRecords(before)
            statusRecordStorage.purgeOldRecords(before)
            temporaryIdStorage.purgeOldTemporaryIds(beforeTempID)
            Preference.putLastPurgeTime(context, System.currentTimeMillis())
        }
    }

    override fun onDestroy() {
        super.onDestroy()
        CentralLog.i(TAG, "BluetoothMonitoringService destroyed - tearing down")
        stopService()
        CentralLog.i(TAG, "BluetoothMonitoringService destroyed")
    }

    private fun stopService() {
        teardown()
        unregisterReceivers()

        worker?.terminateConnections()
        worker?.unregisterReceivers()

        job.cancel()
    }


    private fun registerReceivers() {
        val recordAvailableFilter = IntentFilter(ACTION_RECEIVED_STREETPASS)
        localBroadcastManager.registerReceiver(streetPassReceiver, recordAvailableFilter)

        val statusReceivedFilter = IntentFilter(ACTION_RECEIVED_STATUS)
        localBroadcastManager.registerReceiver(statusReceiver, statusReceivedFilter)

        val temporaryIdReceivedFilter = IntentFilter(ACTION_RECEIVED_TEMPORARY_ID)
        localBroadcastManager.registerReceiver(temporaryIdReceiver, temporaryIdReceivedFilter)

        val messageReceivedFilter = IntentFilter(ACTION_RECEIVED_MESSAGE)
        localBroadcastManager.registerReceiver(messageReceiver, messageReceivedFilter)

        val bluetoothStatusReceivedFilter = IntentFilter(BluetoothAdapter.ACTION_STATE_CHANGED)
        registerReceiver(bluetoothStatusReceiver, bluetoothStatusReceivedFilter)

        CentralLog.i(TAG, "Receivers registered")
    }

    private fun unregisterReceivers() {
        try {
            localBroadcastManager.unregisterReceiver(streetPassReceiver)
        } catch (e: Throwable) {
            CentralLog.w(TAG, "streetPassReceiver is not registered?")
        }

        try {
            localBroadcastManager.unregisterReceiver(statusReceiver)
        } catch (e: Throwable) {
            CentralLog.w(TAG, "statusReceiver is not registered?")
        }

        try {
            localBroadcastManager.unregisterReceiver(temporaryIdReceiver)
        } catch (e: Throwable) {
            CentralLog.w(TAG, "temporaryIdReceiver is not registered?")
        }

        try {
            localBroadcastManager.unregisterReceiver(messageReceiver)
        } catch (e: Throwable) {
            CentralLog.w(TAG, "messageReceiver is not registered?")
        }

        try {
            unregisterReceiver(bluetoothStatusReceiver)
        } catch (e: Throwable) {
            CentralLog.w(TAG, "bluetoothStatusReceiver is not registered?")
        }
    }

    override fun onBind(intent: Intent?): IBinder? {
        return null
    }

    inner class BluetoothStatusReceiver : BroadcastReceiver() {

        override fun onReceive(context: Context?, intent: Intent?) {
            intent?.let {
                val action = intent.action
                if (action == BluetoothAdapter.ACTION_STATE_CHANGED) {

                    when (intent.getIntExtra(BluetoothAdapter.EXTRA_STATE, -1)) {
                        BluetoothAdapter.STATE_TURNING_OFF -> {
                            CentralLog.d(TAG, "BluetoothAdapter.STATE_TURNING_OFF")
                            notifyLackingThings()
                            teardown()
                        }
                        BluetoothAdapter.STATE_OFF -> {
                            CentralLog.d(TAG, "BluetoothAdapter.STATE_OFF")
                        }
                        BluetoothAdapter.STATE_TURNING_ON -> {
                            CentralLog.d(TAG, "BluetoothAdapter.STATE_TURNING_ON")
                        }
                        BluetoothAdapter.STATE_ON -> {
                            CentralLog.d(TAG, "BluetoothAdapter.STATE_ON")
                            BluetraceUtils.startBluetoothMonitoringService(this@BluetoothMonitoringService.applicationContext)
                        }
                    }
                }
            }
        }
    }

    inner class StreetPassReceiver : BroadcastReceiver() {

        private val TAG = "StreetPassReceiver"

        override fun onReceive(context: Context, intent: Intent) {

            if (ACTION_RECEIVED_STREETPASS == intent.action) {
                val connRecord: ConnectionRecord? = intent.getParcelableExtra(STREET_PASS)
                val streetPassRecord: StreetPassRecord? = intent.getParcelableExtra(STREET_PASS_RECORD)
                if (connRecord !== null) {
                    CentralLog.d(
                            TAG,
                            "StreetPass received: $connRecord"
                    )


                    if (connRecord.msg.isNotEmpty()) {
                        val record = StreetPassRecord(
                                v = connRecord.version,
                                msg = connRecord.msg,
                                org = connRecord.org,
                                modelP = connRecord.peripheral.modelP,
                                modelC = connRecord.central.modelC,
                                rssi = connRecord.rssi,
                                txPower = connRecord.txPower,
                                aux = connRecord.aux
                        )

                        launch {
                            if (record.aux.isNullOrBlank()) {
                                CentralLog.d(
                                        TAG,
                                        "Coroutine - Saving StreetPassRecord: ${BluetraceUtils.getDate(record.timestamp)}"
                                )
                                streetPassRecordStorage.saveRecord(record)
                            } else {
                                StreetPassManager.validateRecord(this@BluetoothMonitoringService.applicationContext, record)
                            }
                        }
                    }
                } else if (streetPassRecord !== null) {
                    CentralLog.d(
                            TAG,
                            "StreetPass received: $streetPassRecord"
                    )
                    launch {
                        CentralLog.d(
                                TAG,
                                "Coroutine - Saving StreetPassRecord: ${BluetraceUtils.getDate(streetPassRecord.timestamp)}"
                        )
                        streetPassRecordStorage.saveRecord(streetPassRecord)
                    }
                }
            }
        }
    }

    inner class StatusReceiver : BroadcastReceiver() {
        private val TAG = "StatusReceiver"

        override fun onReceive(context: Context, intent: Intent) {

            if (ACTION_RECEIVED_STATUS == intent.action) {
                val statusRecord: Status = intent.getParcelableExtra(STATUS)!!
                CentralLog.d(TAG, "Status received: ${statusRecord.msg}")

                if (statusRecord.msg.isNotEmpty()) {
                    val statusRecordDB = StatusRecord(statusRecord.msg)
                    launch {
                        statusRecordStorage.saveRecord(statusRecordDB)
                    }
                }
            }
        }
    }

    inner class TemporaryIdReceiver : BroadcastReceiver() {
        private val TAG = "TemporaryIdReceiver"

        override fun onReceive(context: Context, intent: Intent) {

            if (ACTION_RECEIVED_TEMPORARY_ID == intent.action) {
                val temporaryIds: ArrayList<TemporaryID> = intent.getParcelableArrayListExtra(TEMPORARY_ID)!!
                CentralLog.d(TAG, "Temporary IDs received")

                if (temporaryIds.isNotEmpty()) {
                    val temporaryIdsDB = temporaryIds.map { it.toPersistence() }
                    launch {
                        temporaryIdStorage.saveTemporaryIds(temporaryIdsDB)
                    }
                }
            }
        }
    }

    inner class MessageReceiver : BroadcastReceiver() {
        private val TAG = "MessageReceiver"

        override fun onReceive(context: Context, intent: Intent) {

            if (ACTION_RECEIVED_MESSAGE == intent.action) {
                val action: String? = intent.getStringExtra(MESSAGE)
                CentralLog.d(TAG, "Message received")

                if (action.isNullOrEmpty()) {
                    val task = GetTemporaryIdTask()
                    task.execute(context)
                } else {
                    val task = GetTemporaryIdTask(action)
                    task.execute(context)
                }
            }
        }
    }

    class ValidateStreetPassRecordsTask(value: StreetPassRecord) : AsyncTask<Context, Void, Boolean>() {

        private val record: StreetPassRecord = value
        private lateinit var context: Context

        override fun doInBackground(vararg params: Context): Boolean {
            if (params.isNotEmpty()) {
                context = params[0]

                val records = StreetPassRecordStorage(context).getRecordsByAux(record.aux!!, todayDate)
                return records < BuildConfig.BLUETRACE_MAX_INTERVAL
            }
            return false
        }

        override fun onPostExecute(result: Boolean) {
            super.onPostExecute(result)

            if (result) {
                BluetraceUtils.broadcastStreetPassReceived(context, record)
            }
        }
    }

    class SendStreetPassRecordsTask(value: String) : AsyncTask<Context, Void, StreetPassRequest?>() {

        private val pin: String = value
        private lateinit var context: Context

        override fun doInBackground(vararg params: Context): StreetPassRequest? {
            if (params.isNotEmpty()) {
                context = params[0]
                val records = StreetPassRecordStorage(context).getAllRecords().map { it.fromPersistence() }
                return if (records.isNotEmpty()) {
                    StreetPassRequest(records, pin)
                } else {
                    Toast.makeText(context, context.getText(R.string.SEND_RECORDS_NOT_OK_MSG), Toast.LENGTH_LONG).show()
                    null
                }
            }
            return null
        }

        override fun onPostExecute(result: StreetPassRequest?) {
            super.onPostExecute(result)

            if (result !== null) {
                CentralLog.d(TAG, "StreetPassRecords is preparing to send with pin: ${result.pin}")
                BluetraceUtils.sendRecordsToServer(context, result)
            }
        }
    }

    @SuppressLint("StaticFieldLeak")
    inner class GetTemporaryIdTask : AsyncTask<Context, Void, TemporaryID?> {

        private val action: String

        constructor() {
            action = ""
        }

        constructor(value: String) {
            action = value
        }

        override fun doInBackground(vararg params: Context): TemporaryID? {
            if (params.isNotEmpty()) {
                val context = params[0]
                val currentTime = System.currentTimeMillis() / 1000
                val tempIDs = TemporaryIdStorage(context).getValidTemporaryId(currentTime)
                if (tempIDs.isNotEmpty()) {
                    val tempID = tempIDs.last().fromPersistence()
                    Preference.putExpiryTimeInMillis(
                            context,
                            tempID.expiryTime * 1000
                    )
                    return tempID
                }
                return null
            }
            return null
        }

        override fun onPostExecute(result: TemporaryID?) {
            super.onPostExecute(result)
            CentralLog.d(TAG, "Temporary ID get: ${result?.tempID}")
            broadcastMessage = result

            when (action) {
                Command.ACTION_START.string -> {
                    setupCycles()
                }
                Command.ACTION_SCAN.string -> {
                    performScan()
                }
            }
        }
    }

    enum class Command(val index: Int, val string: String) {
        INVALID(-1, "INVALID"),
        ACTION_START(0, "START"),
        ACTION_SCAN(1, "SCAN"),
        ACTION_STOP(2, "STOP"),
        ACTION_ADVERTISE(3, "ADVERTISE"),
        ACTION_SELF_CHECK(4, "SELF_CHECK"),
        ACTION_UPDATE_BM(5, "UPDATE_BM"),
        ACTION_PURGE(6, "PURGE");

        companion object {
            private val types = values().associateBy { it.index }
            fun findByValue(value: Int) = types[value]
        }
    }

    enum class NOTIFICATION_STATE {
        RUNNING,
        LACKING_THINGS
    }

    companion object {

        private const val TAG = "BTMService"

        private const val NOTIFICATION_ID = BuildConfig.SERVICE_FOREGROUND_NOTIFICATION_ID
        private const val CHANNEL_ID = BuildConfig.SERVICE_FOREGROUND_CHANNEL_ID
        const val CHANNEL_SERVICE = BuildConfig.SERVICE_FOREGROUND_CHANNEL_NAME

        const val PUSH_NOTIFICATION_ID = BuildConfig.PUSH_NOTIFICATION_ID

        const val COMMAND_KEY = "${BuildConfig.APPLICATION_ID}_CMD"

        const val PENDING_ACTIVITY = 5
        const val PENDING_START = 6
        const val PENDING_SCAN_REQ_CODE = 7
        const val PENDING_ADVERTISE_REQ_CODE = 8
        const val PENDING_HEALTH_CHECK_CODE = 9
        const val PENDING_WIZARD_REQ_CODE = 10
        const val PENDING_BM_UPDATE = 11
        const val PENDING_PURGE_CODE = 12

        var refreshBearerTokenManager: RefreshBearerTokenDelegate? = null
        var broadcastMessage: TemporaryID? = null
        var deviceID: String = ""
        var todayDate: Long = 0L
        const val day: Long = 86400000L

        //should be more than advertising gap?
        const val scanDuration: Long = BuildConfig.SCAN_DURATION
        const val minScanInterval: Long = BuildConfig.MIN_SCAN_INTERVAL
        const val maxScanInterval: Long = BuildConfig.MAX_SCAN_INTERVAL

        const val advertisingDuration: Long = BuildConfig.ADVERTISING_DURATION
        const val advertisingGap: Long = BuildConfig.ADVERTISING_INTERVAL

        const val maxQueueTime: Long = BuildConfig.MAX_QUEUE_TIME
        const val bmCheckInterval: Long = BuildConfig.BM_CHECK_INTERVAL
        const val healthCheckInterval: Long = BuildConfig.HEALTH_CHECK_INTERVAL
        const val purgeInterval: Long = BuildConfig.PURGE_INTERVAL
        const val purgeTTL: Long = BuildConfig.PURGE_TTL

        const val connectionTimeout: Long = BuildConfig.CONNECTION_TIMEOUT
        const val blacklistDuration: Long = BuildConfig.BLACKLIST_DURATION
    }
}

interface RefreshBearerTokenDelegate{

    fun toRefreshBearerToken(function: (result: Boolean) -> Unit)

    fun toChangeBearerToken(function: (result: Boolean) -> Unit)
}