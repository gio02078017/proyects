package co.gov.ins.guardianes.presentation.view.user

import android.content.Intent
import android.os.Bundle
import android.view.View
import android.widget.ArrayAdapter
import android.widget.Toast
import androidx.lifecycle.Observer
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.helper.Constants.Url.DATA_TRATMENT
import co.gov.ins.guardianes.presentation.view.codeQr.DocumentPdfActivity
import co.gov.ins.guardianes.presentation.view.home.HomeActivity
import co.gov.ins.guardianes.presentation.view.smsCheck.CheckSmsActivity
import co.gov.ins.guardianes.presentation.view.terms_use.TermsOfUseActivity
import co.gov.ins.guardianes.util.Constants.CodeQR.PDF_URL
import co.gov.ins.guardianes.util.Constants.CodeQR.TXT_CONTENT
import co.gov.ins.guardianes.util.Constants.CodeQR.TXT_TOOLBAR
import co.gov.ins.guardianes.util.ext.getTypeDocument
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import co.gov.ins.guardianes.view.utils.fromHtml
import kotlinx.android.synthetic.main.activity_register.*
import kotlinx.android.synthetic.main.base_toolbar.view.*
import org.koin.androidx.viewmodel.ext.android.viewModel


class RegisterActivity : BaseAppCompatActivity() {

    private val userViewModel: UserViewModel by viewModel()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_register)
        ccp.registerPhoneNumberTextView(etEditPhone)
        initView()
        initObserver()
        initListener()
    }

    private fun initObserver() {
        userViewModel.getUserLiveDataState.observe(this, Observer {
            renderState(it)
        })
        userViewModel.validateRegister(this)
    }

    private fun initListener() {
        radioText.setOnClickListener {
            val intent = Intent(this, TermsOfUseActivity::class.java)
            startActivityForResult(intent, TERMS_USE)
        }

        txtHomeView.setOnClickListener {
            navigateTo(HomeActivity::class.java)
        }

        txtDataTreatment.setOnClickListener {
            val intent = Intent(this, DocumentPdfActivity::class.java).apply {
                putExtra(TXT_TOOLBAR, getString(R.string.text_treatment))
                putExtra(TXT_CONTENT, getString(R.string.text_treatment))
                putExtra(PDF_URL, DATA_TRATMENT)
            }
            startActivity(intent)
        }
        btnRegister.setOnClickListener {
            it.isEnabled = false
            requestUser()
        }
    }

    private fun requestUser() {
        userViewModel.registerUser(
                editName.text.toString(),
                editLastName.text.toString(),
                ccp.selectedCountryCodeWithPlus,
                ccp.phoneNumber.nationalNumber.toString(),
                editNumberDocument.text.toString(),
                spinnerTypeDocument.selectedItemPosition.getTypeDocument()
        )
    }

    private fun renderState(state: UserState) {
        progressBar.visibility = View.GONE
        when (state) {
            is UserState.Loading -> progressBar.visibility = View.VISIBLE

            is UserState.Success -> sendSms()

            is UserState.Error -> {
                btnRegister.isEnabled = true
                state.msg?.let {
                    Toast.makeText(this, it, Toast.LENGTH_LONG).show()
                } ?: run {
                    Toast.makeText(this, getString(R.string.error_default), Toast.LENGTH_LONG)
                            .show()
                }
            }
        }
    }

    private fun sendSms() {
        startActivity(Intent(this, CheckSmsActivity::class.java))
        //finish()
    }

    private fun initView() {
        iToolbar.run {
            onToolbar(toolbar)
            txtTitleBar.text = getString(R.string.registro)
            toolbar.setNavigationOnClickListener {
                onBackPressed()
            }
        }
        radioText.fromHtml(getString(R.string.message_terms))
        txtDataTreatment.fromHtml(getString(R.string.data_treatment))
        txtHomeView.fromHtml(getString(R.string.register_last))
        spinnerTypeDocument.adapter = ArrayAdapter(
                this,
                R.layout.spinner_layout,
                R.id.text,
                resources.getStringArray(R.array.type_document_array)
        )
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (requestCode == TERMS_USE) {
            radioTerm.isChecked = true
        }
    }

    override fun onBackPressed() {
        super.onBackPressed()
        startActivity(Intent(this, HomeActivity::class.java))
        finish()
    }

    companion object {
        const val TERMS_USE = 1010
    }
}
