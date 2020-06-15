package co.gov.ins.guardianes.presentation.view.codeQr

import android.content.Intent
import android.os.Bundle
import android.text.Editable
import android.text.TextWatcher
import android.view.View
import android.view.WindowManager
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import co.gov.ins.guardianes.R
import androidx.lifecycle.Observer
import co.gov.ins.guardianes.presentation.models.LegalPerson
import co.gov.ins.guardianes.presentation.view.codeQr.mobilityStatus.StatusCodeQrActivity
import co.gov.ins.guardianes.presentation.view.semaphone.SemaphoreActivity
import co.gov.ins.guardianes.util.Constants.Key.EXCEPTION
import kotlinx.android.synthetic.main.activity_person_type.*
import kotlinx.android.synthetic.main.base_toolbar.*
import kotlinx.android.synthetic.main.base_toolbar.toolbar
import org.koin.androidx.viewmodel.ext.android.viewModel


class PersonTypeActivity : AppCompatActivity() {

    private val codeQrViewModel: CodeQrViewModel by viewModel()
    private var exceptions = emptyArray<String>()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_person_type)
        exceptions = intent?.extras?.getStringArray(EXCEPTION)!!
        setupToolbar()
        setupView()
        initObservable()
    }

    private fun initObservable() {
        codeQrViewModel.codeQrLiveData.observe(this, Observer {
            renderState(it)
        })
    }

    private fun setupView() {
        bntTipConfirm.setOnClickListener {
            progressBar.visibility = View.VISIBLE
            val typePerson = if (cbPersonaNatural.isChecked)  cbPersonaNatural.text.toString() else cbPersonaJuridica.text.toString()
            val workPlace = if (cbCompany.isChecked) cbCompany.text.toString() else cbCustomer.text.toString()
            codeQrViewModel.generateQr(exceptions.toList(), LegalPerson(
                    typePerson,
                    editName.text.toString(),
                    editNit.text.toString(),
                    workPlace,
                    editCustomerNit.text.toString()))
        }

        editName.addTextChangedListener(object : TextWatcher {
            override fun afterTextChanged(p0: Editable?) {
                validateButton()
            }

            override fun beforeTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) {
            }

            override fun onTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) {
            }
        })

        editNit.addTextChangedListener(object : TextWatcher {
            override fun afterTextChanged(p0: Editable?) {
                validateButton()
            }

            override fun beforeTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) {
            }

            override fun onTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) {
            }
        })

        editCustomerNit.addTextChangedListener(object : TextWatcher {
            override fun afterTextChanged(p0: Editable?) {
                validateButton()
            }

            override fun beforeTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) {
            }

            override fun onTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) {
            }
        })

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

    private fun closeView1() {
        questionLayout2.visibility = View.GONE
        questionLayout3.visibility = View.GONE
        editName?.text?.clear()
        editNit?.text?.clear()
        editCustomerNit?.text?.clear()
        cbCustomer.isChecked = false
        cbCompany.isChecked = false

    }

    private fun closeView2() {
        questionLayout3.visibility = View.GONE
        editCustomerNit?.text?.clear()
    }


    fun onCheckboxClicked(view: View) {
        when (view.id) {
            R.id.cbPersonaNatural -> {
                closeView1()
                cbPersonaJuridica.isChecked = false
                validateButton()
            }
            R.id.cbPersonaJuridica -> {
                if (!cbPersonaJuridica.isChecked) {
                    closeView1()
                    validateButton()
                } else {
                    questionLayout2.visibility = View.VISIBLE
                    cbPersonaNatural.isChecked = false

                    validateButton()
                }
            }
        }
    }

    fun onCheckboxClicked2(view: View) {
        when (view.id) {
            R.id.cbCompany -> {
                closeView2()
                cbCustomer.isChecked = false
                validateButton()
            }
            R.id.cbCustomer -> {
                if (!cbCustomer.isChecked) {
                    closeView2()
                    validateButton()
                } else {
                    questionLayout3.visibility = View.VISIBLE
                    cbCompany.isChecked = false
                    validateButton()
                }

            }
        }
    }

    private fun validateButton() {
            bntTipConfirm.isEnabled = ((cbPersonaNatural.isChecked) ||
                    (editName.text!!.isNotEmpty() && editNit.text!!.isNotEmpty() && cbCompany.isChecked) ||
                    (editName.text!!.isNotEmpty() && editNit.text!!.isNotEmpty() && editCustomerNit.text!!.isNotEmpty())
                    )
    }

    private fun renderState(status: CodeQrState?) {
        when (status) {
            is CodeQrState.SuccessGenerate -> {
                progressBar.visibility = View.GONE
                if (status.data.qrType == getString(R.string.type_green))
                    startActivity(Intent(this, AdviceActivity::class.java))
                else
                    startActivity(Intent(this, StatusCodeQrActivity::class.java))
            }
            else -> {
                progressBar.visibility = View.GONE
                alertDialog()
            }

        }
    }

    private fun alertDialog() {
        AlertDialog.Builder(this, R.style.AlertDialogTheme).apply {
            setTitle(getString(R.string.alert))
            setMessage(getString(R.string.self_diagnosis_message))
            setCancelable(false)
            setPositiveButton(getString(R.string.understood)) { _, _ ->
                startActivity(Intent(applicationContext, SemaphoreActivity::class.java))
            }
        }.show()
    }

}
