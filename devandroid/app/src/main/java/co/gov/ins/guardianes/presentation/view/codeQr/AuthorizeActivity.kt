package co.gov.ins.guardianes.presentation.view.codeQr

import android.content.Intent
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import co.gov.ins.guardianes.R
import kotlinx.android.synthetic.main.activity_code_qr_authorize.*
import kotlinx.android.synthetic.main.base_toolbar.*
import org.koin.androidx.viewmodel.ext.android.viewModel

class AuthorizeActivity : AppCompatActivity() {

    private val codeQrViewModel: CodeQrViewModel by viewModel()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_code_qr_authorize)
        setupToolbar()
        setupView()
    }

    private fun setupView() {
        txvAuthorizeTitle.text =
            getString(R.string.code_qr_authorize_title, codeQrViewModel.getUser()?.firstName)
        bntAuthorizeDeclare.setOnClickListener {
            bntAuthorizeConfirm.isEnabled = bntAuthorizeDeclare.isChecked
        }
        bntAuthorizeConfirm.setOnClickListener {
            startActivity(Intent(this, ExceptionFormActivity::class.java))
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
        txtTitleBar.text = getString(R.string.code_qr_permission_title)
    }
}