package co.gov.ins.guardianes.presentation.view.codeQr

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.domain.uc.DecretoUc
import co.gov.ins.guardianes.domain.uc.ExceptionUc
import co.gov.ins.guardianes.domain.uc.ResolutionUc
import co.gov.ins.guardianes.domain.uc.UserPreferencesUc
import co.gov.ins.guardianes.presentation.models.ExceptionDecreto
import co.gov.ins.guardianes.presentation.models.ExceptionResolution
import co.gov.ins.guardianes.util.ext.checkBoxButtonObservable
import co.gov.ins.guardianes.util.ext.textWatcherObserver
import co.gov.ins.guardianes.view.base.BaseViewModel
import kotlinx.android.synthetic.main.activity_exception_form.*
import co.gov.ins.guardianes.domain.models.ExceptionDecreto as DomainDecreto
import co.gov.ins.guardianes.domain.models.ExceptionResolution as DomainResolution
import io.reactivex.Observable
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.functions.Function3
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers

class ExceptionFormViewModel(
        private val userPreferencesUc: UserPreferencesUc,
        private val exceptionUc: ExceptionUc,
        private val decretoUc: DecretoUc,
        private val resolutionUc: ResolutionUc
) : BaseViewModel() {

    private val itemLiveData = MutableLiveData<ExceptionState>()
    val getItemLiveData: LiveData<ExceptionState>
        get() = itemLiveData

    fun getUser() = userPreferencesUc.getUser()

    fun setDecretos(){
        decretoUc.updateDecretosDb()
    }

    fun getDecretosDB() {
        decretoUc.getDecretosDB()
                .doOnSubscribe {}
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy(
                        onNext = { request ->
                            val array: ArrayList<ExceptionDecreto> = request.map {
                                it.fromPresentation()
                            } as ArrayList<ExceptionDecreto>
                            itemLiveData.value = ExceptionState.SuccessDecreto(array)
                        },
                        onError = {
                            itemLiveData.value = ExceptionState.Error(it.message)
                        }
                ).addTo(disposeBag)
    }


    fun updateDecretoSelect(
            id: Int,
            body: Int,
            value: String,
            isSelect: Boolean
    ) {
        decretoUc.updateDecretoSelect(
                id,
                body,
                value,
                isSelect
        ).addTo(disposeBag)
    }

    fun setResolutions(){
        resolutionUc.updateResolutionsDb()
    }

    fun getResolutionsDB() {
        resolutionUc.getResolutionsDB()
                .doOnSubscribe {}
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy(
                        onNext = { request ->
                            val array: ArrayList<ExceptionResolution> = request.map {
                                it.fromPresentation()
                            } as ArrayList<ExceptionResolution>
                            itemLiveData.value = ExceptionState.SuccessResolution(array)
                        },
                        onError = {
                            itemLiveData.value = ExceptionState.Error(it.message)
                        }
                ).addTo(disposeBag)
    }


    fun updateResolutionSelect(
            id: Int,
            body: Int,
            value: String,
            isSelect: Boolean
    ) {
        resolutionUc.updateResolutionSelect(
                id,
                body,
                value,
                isSelect
        ).addTo(disposeBag)
    }


    private fun DomainDecreto.fromPresentation() = ExceptionDecreto(id, body, value, isSelect)
    private fun DomainResolution.fromPresentation() = ExceptionResolution(id, body, value, isSelect)

    fun validForm(exceptionFormActivity: ExceptionFormActivity) {

        val edittextList = exceptionFormActivity.edittextList.textWatcherObserver().share()
        val checkNoHaveCovid = exceptionFormActivity.checkNoHaveCovid.checkBoxButtonObservable().share()
        val checkNothing = exceptionFormActivity.checkNothing.checkBoxButtonObservable().share()

        val observerEdittextListResult = Observable.create<Boolean> { emitter ->
            edittextList.subscribe {
                if (exceptionFormActivity.edittextList.text.isNotEmpty()){
                    exceptionFormActivity.myException.isChecked = true
                    exceptionFormActivity.checkNothing.isChecked = false
                    exceptionFormActivity.checkNoHaveCovid.isChecked = false
                }
                emitter.onNext(exceptionFormActivity.edittextList.text.isNotEmpty())
                validButton(exceptionFormActivity)
            }
        }

        val observerCheckNoHaveCovidResult = Observable.create<Boolean> { emitter ->
            checkNoHaveCovid.subscribe {
                    if (exceptionFormActivity.checkNoHaveCovid.isChecked) {
                        exceptionFormActivity.decretoId = ""
                        exceptionFormActivity.resolutionId = ""
                        exceptionFormActivity.edittextList.setText("")
                        exceptionFormActivity.listExceptiones.clear()
                        exceptionFormActivity.listExceptiones.add(ExceptionFormActivity.VALUE_NO_COVID)
                        exceptionFormActivity.checkNothing.isChecked = false
                        exceptionFormActivity.myException.isChecked = false
                        setDecretos()
                        setResolutions()
                    } else {
                        exceptionFormActivity.listExceptiones.remove(ExceptionFormActivity.VALUE_NO_COVID)
                    }
                emitter.onNext(exceptionFormActivity.checkNoHaveCovid.isChecked)
                validButton(exceptionFormActivity)
            }
        }

        val observerCheckNothingResult = Observable.create<Boolean> { emitter ->
            checkNothing.subscribe {
                    if (exceptionFormActivity.checkNothing.isChecked) {
                        exceptionFormActivity.decretoId = ""
                        exceptionFormActivity.resolutionId = ""
                        exceptionFormActivity.edittextList.setText("")
                        exceptionFormActivity.listExceptiones.clear()
                        exceptionFormActivity.listExceptiones.add(ExceptionFormActivity.VALUE_NOTHING)
                        exceptionFormActivity.checkNoHaveCovid.isChecked = false
                        exceptionFormActivity.myException.isChecked = false
                        setDecretos()
                        setResolutions()
                    } else {
                        exceptionFormActivity.listExceptiones.remove(ExceptionFormActivity.VALUE_NOTHING)
                    }
                emitter.onNext(exceptionFormActivity.checkNothing.isChecked)
                validButton(exceptionFormActivity)
            }
        }


        Observable
                .combineLatest(observerEdittextListResult,
                        observerCheckNoHaveCovidResult, observerCheckNothingResult,
                        Function3<Boolean, Boolean, Boolean, Boolean> { t1, t2, t3 ->
                            return@Function3 t1 || t2 || t3
                        })
                .subscribe {
                    exceptionFormActivity.button.isEnabled = it
                }.addTo(disposeBag)

    }

    private fun validButton(exceptionFormActivity: ExceptionFormActivity){
        exceptionFormActivity.button.isEnabled = (
                exceptionFormActivity.myException.isChecked
                || exceptionFormActivity.checkNoHaveCovid.isChecked ||
                exceptionFormActivity.checkNothing.isChecked)
    }

    override fun onCleared() {
        super.onCleared()
        disposeBag
    }
}

private fun ExceptionDecreto.fromDomain() = ExceptionDecreto(id, body, value, isSelect)
