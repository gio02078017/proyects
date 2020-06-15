package co.gov.ins.guardianes.presentation.view.welcome

import android.Manifest
import android.content.Intent
import android.content.pm.PackageManager
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.helper.Constants
import co.gov.ins.guardianes.manager.PrefManager
import co.gov.ins.guardianes.presentation.view.home.HomeActivity
import co.gov.ins.guardianes.presentation.view.user.RegisterActivity
import co.gov.ins.guardianes.view.utils.fromHtml
import kotlinx.android.synthetic.main.tutorial1.*
import kotlinx.android.synthetic.main.tutorial2.*
import kotlinx.android.synthetic.main.tutorial3.*
import kotlinx.android.synthetic.main.tutorial4.*

class WelcomePageFragment : Fragment() {

    override fun onCreateView(
            inflater: LayoutInflater,
            viewGroup: ViewGroup?,
            bundle: Bundle?
    ): View? {
        val welcome = arguments?.getSerializable(Constants.Bundle.WELCOME) as Welcome

        return inflater.inflate(welcome.layout, viewGroup, false)
    }

    fun bnHome() {
        btEmpezar.setOnClickListener {
            setOnBoardingUpdate()
            setOnBoardingViewed()
            setPermission()
        }
    }

    private fun setOnBoardingViewed() {
        val prefManager = PrefManager(context)
        prefManager.putBoolean(Constants.Bundle.ONBOARDING_VIEWED, true)
    }

    private fun setOnBoardingUpdate() {
        val prefManager = PrefManager(context)
        prefManager.putBoolean(Constants.Bundle.ONBOARDING_UPDATE, true)
    }

    private fun setPermission() {
        if (ContextCompat.checkSelfPermission(
                        activity!!,
                        Manifest.permission.ACCESS_FINE_LOCATION
                ) != PackageManager.PERMISSION_GRANTED
        ) {
            requestPermissions(
                    arrayOf(
                            Manifest.permission.ACCESS_FINE_LOCATION,
                            Manifest.permission.ACCESS_COARSE_LOCATION
                    ), MY_PERMISSIONS_REQUEST_LOCATION
            )
        } else {
            goActivity(FLAG_HOME)
        }
    }


    override fun onRequestPermissionsResult(
            requestCode: Int,
            permissions: Array<out String>,
            grantResults: IntArray
    ) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        when (requestCode) {
            MY_PERMISSIONS_REQUEST_LOCATION -> {
                if ((grantResults.isNotEmpty() && grantResults[0] == PackageManager.PERMISSION_GRANTED)) {
                    goActivity(FLAG_HOME)
                } else {
                    goActivity(FLAG_REGISTER)
                }
                return
            }
            else -> Unit
        }
    }

    private fun goActivity(flag: Int) {
        val intent = when (flag) {
            FLAG_HOME -> Intent(context, RegisterActivity::class.java)
            else -> Intent(context, HomeActivity::class.java)
        }
        startActivity(intent)
        activity?.finish()
    }

    fun getHtml1() =
            tuto_text1.fromHtml(getString(R.string.tuto_text1))

    fun getHtml2() =
            tuto_text2.fromHtml(getString(R.string.tuto_text2))

    fun getHtml3() =
            tuto_text3.fromHtml(getString(R.string.tuto_text3))

    fun getHtml4() =
            tuto_text4.fromHtml(getString(R.string.tuto_text4))

    companion object {
        private const val MY_PERMISSIONS_REQUEST_LOCATION = 12000
        private const val FLAG_HOME = 1
        private const val FLAG_REGISTER = 2
    }
}