package co.gov.ins.guardianes.presentation.view.home

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.domain.repository.UserPreferences
import co.gov.ins.guardianes.presentation.view.smsCheck.CheckSmsActivity
import co.gov.ins.guardianes.presentation.view.user.RegisterActivity
import kotlinx.android.synthetic.main.activity_check_sms.*
import kotlinx.android.synthetic.main.home_activity.*
import kotlinx.android.synthetic.main.home_not_login_fragment.*
import org.koin.android.ext.android.inject

class HomeNotLoginFragment : Fragment() {

    private val prefUser: UserPreferences by inject()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.home_not_login_fragment, container, false)
    }

    override fun onResume() {
        super.onResume()
        btnSignUp.setOnClickListener {
            prefUser.getUser()?.let {
                startActivity(Intent(requireActivity(), CheckSmsActivity::class.java))
            } ?: run {
                startActivity(Intent(requireActivity(), RegisterActivity::class.java))
            }
        }
    }

    companion object {
        fun newInstance() = HomeNotLoginFragment()
    }
}
