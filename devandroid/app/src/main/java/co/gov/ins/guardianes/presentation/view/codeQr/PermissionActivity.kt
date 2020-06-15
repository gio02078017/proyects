package co.gov.ins.guardianes.presentation.view.codeQr

import android.content.Intent
import android.os.Bundle
import android.view.View
import androidx.appcompat.app.AppCompatActivity
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.util.Constants
import co.gov.ins.guardianes.util.ext.makeLinks
import co.gov.ins.guardianes.view.utils.fromHtml
import kotlinx.android.synthetic.main.activity_code_qr_permission.*
import kotlinx.android.synthetic.main.base_toolbar.*
import org.koin.androidx.viewmodel.ext.android.viewModel

class PermissionActivity : AppCompatActivity() {

    private val codeQrViewModel: CodeQrViewModel by viewModel()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_code_qr_permission)
        setupToolbar()
        setupView()
    }

    private fun setupView() {
        txvPermissionName.text = codeQrViewModel.getUser()?.firstName
        txvPermissionContent.fromHtml(getString(R.string.code_qr_permission_content))
        txvPermissionContent2.fromHtml(getString(R.string.code_qr_permission_content2))
        txvPermissionContent.makeLinks(
            Pair(getString(R.string.code_qr_document_decree), View.OnClickListener {
                val text = getString(R.string.code_qr_document_decree)
                downloadPDF(
                    "$text del 2020",
                    "$text del 28 de mayo de 2020",
                    "del $text",
                    getString(R.string.code_qr_document_url_decree)
                )
            }),
            Pair(getString(R.string.code_qr_document_resolution), View.OnClickListener {
                val text = getString(R.string.code_qr_document_resolution)
                downloadPDF(
                    "$text del 2020",
                    "$text del 18 de marzo de 2020",
                    "de la $text",
                    getString(R.string.code_qr_document_url_resolution)
                )
            })
        )

        bntPermissionGenerate.setOnClickListener {
            startActivity(Intent(this, TipStatusActivity::class.java))
        }
    }

    private fun downloadPDF(
        txtToolbar: String,
        txtTitle: String,
        txtContent: String,
        urlPdf: String
    ) {
        val intent = Intent(this, DocumentPdfActivity::class.java).apply {
            putExtra(Constants.CodeQR.TXT_TOOLBAR, txtToolbar)
            putExtra(Constants.CodeQR.TXT_ACTIVITY, txtTitle)
            putExtra(Constants.CodeQR.TXT_CONTENT, txtContent)
            putExtra(Constants.CodeQR.PDF_URL, urlPdf)
        }
        startActivity(intent)
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