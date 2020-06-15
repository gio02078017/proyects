package co.gov.ins.guardianes.presentation.view.splash

import android.os.Bundle
import android.os.Handler
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.helper.Constants
import co.gov.ins.guardianes.manager.PrefManager
import co.gov.ins.guardianes.presentation.view.welcome.WelcomeIntro
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity

class SplashActivity : BaseAppCompatActivity(), Runnable {

    private val handler = Handler()

    override fun onCreate(bundle: Bundle?) {
        super.onCreate(bundle)
        setContentView(R.layout.splash)
        setNotifyNewVersion()
    }

    override fun onResume() {
        super.onResume()
        handler.postDelayed(this, WAIT_TIME)
    }

    override fun onPause() {
        super.onPause()
        handler.removeCallbacks(this)
    }

    override fun run() {
        navigateNewTaskTo(WelcomeIntro::class.java)
    }

    private fun setNotifyNewVersion() {
        PrefManager(this).putBoolean(Constants.Bundle.NEW_VERSION_NOTIFIED, false)
    }

    companion object {
        private const val WAIT_TIME = 1000L
    }
}