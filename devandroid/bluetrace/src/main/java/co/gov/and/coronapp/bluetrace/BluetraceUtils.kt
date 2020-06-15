package co.gov.and.coronapp.bluetrace

import android.Manifest
import android.annotation.SuppressLint
import android.bluetooth.BluetoothAdapter
import android.bluetooth.BluetoothDevice
import android.content.Context
import android.content.Intent
import androidx.localbroadcastmanager.content.LocalBroadcastManager
import co.gov.and.coronapp.bluetrace.bluetooth.gatt.*
import co.gov.and.coronapp.bluetrace.idmanager.TemporaryID
import co.gov.and.coronapp.bluetrace.scheduler.Scheduler
import co.gov.and.coronapp.bluetrace.services.BluetoothMonitoringService
import co.gov.and.coronapp.bluetrace.services.BluetoothMonitoringService.Companion.PENDING_ADVERTISE_REQ_CODE
import co.gov.and.coronapp.bluetrace.services.BluetoothMonitoringService.Companion.PENDING_BM_UPDATE
import co.gov.and.coronapp.bluetrace.services.BluetoothMonitoringService.Companion.PENDING_HEALTH_CHECK_CODE
import co.gov.and.coronapp.bluetrace.services.BluetoothMonitoringService.Companion.PENDING_PURGE_CODE
import co.gov.and.coronapp.bluetrace.services.BluetoothMonitoringService.Companion.PENDING_SCAN_REQ_CODE
import co.gov.and.coronapp.bluetrace.services.BluetoothMonitoringService.Companion.PENDING_START
import co.gov.and.coronapp.bluetrace.services.RefreshBearerTokenDelegate
import co.gov.and.coronapp.bluetrace.status.Status
import co.gov.and.coronapp.bluetrace.streetpass.ACTION_DEVICE_SCANNED
import co.gov.and.coronapp.bluetrace.streetpass.ConnectablePeripheral
import co.gov.and.coronapp.bluetrace.streetpass.ConnectionRecord
import co.gov.and.coronapp.bluetrace.streetpass.StreetPassManager
import co.gov.and.coronapp.bluetrace.streetpass.persistence.StreetPassRecord
import co.gov.and.coronapp.bluetrace.streetpass.server.StreetPassRequest
import java.text.SimpleDateFormat
import java.util.*

object BluetraceUtils {

    fun getRequiredPermissions(): Array<String> {
        return arrayOf(Manifest.permission.ACCESS_FINE_LOCATION)
    }

    @SuppressLint("SimpleDateFormat")
    fun getDate(milliSeconds: Long): String {
        val dateFormat = "dd/MM/yyyy HH:mm:ss.SSS"
        // Create a DateFormatter object for displaying date in specified format.
        val formatter = SimpleDateFormat(dateFormat)

        // Create a calendar object that will convert the date and time value in milliseconds to date.
        val calendar = Calendar.getInstance()
        calendar.timeInMillis = milliSeconds
        return formatter.format(calendar.time)
    }

    private fun getToday(): Long {
        val today: Calendar = GregorianCalendar()
        today[Calendar.HOUR_OF_DAY] = 0
        today[Calendar.MINUTE] = 0
        today[Calendar.SECOND] = 0
        today[Calendar.MILLISECOND] = 0
        return today.timeInMillis
    }

    fun setAuthorizationToken(context: Context, token: String) {
        Preference.putAuthorization(context, token)
    }

    fun getAuthorizationToken(context: Context): String {
        return Preference.getAuthorization(context)
    }

    fun startBluetoothMonitoringService(context: Context) {
        val intent = Intent(context, BluetoothMonitoringService::class.java)
        intent.putExtra(
            BluetoothMonitoringService.COMMAND_KEY,
            BluetoothMonitoringService.Command.ACTION_START.index
        )
        context.startService(intent)
    }

    fun setTokenDelegate(delegate: RefreshBearerTokenDelegate?) {
        BluetoothMonitoringService.refreshBearerTokenManager = delegate
    }

    fun scheduleStartMonitoringService(context: Context, timeInMillis: Long) {
        val intent = Intent(context, BluetoothMonitoringService::class.java)
        intent.putExtra(
            BluetoothMonitoringService.COMMAND_KEY,
            BluetoothMonitoringService.Command.ACTION_START.index
        )

        Scheduler.scheduleServiceIntent(
            PENDING_START,
            context,
            intent,
            timeInMillis
        )
    }

