package co.gov.ins.guardianes.presentation.view.codeQr

import android.content.Intent
import android.os.Bundle
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.view.utils.fromHtml
import kotlinx.android.synthetic.main.activity_code_qr_permission.*
import kotlinx.android.synthetic.main.activity_tip_qr.*
import kotlinx.android.synthetic.main.base_toolbar.*
import org.koin.androidx.viewmodel.ext.android.viewModel

class TipStatusActivity : AppCompatActivity() {

    private val codeQrViewModel: CodeQrViewModel by viewModel()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_tip_qr)
        setupToolbar()
        setupView()
    }

    private fun setupView() {
        txvName.text = codeQrViewModel.getUser()?.firstName
        txvTipContent.fromHtml(getString(R.string.code_qr_tip_text))

        bntTipGenerate.setOnClickListener {
            alertDialog()
        }
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

    private fun alertDialog(  ) {
        AlertDialog.Builder(this, R.style.AlertDialogTheme).apply {
            setTitle(getString(R.string.alerta))
            setMessage(getString(R.string.alert_tex_tip))
            setCancelable(false)
            setPositiveButton(getString(R.string.agree)) { _, _ ->
                startActivity(Intent(applicationContext, AuthorizeActivity::class.java))
            }
        }.show()
    }


}