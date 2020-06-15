package co.gov.ins.guardianes.presentation.view.healthTip

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.domain.uc.HealthTipUc
import co.gov.ins.guardianes.presentation.mappers.fromPresentation
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers

class HealthTipViewModel(
    private val healthTipUc: HealthTipUc
) : BaseViewModel() {

    private val tipLiveData = MutableLiveData<HealthTipState>()
    val getTipLiveDataState: LiveData<HealthTipState>
        get() = tipLiveData

    fun getHealthTip() {
        healthTipUc.getHealthTip()
            .doOnSubscribe {
                tipLiveData.postValue(HealthTipState.Loading)
            }
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribeBy(
                onNext = { list ->
                    if (list.isNotEmpty()) {
                        tipLiveData.postValue(HealthTipState.Success(list.map {
                            it.fromPresentation()
                        }))
                    }
                },
                onError = {
                    if (it.message != "Empty")
                        tipLiveData.value = HealthTipState.Error(null)
                }
            ).addTo(disposeBag)
    }

    override fun onCleared() {
        super.onCleared()
        disposeBag
    }
}