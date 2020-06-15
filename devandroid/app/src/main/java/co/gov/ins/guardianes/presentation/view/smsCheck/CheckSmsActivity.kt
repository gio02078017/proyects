package co.gov.ins.guardianes.presentation.view.smsCheck

import android.annotation.SuppressLint
import android.content.Intent
import android.os.Bundle
import android.view.MotionEvent
import android.view.View.*
import androidx.appcompat.app.AlertDialog
import androidx.lifecycle.Observer
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.models.UserResponse
import co.gov.ins.guardianes.presentation.view.home.HomeActivity
import co.gov.ins.guardianes.presentation.view.smsCheck.CheckSmsState.*
import co.gov.ins.guardianes.util.Constants.Key.EMPTY_STRING
import co.gov.ins.guardianes.util.ext.getColorCompat
import co.gov.ins.guardianes.util.ext.getDrawableCompat
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import co.gov.ins.guardianes.view.utils.fromHtml
import com.jakewharton.rxbinding2.widget.RxTextView
import io.reactivex.Observable
import io.reactivex.functions.Function4
import kotlinx.android.synthetic.main.activity_check_sms.*
import kotlinx.android.synthetic.main.base_toolbar.*
import org.koin.androidx.viewmodel.ext.android.viewModel

class CheckSmsActivity : BaseAppCompatActivity() {

