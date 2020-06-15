package co.gov.and.coronapp.bluetrace.permissions

import android.app.Activity
import android.bluetooth.BluetoothAdapter
import android.bluetooth.BluetoothManager
import android.content.Context
import android.content.Intent
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import co.gov.and.coronapp.bluetrace.BluetraceUtils
import co.gov.and.coronapp.bluetrace.logging.CentralLog

class RequestBluetoothPermission : AppCompatActivity() {

    private val bluetoothAdapter: BluetoothAdapter? by lazy(LazyThreadSafetyMode.NONE) {
        val bluetoothManager = getSystemService(Context.BLUETOOTH_SERVICE) as BluetoothManager
        bluetoothManager.adapter
    }

    private val BluetoothAdapter.isDisabled: Boolean
        get() = !isEnabled

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableBluetooth()
    }

    private fun enableBluetooth() {
        CentralLog.d(TAG, "[enableBluetooth]")
        bluetoothAdapter?.let {
            if (it.isDisabled) {
                val enableBtIntent = Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE)
                startActivityForResult(
                        enableBtIntent,
                        REQUEST_ENABLE_BT
                )
            } else {
                BluetraceUtils.stopBluetoothMonitoringService(this)
                finish()
            }
        }
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        CentralLog.d(TAG, "requestCode $requestCode resultCode $resultCode")
        if (requestCode == REQUEST_ENABLE_BT) {
            if (resultCode == Activity.RESULT_CANCELED) {
                BluetraceUtils.stopBluetoothMonitoringService(this)
                finish()
                return
            } else {
                BluetraceUtils.startBluetoothMonitoringService(this)
                finish()
            }
        }
        super.onActivityResult(requestCode, resultCode, data)
    }

    companion object {
        private const val TAG = "PermissionBT"
        private const val REQUEST_ENABLE_BT = 123
    }
}