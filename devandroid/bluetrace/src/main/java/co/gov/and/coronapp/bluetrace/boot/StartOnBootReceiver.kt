package co.gov.and.coronapp.bluetrace.boot

import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import co.gov.and.coronapp.bluetrace.BluetraceUtils
import co.gov.and.coronapp.bluetrace.logging.CentralLog

class StartOnBootReceiver : BroadcastReceiver() {
    override fun onReceive(context: Context, intent: Intent) {

        if (Intent.ACTION_BOOT_COMPLETED == intent.action) {
            CentralLog.d("StartOnBootReceiver", "boot completed received")

            try {
                //can i try a scheduled service start here?
                CentralLog.d("StartOnBootReceiver", "Attempting to start service")
                BluetraceUtils.scheduleStartMonitoringService(context, 500)
            } catch (e: Throwable) {
                CentralLog.e("StartOnBootReceiver", e.localizedMessage!!)
                e.printStackTrace()
            }

        }
    }
}
