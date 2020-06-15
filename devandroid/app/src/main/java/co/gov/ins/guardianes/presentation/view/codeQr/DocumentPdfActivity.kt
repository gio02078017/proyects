package co.gov.ins.guardianes.presentation.view.codeQr

import android.content.ActivityNotFoundException
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.util.Constants
import co.gov.ins.guardianes.view.utils.fromHtml
import kotlinx.android.synthetic.main.activity_code_qr_pdf.*
import kotlinx.android.synthetic.main.base_toolbar.*

class DocumentPdfActivity : AppCompatActivity() {

    private val txtToolbar
        get() = intent.extras?.let {
            it.getString(
                Constants.CodeQR.TXT_TOOLBAR,
                getString(R.string.code_qr_permission_title)
            ) as String
        }
    private val txtTitle
        get() = intent.extras?.let {
            it.getString(Constants.CodeQR.TXT_ACTIVITY, "") as String
        }
    private val txtContent
        get() = intent.extras?.let {
            it.getString(Constants.CodeQR.TXT_CONTENT, "") as String
        }
    private val urlPdf
        get() = intent.extras?.let {
            it.getString(Constants.CodeQR.PDF_URL, "") as String
        }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_code_qr_pdf)
        setupToolbar()
        setupView()
        setupListeners()
    }

    private fun setupListeners() {
        toolbar.setNavigationOnClickListener {
            onBackPressed()
        }

        btnDocumentDownload.setOnClickListener {
            try {
                val intent = Intent(Intent.ACTION_VIEW).apply {
                    setDataAndType(Uri.parse(urlPdf), "application/pdf")
                }
                startActivity(intent)
            } catch (ignore: ActivityNotFoundException) {
                val intent = Intent(Intent.ACTION_VIEW, Uri.parse(urlPdf))
                startActivity(intent)
            }
        }
    }

    private fun setupView() {
        txtTitleBar.text = txtToolbar
        txvDocumentContent.fromHtml(getString(R.string.code_qr_document_content, txtContent))
    }

    private fun setupToolbar() {
        setSupportActionBar(toolbar)
        supportActionBar?.run {
            setDisplayHomeAsUpEnabled(true)
            setDisplayShowHomeEnabled(true)
        }
    }
}