package co.gov.and.coronapp.bluetrace

import android.app.Application
import android.content.Context
import android.os.Build
import co.gov.and.coronapp.bluetrace.streetpass.CentralDevice
import co.gov.and.coronapp.bluetrace.streetpass.PeripheralDevice


class TracerApp : Application() {

    override fun onCreate() {
        super.onCreate()

        AppContext = applicationContext
        BluetraceUtils.broadcastMessageReceived(AppContext)
    }

    companion object {

        const val ORG = BuildConfig.ORG

        lateinit var AppContext: Context

        fun asPeripheralDevice(): PeripheralDevice {
            return PeripheralDevice(Build.MODEL, "SELF")
        }

        fun asCentralDevice(): CentralDevice {
            return CentralDevice(Build.MODEL, "SELF")
        }
    }
}