    fun scheduleBMUpdateCheck(context: Context, bmCheckInterval: Long) {

        cancelBMUpdateCheck(context)

        val intent = Intent(context, BluetoothMonitoringService::class.java)
        intent.putExtra(
            BluetoothMonitoringService.COMMAND_KEY,
            BluetoothMonitoringService.Command.ACTION_UPDATE_BM.index
        )

        Scheduler.scheduleServiceIntent(
            PENDING_BM_UPDATE,
            context,
            intent,
            bmCheckInterval
        )
    }

    fun cancelBMUpdateCheck(context: Context) {
        val intent = Intent(context, BluetoothMonitoringService::class.java)
        intent.putExtra(
            BluetoothMonitoringService.COMMAND_KEY,
            BluetoothMonitoringService.Command.ACTION_UPDATE_BM.index
        )

        Scheduler.cancelServiceIntent(PENDING_BM_UPDATE, context, intent)
    }

    fun stopBluetoothMonitoringService(context: Context) {
        val intent = Intent(context, BluetoothMonitoringService::class.java)
        intent.putExtra(
            BluetoothMonitoringService.COMMAND_KEY,
            BluetoothMonitoringService.Command.ACTION_STOP.index
        )
        cancelNextScan(context)
        cancelNextHealthCheck(context)
        context.stopService(intent)
    }

    fun scheduleNextScan(context: Context, timeInMillis: Long) {

        //cancels any outstanding scan schedules.
        cancelNextScan(context)

        val nextIntent = Intent(context, BluetoothMonitoringService::class.java)
        nextIntent.putExtra(
            BluetoothMonitoringService.COMMAND_KEY,
            BluetoothMonitoringService.Command.ACTION_SCAN.index
        )
        //runs every XXX milliseconds
        Scheduler.scheduleServiceIntent(
            PENDING_SCAN_REQ_CODE,
            context,
            nextIntent,
            timeInMillis
        )
    }

    fun cancelNextScan(context: Context) {
        val nextIntent = Intent(context, BluetoothMonitoringService::class.java)
        nextIntent.putExtra(
            BluetoothMonitoringService.COMMAND_KEY,
            BluetoothMonitoringService.Command.ACTION_SCAN.index
        )
        Scheduler.cancelServiceIntent(PENDING_SCAN_REQ_CODE, context, nextIntent)
    }

    fun scheduleNextAdvertise(context: Context, timeToNextAdvertise: Long) {

        //cancels any outstanding scan schedules.
        cancelNextAdvertise(context)

        val nextIntent = Intent(context, BluetoothMonitoringService::class.java)
        nextIntent.putExtra(
            BluetoothMonitoringService.COMMAND_KEY,
            BluetoothMonitoringService.Command.ACTION_ADVERTISE.index
        )
        //runs every XXX milliseconds
        Scheduler.scheduleServiceIntent(
            PENDING_ADVERTISE_REQ_CODE,
            context,
            nextIntent,
            timeToNextAdvertise
        )
    }

    fun cancelNextAdvertise(context: Context) {
        val nextIntent = Intent(context, BluetoothMonitoringService::class.java)
        nextIntent.putExtra(
            BluetoothMonitoringService.COMMAND_KEY,
            BluetoothMonitoringService.Command.ACTION_ADVERTISE.index
        )
        Scheduler.cancelServiceIntent(PENDING_ADVERTISE_REQ_CODE, context, nextIntent)
    }

    fun scheduleNextHealthCheck(context: Context, timeInMillis: Long) {
        //cancels any outstanding check schedules.
        cancelNextHealthCheck(context)

        val nextIntent = Intent(context, BluetoothMonitoringService::class.java)
        nextIntent.putExtra(
            BluetoothMonitoringService.COMMAND_KEY,
            BluetoothMonitoringService.Command.ACTION_SELF_CHECK.index
        )
        //runs every XXX milliseconds - every minute?
        Scheduler.scheduleServiceIntent(
            PENDING_HEALTH_CHECK_CODE,
            context,
            nextIntent,
            timeInMillis
        )
    }

    private fun cancelNextHealthCheck(context: Context) {
        val nextIntent = Intent(context, BluetoothMonitoringService::class.java)
        nextIntent.putExtra(
            BluetoothMonitoringService.COMMAND_KEY,
            BluetoothMonitoringService.Command.ACTION_SELF_CHECK.index
        )
        Scheduler.cancelServiceIntent(PENDING_HEALTH_CHECK_CODE, context, nextIntent)
    }

