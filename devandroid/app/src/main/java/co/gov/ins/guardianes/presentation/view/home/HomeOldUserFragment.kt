package co.gov.ins.guardianes.presentation.view.home

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.view.user.RegisterActivity
import kotlinx.android.synthetic.main.home_old_user_fragment.*

class HomeOldUserFragment : Fragment() {
    override fun onCreateView(
            inflater: LayoutInflater, container: ViewGroup?,
            savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.home_old_user_fragment, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        btnUpdate.setOnClickListener {
            updateData()
        }
    }

    private fun updateData() {
        (activity as? HomeActivity)?.logout()
        startActivity(Intent(requireActivity(), RegisterActivity::class.java))
    }

    companion object {
        fun newInstance() = HomeOldUserFragment()
    }
}