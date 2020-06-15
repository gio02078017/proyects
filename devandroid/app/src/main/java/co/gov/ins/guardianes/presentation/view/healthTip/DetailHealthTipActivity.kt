package co.gov.ins.guardianes.presentation.view.healthTip

import android.content.ActivityNotFoundException
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.models.HealthTip
import co.gov.ins.guardianes.util.Constants
import co.gov.ins.guardianes.util.ext.fromHtml
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import kotlinx.android.synthetic.main.activity_detail_health_tip.*
import kotlinx.android.synthetic.main.base_toolbar.*

class DetailHealthTipActivity : BaseAppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_detail_health_tip)
        initToolbar()
        initBind()
    }

    private fun initToolbar() {
        onToolbar(toolbar)
        toolbar.setNavigationOnClickListener {
            onBackPressed()
        }
        txtTitleBar.text = getString(R.string.tip_home_care)
    }

    private fun initBind() {
        val tip = intent.getParcelableExtra(Constants.Key.TIP) as HealthTip?
        txvTitleTip.text = tip?.title
        appCompatTextView.text =
            fromHtml(getString(R.string.description_detail_tip_home_care, tip?.title))
        btnPdf.setOnClickListener {
            tip?.document?.let {
                try {
                    val intent = Intent(Intent.ACTION_VIEW).apply {
                        setDataAndType(Uri.parse(it), "application/pdf")
                    }
                    startActivity(intent)
                } catch (ignore: ActivityNotFoundException) {
                    val intent = Intent(Intent.ACTION_VIEW, Uri.parse(it))
                    startActivity(intent)
                }
            }
        }
    }
}
