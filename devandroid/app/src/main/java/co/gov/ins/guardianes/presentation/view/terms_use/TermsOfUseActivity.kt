package co.gov.ins.guardianes.presentation.view.terms_use

import android.os.Bundle
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import kotlinx.android.synthetic.main.term.*

class TermsOfUseActivity : BaseAppCompatActivity() {

    override fun onCreate(bundle: Bundle?) {
        super.onCreate(bundle)
        setContentView(R.layout.term)
        initListener()
    }

    private fun initListener() {
        button_ok.setOnClickListener {
            finish()
        }

        toolbar.setNavigationOnClickListener {
            onBackPressed()
        }

        toolbar_exit.setOnClickListener {
            finish()
        }

    }
}