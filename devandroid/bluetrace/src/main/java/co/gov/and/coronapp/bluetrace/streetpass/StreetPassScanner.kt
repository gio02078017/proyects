package co.gov.and.coronapp.bluetrace.streetpass

import android.bluetooth.le.ScanCallback
import android.bluetooth.le.ScanResult
import android.content.Context
import android.os.Build
import android.os.Handler
import co.gov.and.coronapp.bluetrace.BluetraceUtils
import co.gov.and.coronapp.bluetrace.bluetooth.BLEScanner
import co.gov.and.coronapp.bluetrace.logging.CentralLog
import co.gov.and.coronapp.bluetrace.status.Status
import kotlin.properties.Delegates

private const val TAG = "StreetPassScanner"

class StreetPassScanner constructor(
    context: Context,
    serviceUUIDString: String,
    private val scanDurationInMillis: Long
) {

    private var scanner: BLEScanner by Delegates.notNull()

    private var context: Context by Delegates.notNull()

    private var handler: Handler = Handler()

    var scannerCount = 0

    private val scanCallback = BleScanCallback()

//    var discoverer: BLEDiscoverer

    init {
        scanner = BLEScanner(context, serviceUUIDString, 0)
        this.context = context
//        discoverer = BLEDiscoverer(context, serviceUUIDString)
    }

    fun startScan() {

        val statusRecord = Status("Scanning Started")
        BluetraceUtils.broadcastStatusReceived(context, statusRecord)

        scanner.startScan(scanCallback)
        scannerCount++

        handler.postDelayed(
            { stopScan() }
            , scanDurationInMillis)

        CentralLog.d(TAG, "scanning started")
//        discoverer.startDiscovery()
    }

    fun stopScan() {
        //only stop if scanning was successful - kinda.
        if (scannerCount > 0) {
            val statusRecord = Status("Scanning Stopped")
            BluetraceUtils.broadcastStatusReceived(context, statusRecord)
            scannerCount--
            scanner.stopScan()
//        discoverer.cancelDiscovery()
        }
    }

    fun isScanning(): Boolean {
        return scannerCount > 0
    }

    inner class BleScanCallback : ScanCallback() {

        private val TAG = "BleScanCallback"

        private fun processScanResult(scanResult: ScanResult?) {

            scanResult?.let { result ->
                val device = result.device
                val rssi = result.rssi // get RSSI value

                var txPower: Int? = null

                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
                    txPower = result.txPower
                    if (txPower == 127) {
                        txPower = null
                    }
                }

                val manuData: ByteArray =
                    scanResult.scanRecord?.getManufacturerSpecificData(1023) ?: "N.A".toByteArray()
                val manuString = String(manuData, Charsets.UTF_8)

                val connectable = ConnectablePeripheral(manuString, txPower, rssi)

                CentralLog.i(TAG, "Scanned: $manuString - ${device.address}")

                BluetraceUtils.broadcastDeviceScanned(context, device, connectable)
            }
        }

        override fun onScanResult(callbackType: Int, result: ScanResult?) {
            super.onScanResult(callbackType, result)
            processScanResult(result)
        }

        override fun onScanFailed(errorCode: Int) {
            super.onScanFailed(errorCode)

            val reason = when (errorCode) {
                SCAN_FAILED_ALREADY_STARTED -> "$errorCode - SCAN_FAILED_ALREADY_STARTED"
                SCAN_FAILED_APPLICATION_REGISTRATION_FAILED -> "$errorCode - SCAN_FAILED_APPLICATION_REGISTRATION_FAILED"
                SCAN_FAILED_FEATURE_UNSUPPORTED -> "$errorCode - SCAN_FAILED_FEATURE_UNSUPPORTED"
                SCAN_FAILED_INTERNAL_ERROR -> "$errorCode - SCAN_FAILED_INTERNAL_ERROR"
                else -> {
                    "$errorCode - UNDOCUMENTED"
                }
            }
            CentralLog.e(TAG, "BT Scan failed: $reason")
            if (scannerCount > 0) {
                scannerCount--
            }
        }
    }


}

