package co.gov.ins.guardianes.presentation.view.codeQr

import android.app.Activity
import android.content.Intent
import android.os.Bundle
import android.view.View
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.Observer
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.models.LegalPerson
import co.gov.ins.guardianes.presentation.view.codeQr.mobilityStatus.StatusCodeQrActivity
import co.gov.ins.guardianes.util.Constants.Key.EXCEPTION
import kotlinx.android.synthetic.main.activity_exception_form.*
import kotlinx.android.synthetic.main.base_toolbar.*
import org.koin.androidx.viewmodel.ext.android.viewModel


class ExceptionFormActivity : AppCompatActivity() {

    private val exceptionFormViewModel: ExceptionFormViewModel by viewModel()
    private val codeQrViewModel: CodeQrViewModel by viewModel()

    var listExceptiones: MutableSet<String> = mutableSetOf()
    var decretoId = ""
    var resolutionId = ""

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_exception_form)

        setupToolbar()
        setupView()
        setupListeners()

        exceptionFormViewModel.validForm(this)
        exceptionFormViewModel.setDecretos()
        exceptionFormViewModel.setResolutions()

        initObservable()
    }

    private fun initObservable() {
        exceptionFormViewModel.getItemLiveData.observe(this, Observer {
            renderData(it)
        })

        codeQrViewModel.codeQrLiveData.observe(this, Observer {
            renderState(it)
        })

    }


    private fun renderData(state: ExceptionState?) {
        val listExceptions: MutableSet<String> = mutableSetOf()
        when (state) {
            is ExceptionState.SuccessDecreto -> {
                decretoId = ""
                state.data.map {
                    if (it.isSelect) {
                        decretoId += "${it.id} "
                        listExceptions.add(it.value)
                        listExceptiones.clear()
                        exceptionFormViewModel.setResolutions()
                        edittextList.setText(decretoId)
                    }
                    listExceptiones.addAll(listExceptions)
                }


            }
            is ExceptionState.SuccessResolution -> {
                resolutionId = ""
                state.data.map {
                    if (it.isSelect) {
                        resolutionId += "${it.id} "
                        listExceptions.add(it.value)
                        listExceptiones.clear()
                        exceptionFormViewModel.setDecretos()
                        edittextList.setText(resolutionId)
                    }
                    listExceptiones.addAll(listExceptions)
                }
            }
        }

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
            }

        }
    }


    private fun showException(code: Int) {
        val intent = Intent(this@ExceptionFormActivity, ExceptionListActivity::class.java)
        intent.putExtra(EXCEPTION, code)
        startActivityForResult(intent, code)
    }

    private fun setupView() {
        userName.text = exceptionFormViewModel.getUser()?.firstName
    }

    private fun setupListeners() {
        lyDecretosSelect.setOnClickListener {
            showException(CODE_DECRETO)
        }

        lyResolutions.setOnClickListener {
            showException(CODE_REQUEST)
        }

        button.setOnClickListener {
            var notTypePerson = false
            var typePerson = false

            resources.getStringArray(R.array.exceptions_array).forEach { decreto ->
                if (decreto == listExceptiones.find { exception ->
                            exception == decreto
                        }) {
                    notTypePerson = true
                }
            }

            resources.getStringArray(R.array.not_exceptions_array).forEach { not_decreto ->
                if (not_decreto == listExceptiones.find { exception ->
                            exception == not_decreto
                        }) {
                    typePerson = true
                }
            }

            when {
                notTypePerson && typePerson -> {
                    intentTypePerson()
                }
                !notTypePerson && typePerson -> {
                    intentTypePerson()
                }
                notTypePerson && !typePerson -> {
                    generateQr()
                }

            }


        }


        toolbar.setNavigationOnClickListener {
            onBackPressed()
        }
    }

    private fun intentTypePerson() {
        val intent = Intent(this@ExceptionFormActivity, PersonTypeActivity::class.java)
        val bundle = Bundle()
        bundle.putStringArray(EXCEPTION, listExceptiones.toTypedArray())
        intent.putExtras(bundle)
        startActivity(intent)
    }

    private fun generateQr() {
        codeQrViewModel.generateQr(listExceptiones.toList(), LegalPerson(
                "",
                "",
                "",
                "",
                ""))
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
        txtTitleBar.text = getString(R.string.code_qr_status2)
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (resultCode == Activity.RESULT_OK) {
            decretoId = ""
            resolutionId = ""
            if (requestCode == CODE_DECRETO) {
                exceptionFormViewModel.getDecretosDB()
            } else {
                exceptionFormViewModel.getResolutionsDB()
            }
        }
    }

    companion object {
        private const val CODE_DECRETO = 1
        private const val CODE_REQUEST = 2
        const val VALUE_NO_COVID = "REC-19"
        const val VALUE_NOTHING = "N"
    }
}
