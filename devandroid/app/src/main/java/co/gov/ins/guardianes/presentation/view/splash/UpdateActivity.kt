package co.gov.ins.guardianes.presentation.view.splash

import android.content.ActivityNotFoundException
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.helper.Constants
import co.gov.ins.guardianes.manager.PrefManager
import kotlinx.android.synthetic.main.splash_update_app.*

class UpdateActivity : AppCompatActivity() {

    override fun onCreate(bundle: Bundle?) {
        super.onCreate(bundle)
        setContentView(R.layout.splash_update_app)
        getListeners()
        setNotifyNewVersion()
    }

    private fun getListeners() {
        btnUpdate.setOnClickListener {
            updateApp()
        }

        toolbar_exit.setOnClickListener {
            finish()
        }
    }

    private fun updateApp() {
        try {
            startActivity(
                    Intent(
                            Intent.ACTION_VIEW,
                            Uri.parse("market://details?id=$packageName&hl=en_US")
                    )
            )
        } catch (e: ActivityNotFoundException) {
            startActivity(
                    Intent(
                            Intent.ACTION_VIEW,
                            Uri.parse("https://play.google.com/store/apps/details?id=$packageName&hl=en_US")
                    )
            )
        }
    }

    private fun setNotifyNewVersion() {
        PrefManager(this).putBoolean(Constants.Bundle.NEW_VERSION_NOTIFIED, true)
    }
}