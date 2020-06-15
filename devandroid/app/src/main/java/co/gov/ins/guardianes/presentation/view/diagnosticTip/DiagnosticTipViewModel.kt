package co.gov.ins.guardianes.presentation.view.diagnosticTip

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.domain.uc.FirebaseEventUc
import co.gov.ins.guardianes.domain.uc.SymptomUc
import co.gov.ins.guardianes.presentation.mappers.fromDomain
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers

class DiagnosticTipViewModel(
    private val symptomUc: SymptomUc,
    private val firebaseEventUc: FirebaseEventUc
) : BaseViewModel() {

    private val diagnosticLiveData = MutableLiveData<DiagnosticState>()
    val getDiagnosticLiveData: LiveData<DiagnosticState>
        get() = diagnosticLiveData

    fun getDiagnostic(id: String) {
        symptomUc.getDiagnosticsById(id).subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribeBy(
                onNext = {
                    diagnosticLiveData.value = DiagnosticState.Success(it.fromDomain())
                },
                onError = {
                    diagnosticLiveData.value = DiagnosticState.Error
                }
            ).addTo(disposeBag)
    }

    fun createEvent(key: String) = firebaseEventUc.createEvent(key)

    override fun onCleared() {
        super.onCleared()
        disposeBag.clear()
    }
}