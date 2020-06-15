package co.gov.ins.guardianes.presentation.view.codeQr

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.view.codeQr.mobilityStatus.StatusCodeQrActivity
import co.gov.ins.guardianes.view.utils.fromHtml
import kotlinx.android.synthetic.main.activity_advice.*
import kotlinx.android.synthetic.main.activity_tip_qr.*
import kotlinx.android.synthetic.main.base_toolbar.*

class AdviceActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_advice)
        setupToolbar()
        setupView()

        bntTipConfirm.setOnClickListener {
            startActivity(Intent(this, StatusCodeQrActivity::class.java))
        }
    }


    private fun setupView() {

        tvStatusItems.fromHtml(getString(R.string.status_advice_items))
    }


    private fun setupToolbar() {
        setSupportActionBar(toolbar)
        supportActionBar?.run {
            setDisplayHomeAsUpEnabled(true)
            setDisplayShowHomeEnabled(true)
        }
        toolbar.setNavigationOnClickListener {
            onBackPressed()
        }
        txtTitleBar.text = getString(R.string.code_qr_status)

    }
}
