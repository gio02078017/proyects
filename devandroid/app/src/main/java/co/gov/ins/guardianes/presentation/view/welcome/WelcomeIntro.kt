package co.gov.ins.guardianes.presentation.view.welcome

import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.Observer
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.helper.Constants
import co.gov.ins.guardianes.manager.PrefManager
import co.gov.ins.guardianes.presentation.view.home.HomeActivity
import co.gov.ins.guardianes.presentation.view.smsCheck.CheckSmsActivity
import kotlinx.android.synthetic.main.welcome_intro.*
import org.koin.androidx.viewmodel.ext.android.viewModel

class WelcomeIntro : AppCompatActivity() {

    private val welcomeViewModel: WelcomeViewModel by viewModel()
    private var typeView = TYPE_WELCOME

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.welcome_intro)
        initViewModel()
        initBind()
    }

    override fun onResume() {
        super.onResume()
        getTypeView()
    }

    private fun initBind() {
        btnGo.setOnClickListener {
            validView()
        }
    }

    private fun initViewModel() {
        welcomeViewModel.getWelcomeLiveDataState.observe(this, Observer {
            renderState(it)
        })
    }

    private fun renderState(state: WelcomeState) {
        when (state) {
            is WelcomeState.SuccessToken -> {
                typeView = TYPE_HOME
            }

            is WelcomeState.Error -> {
                Toast.makeText(this, state.msg ?: "Error", Toast.LENGTH_LONG).show()
            }
        }
    }

    private fun getTypeView() {
        val userNew = welcomeViewModel.getUser()
        userNew?.let {
            typeView = when {
                !welcomeViewModel.isTokenRegister() && !welcomeViewModel.isTokenNew() -> VALIDATE_CODE

                !welcomeViewModel.isTokenNew() -> {
                    welcomeViewModel.getToken()
                    WAIT
                }

                else -> TYPE_HOME
            }
        } ?: run {
            typeView = TYPE_WELCOME
            setNotifyUpdate()
            setNotifyNewTerms()
        }
    }

    private fun validView() {
        when (typeView) {
            TYPE_WELCOME -> {
                if (isOnboardingViewed() && isOnboardingUpdate()) {
                    startActivity(Intent(this, HomeActivity::class.java))
                } else {
                    startActivity(Intent(this, WelcomeActivity::class.java))
                }
            }
            TYPE_HOME -> {
                startActivity(Intent(this, HomeActivity::class.java))
            }
            VALIDATE_CODE -> {
                startActivity(Intent(this, CheckSmsActivity::class.java))
            }
        }
    }

    private fun isOnboardingViewed(): Boolean {
        return PrefManager(this).getBoolean(Constants.Bundle.ONBOARDING_VIEWED, false)
    }

    private fun isOnboardingUpdate(): Boolean {
        return PrefManager(this).getBoolean(Constants.Bundle.ONBOARDING_UPDATE, false)
    }

    private fun setNotifyUpdate() {
        PrefManager(this).putBoolean(Constants.Bundle.UPDATE_NOTIFIED, true)
    }

    private fun setNotifyNewTerms() {
        PrefManager(this).putBoolean(Constants.Bundle.TERM_NOTIFIED, true)
    }

    companion object {
        private const val TYPE_WELCOME = 1
        private const val TYPE_HOME = 2
        private const val VALIDATE_CODE = 3
        private const val WAIT = 4
    }
}