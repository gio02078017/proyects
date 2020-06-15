package co.gov.ins.guardianes.presentation.view.codeQr

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.domain.uc.CodeQrUc
import co.gov.ins.guardianes.domain.uc.UserPreferencesUc
import co.gov.ins.guardianes.presentation.mappers.fromDomain
import co.gov.ins.guardianes.presentation.mappers.fromPresentation
import co.gov.ins.guardianes.presentation.models.GenerateQrRequest
import co.gov.ins.guardianes.presentation.models.GenerateQrResponse
import co.gov.ins.guardianes.presentation.models.LegalPerson
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers

class CodeQrViewModel(
        private val codeQrUc: CodeQrUc,
        private val userPreferencesUc: UserPreferencesUc
) : BaseViewModel() {

    private val _codeQrMutableLiveData = MutableLiveData<CodeQrState>()
    val codeQrLiveData: LiveData<CodeQrState>
        get() = _codeQrMutableLiveData

    private fun getLocalQr() {
        codeQrUc.getCodeQrLocal()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy (
                        onNext = {
                            _codeQrMutableLiveData.value = CodeQrState.Success(it.fromPresentation())
                        },
                        onError = {
                            _codeQrMutableLiveData.value = CodeQrState.Error(it.message)
                        }
                ).addTo(disposeBag)
    }

    fun validateQr(userId: String) {
        codeQrUc.validateCodeQr(userId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy (
                        onSuccess = {
                            _codeQrMutableLiveData.value = CodeQrState.SuccessValidate(it.fromPresentation())
                        },
                        onError = {
                            _codeQrMutableLiveData.value = CodeQrState.Error(it.message)
                            getLocalQr()
                        }
                ).addTo(disposeBag)
    }

    fun generateQr(toList: List<String>, person: LegalPerson) {
        val generateQrRequest = GenerateQrRequest(getUser()!!.id, person, toList)
        codeQrUc.generateCodeQr(generateQrRequest.fromDomain())
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy (
                        onSuccess = {
                            _codeQrMutableLiveData.value = CodeQrState.SuccessGenerate(it.fromPresentation())
                        },
                        onError = {
                            _codeQrMutableLiveData.value = CodeQrState.Error(it.message)
                            getLocalQr()
                        }
                ).addTo(disposeBag)
    }

    fun getUser() = userPreferencesUc.getUser()

    override fun onCleared() {
        super.onCleared()
        disposeBag
    }
}

