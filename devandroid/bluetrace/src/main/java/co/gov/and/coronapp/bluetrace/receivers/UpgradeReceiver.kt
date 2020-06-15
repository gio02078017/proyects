package co.gov.and.coronapp.bluetrace.receivers

import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import co.gov.and.coronapp.bluetrace.BluetraceUtils
import co.gov.and.coronapp.bluetrace.logging.CentralLog

class UpgradeReceiver : BroadcastReceiver() {

    override fun onReceive(context: Context?, intent: Intent?) {

        try {
            if (Intent.ACTION_MY_PACKAGE_REPLACED != intent!!.action) return
            // Start your service here.
            context?.let {
                CentralLog.i("UpgradeReceiver", "Starting service from upgrade receiver")
                BluetraceUtils.startBluetoothMonitoringService(context)
            }
        } catch (e: Exception) {
            CentralLog.e("UpgradeReceiver", "Unable to handle upgrade: ${e.localizedMessage}")
        }
    }
}