    fun scheduleRepeatingPurge(context: Context, intervalMillis: Long) {
        val nextIntent = Intent(context, BluetoothMonitoringService::class.java)
        nextIntent.putExtra(
            BluetoothMonitoringService.COMMAND_KEY,
            BluetoothMonitoringService.Command.ACTION_PURGE.index
        )

        Scheduler.scheduleRepeatingServiceIntent(
            PENDING_PURGE_CODE,
            context,
            nextIntent,
            intervalMillis
        )
    }

    fun broadcastDeviceScanned(
        context: Context,
        device: BluetoothDevice,
        connectableBleDevice: ConnectablePeripheral
    ) {
        val intent = Intent(ACTION_DEVICE_SCANNED)
        intent.putExtra(BluetoothDevice.EXTRA_DEVICE, device)
        intent.putExtra(CONNECTION_DATA, connectableBleDevice)
        LocalBroadcastManager.getInstance(context).sendBroadcast(intent)
    }

    fun broadcastDeviceProcessed(context: Context, deviceAddress: String) {
        val intent = Intent(ACTION_DEVICE_PROCESSED)
        intent.putExtra(DEVICE_ADDRESS, deviceAddress)
        LocalBroadcastManager.getInstance(context).sendBroadcast(intent)
    }


    fun broadcastStreetPassReceived(context: Context, streetpass: ConnectionRecord) {
        val intent = Intent(ACTION_RECEIVED_STREETPASS)
        intent.putExtra(STREET_PASS, streetpass)
        LocalBroadcastManager.getInstance(context).sendBroadcast(intent)
    }

    fun broadcastStreetPassReceived(context: Context, streetpass: StreetPassRecord) {
        val intent = Intent(ACTION_RECEIVED_STREETPASS)
        intent.putExtra(STREET_PASS_RECORD, streetpass)
        LocalBroadcastManager.getInstance(context).sendBroadcast(intent)
    }

    fun broadcastStatusReceived(context: Context, statusRecord: Status) {
        val intent = Intent(ACTION_RECEIVED_STATUS)
        intent.putExtra(STATUS, statusRecord)
        LocalBroadcastManager.getInstance(context).sendBroadcast(intent)
    }

    fun broadcastTemporaryIdReceived(context: Context, temporaryID: ArrayList<TemporaryID>) {
        val intent = Intent(ACTION_RECEIVED_TEMPORARY_ID)
        intent.putExtra(TEMPORARY_ID, temporaryID)
        LocalBroadcastManager.getInstance(context).sendBroadcast(intent)
    }

    fun broadcastMessageReceived(context: Context, action: String) {
        val intent = Intent(ACTION_RECEIVED_MESSAGE)
        intent.putExtra(MESSAGE, action)
        LocalBroadcastManager.getInstance(context).sendBroadcast(intent)
    }

    fun broadcastMessageReceived(context: Context) {
        val intent = Intent(ACTION_RECEIVED_MESSAGE)
        LocalBroadcastManager.getInstance(context).sendBroadcast(intent)
    }

    fun sendTraces(context: Context) {
        val task = BluetoothMonitoringService.SendStreetPassRecordsTask("")
        task.execute(context)
    }

    fun sendTraces(context: Context, pin: String) {
        val task = BluetoothMonitoringService.SendStreetPassRecordsTask(pin)
        task.execute(context)
    }

    fun sendRecordsToServer(context: Context, request: StreetPassRequest) {
        StreetPassManager.sendRecords(context, request)
    }

    fun isBluetoothAvailable(): Boolean {
        val bluetoothAdapter = BluetoothAdapter.getDefaultAdapter()
        return bluetoothAdapter != null &&
                bluetoothAdapter.isEnabled && bluetoothAdapter.state == BluetoothAdapter.STATE_ON
    }

    fun setDeviceID(context: Context) {
        var deviceID = Preference.getDeviceID(context)
        if (deviceID.isNullOrEmpty()) {
            deviceID = UUID.randomUUID().toString()
            Preference.putDeviceID(context, deviceID)
        }
    }

    fun setToday(context: Context) {
        Preference.putTodayDate(context, getToday())
    }
}
