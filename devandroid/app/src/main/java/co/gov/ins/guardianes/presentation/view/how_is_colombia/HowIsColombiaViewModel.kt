package co.gov.ins.guardianes.presentation.view.how_is_colombia

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.domain.uc.FirebaseEventUc
import co.gov.ins.guardianes.domain.uc.HowIsColombiaUc
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers

class HowIsColombiaViewModel(
    private val howIsColombiaUc: HowIsColombiaUc,
    private val firebaseEventUc: FirebaseEventUc
) : BaseViewModel() {

    private val howIsColombiaLiveData = MutableLiveData<HowIsColombiaState>()
    val getHowIsColombiaState: LiveData<HowIsColombiaState>
        get() = howIsColombiaLiveData

    fun getData() {
        howIsColombiaUc.getDataRemote()
            .doOnSubscribe {
                howIsColombiaLiveData.postValue(HowIsColombiaState.Loading)
            }
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribeBy(
                onSuccess = {
                    howIsColombiaLiveData.value = HowIsColombiaState.Success(it)
                },
                onError = {
                    howIsColombiaLiveData.value = HowIsColombiaState.Error(it.message)
                    getLocal()
                }
            ).addTo(disposeBag)
    }

    private fun getLocal() {
        howIsColombiaUc.getDataLocal()
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribeBy(
                onNext =
                {
                    howIsColombiaLiveData.value = HowIsColombiaState.Success(it)
                },
                onError =
                {
                    howIsColombiaLiveData.value = HowIsColombiaState.Error(it.message)
                }
            ).addTo(disposeBag)
    }

    fun createEvent(key: String) {
        firebaseEventUc.createEvent(key)
    }

    override fun onCleared() {
        super.onCleared()
        disposeBag.clear()
    }
}