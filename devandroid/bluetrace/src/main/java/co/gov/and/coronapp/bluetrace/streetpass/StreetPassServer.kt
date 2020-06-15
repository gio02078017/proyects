package co.gov.and.coronapp.bluetrace.streetpass

import android.content.Context
import co.gov.and.coronapp.bluetrace.bluetooth.gatt.GattServer
import co.gov.and.coronapp.bluetrace.bluetooth.gatt.GattService

class StreetPassServer constructor(val context: Context, private val serviceUUIDString: String) {

    private var gattServer: GattServer? = null

    init {
        gattServer = setupGattServer(context, serviceUUIDString)
    }

    private fun setupGattServer(context: Context, serviceUUIDString: String): GattServer? {
        val gattServer = GattServer(context, serviceUUIDString)
        val started = gattServer.startServer()

        if (started) {
            val readService = GattService(context, serviceUUIDString)
            gattServer.addService(readService)
            return gattServer
        }
        return null
    }

    fun tearDown() {
        gattServer?.stop()
    }

    fun checkServiceAvailable(): Boolean {
        return gattServer?.bluetoothGattServer?.services?.any {
            it.uuid.toString() == serviceUUIDString
        } ?: false
    }

}
