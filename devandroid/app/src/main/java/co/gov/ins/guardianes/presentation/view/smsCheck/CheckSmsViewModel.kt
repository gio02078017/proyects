package co.gov.ins.guardianes.presentation.view.smsCheck

import android.view.KeyEvent
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.domain.models.SendSmsResponse
import co.gov.ins.guardianes.domain.models.VerifyUserResponse
import co.gov.ins.guardianes.domain.uc.CheckSmsUc
import co.gov.ins.guardianes.domain.uc.UserPreferencesUc
import co.gov.ins.guardianes.presentation.mappers.fromDomain
import co.gov.ins.guardianes.presentation.mappers.fromPresentation
import co.gov.ins.guardianes.presentation.models.SendSmsView
import co.gov.ins.guardianes.presentation.models.UserResponse
import co.gov.ins.guardianes.util.ext.textWatcherObserver
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers
import kotlinx.android.synthetic.main.activity_check_sms.*

class CheckSmsViewModel(
    private val checkSmsUc: CheckSmsUc,
    private val userPreferencesUc: UserPreferencesUc
) : BaseViewModel() {

    private val _checkLiveData = MutableLiveData<CheckSmsState>()
    val checkLiveData: LiveData<CheckSmsState>
        get() = _checkLiveData

    fun getUser() = userPreferencesUc.getUser()?.fromPresentation()

    fun requestSms() {
        userPreferencesUc.getUser()?.let { user ->
            val sms = SendSmsView(
                documentNumber = user.documentNumber,
                documentType = user.documentType,
                phoneAreaCode = user.countryCode,
                phone = user.phoneNumber
            )
            checkSmsUc.requestSms(sms.fromDomain())
                .doOnSubscribe {
                    _checkLiveData.postValue(CheckSmsState.Loading)
                }
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy(
                    onSuccess = {
                        _checkLiveData.value = CheckSmsState.Success(it.fromPresentation())
                    },
                    onError = {
                        _checkLiveData.value = CheckSmsState.Error(it.message)
                    }
                ).addTo(disposeBag)

        } ?: run {
            _checkLiveData.value = CheckSmsState.UserFail
        }
    }

    fun verifySms(code: String, verificationId: String) {
        userPreferencesUc.getUser()?.let { user ->
            val sms = SendSmsView(
                documentNumber = user.documentNumber,
                documentType = user.documentType,
                phoneAreaCode = user.countryCode,
                phone = user.phoneNumber,
                verificationCode = code,
                verificationId = verificationId
            )
            checkSmsUc.verifySms(sms.fromDomain())
                .doOnSubscribe {
                    _checkLiveData.postValue(CheckSmsState.Loading)
                }
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy(
                    onSuccess = {
                        _checkLiveData.value = CheckSmsState.SuccessVerify(it)
                    },
                    onError = {
                        _checkLiveData.value = CheckSmsState.Error(it.message)
                    }
                ).addTo(disposeBag)
        } ?: run {
            _checkLiveData.value = CheckSmsState.UserFail
        }
    }

    fun setUser(userResponse: UserResponse) {
        userPreferencesUc.setUser(userResponse.fromDomain())
    }

    fun initTextChange(checkSmsActivity: CheckSmsActivity) {

        val codeOneObservable = checkSmsActivity.codeOne.textWatcherObserver().subscribe {
            if (!it.isNullOrEmpty())
                checkSmsActivity.codeTwo.requestFocus()
        }

        checkSmsActivity.codeTwo.setOnKeyListener { _, keyCode, event ->
            if (event.action == KeyEvent.ACTION_DOWN && keyCode == KeyEvent.KEYCODE_DEL) {
                if (checkSmsActivity.codeTwo.length() > 0) {
                    checkSmsActivity.codeTwo.setText("")
                } else {
                    checkSmsActivity.codeOne.setText("")
                    checkSmsActivity.codeOne.requestFocus()
                }
                return@setOnKeyListener true
            }
            false
        }

        val codeTwoObservable = checkSmsActivity.codeTwo.textWatcherObserver().subscribe {
            if (!it.isNullOrEmpty())
                checkSmsActivity.codeThree.requestFocus()
        }

        checkSmsActivity.codeThree.setOnKeyListener { _, keyCode, event ->
            if (event.action == KeyEvent.ACTION_DOWN && keyCode == KeyEvent.KEYCODE_DEL) {
                if (checkSmsActivity.codeThree.length() > 0) {
                    checkSmsActivity.codeThree.setText("")
                } else {
                    checkSmsActivity.codeTwo.setText("")
                    checkSmsActivity.codeTwo.requestFocus()
                }
                return@setOnKeyListener true
            }
            false
        }

        val codeThreeObservable = checkSmsActivity.codeThree.textWatcherObserver().subscribe {
            if (!it.isNullOrEmpty())
                checkSmsActivity.codeFour.requestFocus()
        }

        checkSmsActivity.codeFour.setOnKeyListener { _, keyCode, event ->
            if (event.action == KeyEvent.ACTION_DOWN && keyCode == KeyEvent.KEYCODE_DEL) {
                if (checkSmsActivity.codeFour.length() > 0) {
                    checkSmsActivity.codeFour.setText("")
                } else {
                    checkSmsActivity.codeThree.setText("")
                    checkSmsActivity.codeThree.requestFocus()
                }
                return@setOnKeyListener true
            }
            false
        }

        disposeBag.add(codeOneObservable)
        disposeBag.add(codeTwoObservable)
        disposeBag.add(codeThreeObservable)
    }

    private fun SendSmsResponse.fromPresentation() =
        co.gov.ins.guardianes.presentation.models.SendSmsResponse(
            verificationId, responseCode
        )

    override fun onCleared() {
        super.onCleared()
        disposeBag.clear()
    }
}