    private var verificationId: String? = null
    private val checkSmsViewModel: CheckSmsViewModel by viewModel()
    private var user: UserResponse? = null
    private val listener = OnTouchListener { _, event ->
        if (event.action == MotionEvent.ACTION_DOWN && invalidCode.visibility == VISIBLE) {
            backgroundCodesView(R.drawable.round_ligth_blue, R.color.text_blue)
            invalidCode.visibility = INVISIBLE
            clearInputs()
        }
        false
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_check_sms)
        user = checkSmsViewModel.getUser()
        instantiateVariable()
        observables()
        observablesTouch()
        observablesLiveData()
        initToolbar()
        onClick()
        checkSmsViewModel.initTextChange(this)
        sendSms()
    }

    private fun initToolbar() {
        onToolbar(toolbar)
        toolbar.setNavigationOnClickListener {
            onBackPressed()
        }
    }

    private fun instantiateVariable() {
        notReceiveSms.fromHtml(getString(R.string.not_receive_sms))
        txtTitleBar.text = getString(R.string.registry)
    }

    private fun sendSms() {
        user?.apply {
            val numberFull = "$countryCode $phoneNumber"
            textNumber.text = getString(R.string.check_cellphone_number, numberFull)
            checkSmsViewModel.requestSms()
        }
    }

    @SuppressLint("CheckResult")
    private fun observables() {
        val codeOneObservable: Observable<Boolean> = RxTextView.textChanges(codeOne)
                .map { t -> t.isNotEmpty() }

        val codeTwoObservable: Observable<Boolean> = RxTextView.textChanges(codeTwo)
                .map { t -> t.isNotEmpty() }

        val codeThreeObservable: Observable<Boolean> = RxTextView.textChanges(codeThree)
                .map { t -> t.isNotEmpty() }

        val codeFourObservable: Observable<Boolean> = RxTextView.textChanges(codeFour)
                .map { t -> t.isNotEmpty() }

        val signInEnabled: Observable<Boolean> = Observable.combineLatest(
                codeOneObservable, codeTwoObservable,
                codeThreeObservable, codeFourObservable,
                Function4 { codeOneKey, codeTwoKey,
                            codeThreeKey, codeFourKey ->
                    codeOneKey && codeTwoKey &&
                            codeThreeKey && codeFourKey
                })

        signInEnabled
                .distinctUntilChanged()
                .subscribe { key ->
                    bnCheck.isEnabled = key
                    if (key) {
                        bnCheck.setTextColor(getColorCompat(android.R.color.white))
                    } else {
                        bnCheck.setTextColor(getColorCompat(android.R.color.white))
                    }
                }
    }

    private fun observablesTouch() {
        codeOne.setOnTouchListener(listener)
        codeTwo.setOnTouchListener(listener)
        codeThree.setOnTouchListener(listener)
        codeFour.setOnTouchListener(listener)
    }

    private fun onClick() {
        bnCheck.setOnClickListener {
            val codeOneData = codeOne.text.toString()
            val codeTwoData = codeTwo.text.toString()
            val codeThreeData = codeThree.text.toString()
            val codeFourData = codeFour.text.toString()

            val code = "$codeOneData$codeTwoData$codeThreeData$codeFourData"
            verificationId?.let { it1 -> checkSmsViewModel.verifySms(code, it1) } ?: run {
                checkSmsViewModel.requestSms()
            }
        }
        notReceiveSms.setOnClickListener {
            backgroundCodesView(R.drawable.round_ligth_blue, R.color.text_blue)
            checkSmsViewModel.requestSms()
        }
    }

    private fun observablesLiveData() {
        checkSmsViewModel.checkLiveData.observe(this, Observer {
            renderState(it)
        })
    }

    private fun renderState(state: CheckSmsState) {
        progressBar.visibility = GONE
        when (state) {
            is Loading -> progressBar.visibility = VISIBLE
            is SuccessVerify -> {
                renderVerify(state)
            }
            is Success -> {
                renderSend(state)
            }
            is Error -> {
                codeInvalid()
            }
            is UserFail -> {
                finish()
            }
        }
    }

    private fun renderSend(state: Success) {
        when (state.data.responseCode) {
            SMS_SENT -> {
                verificationId = state.data.verificationId
                alertInformation(
                        getString(R.string.new_code),
                        "${getString(R.string.new_code_success)}${user?.phoneNumber}."
                )
            }
            CODE_INVALID -> {
                codeInvalid()
            }
        }
    }

    private fun renderVerify(state: SuccessVerify) {
        when (state.data) {
            VERIFICATION_SUCCESS -> {
                alertDialog(
                        getString(R.string.successful_registration),
                        getString(R.string.message_successful_registration),
                        getString(R.string.report_btn),
                        getString(R.string.then)
                )
            }
            CODE_INVALID -> {
                codeInvalid()
            }
        }
    }

    private fun backgroundCodesView(idDrawable: Int, idColor: Int) {
        codeOne.background = getDrawableCompat(idDrawable)
        codeTwo.background = getDrawableCompat(idDrawable)
        codeThree.background = getDrawableCompat(idDrawable)
        codeFour.background = getDrawableCompat(idDrawable)

        codeOne.setTextColor(getColorCompat(idColor))
        codeTwo.setTextColor(getColorCompat(idColor))
        codeThree.setTextColor(getColorCompat(idColor))
        codeFour.setTextColor(getColorCompat(idColor))
    }

    private fun codeInvalid() {
        backgroundCodesView(R.drawable.error_code, R.color.shiraz)
        invalidCode.visibility = VISIBLE
        bnCheck.isEnabled = false
    }

    private fun clearInputs() {
        codeOne.setText(EMPTY_STRING)
        codeTwo.setText(EMPTY_STRING)
        codeThree.setText(EMPTY_STRING)
        codeFour.setText(EMPTY_STRING)
    }

    private fun alertDialog(
            title: String, message: String,
            namePositive: String = getString(R.string.ready),
            nameNegative: String
    ) {
        AlertDialog.Builder(this, R.style.AlertDialogTheme).apply {
            setTitle(title)
            setMessage(message)
            setCancelable(false)
            setNegativeButton(nameNegative) { _, _ ->
                goToHomeActivity(true)
            }
            setPositiveButton(namePositive) { _, _ ->
                goToHomeActivity(false)
            }
        }.show()
    }

    private fun goToHomeActivity(isShowHome: Boolean = true) {

        val intent = Intent(this@CheckSmsActivity, HomeActivity::class.java).apply {
            addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP)
            addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK)
            addFlags(Intent.FLAG_ACTIVITY_NEW_TASK)
            putExtra("EXIT", true)
            putExtra("isShowHome", isShowHome)
        }

        startActivity(intent)
        finish()
    }

    private fun alertInformation(
            title: String, message: String,
            namePositive: String = getString(R.string.ready)
    ) {
        AlertDialog.Builder(this, R.style.AlertDialogTheme).apply {
            setTitle(title)
            setMessage(message)
            setCancelable(false)
            setPositiveButton(namePositive, null)
        }.show()
    }

    companion object {
        const val SMS_SENT = "SMS_SENT"
        const val VERIFICATION_SUCCESS = "VERIFICATION_SUCCESS"
        const val CODE_INVALID = "Ocurrió un error al verificar el mensaje"
    }
}