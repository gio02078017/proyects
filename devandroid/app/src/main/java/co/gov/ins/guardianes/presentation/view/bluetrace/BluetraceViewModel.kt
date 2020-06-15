package co.gov.ins.guardianes.presentation.view.bluetrace

import android.util.Log
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.domain.uc.BluetraceUc
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers
import java.util.*

class BluetraceViewModel(
        private val bluetraceUc: BluetraceUc
) : BaseViewModel() {

    private val bluetraceLiveData = MutableLiveData<BluetraceState>()
    val getBluetraceLiveDataState: LiveData<BluetraceState>
        get() = bluetraceLiveData

    fun getState() {
        bluetraceUc.getState()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy(
                        onSuccess = {
                            if (it.status.toLowerCase(Locale.ROOT) == "success") {
                                when(it.message.toLowerCase(Locale.ROOT)) {
                                    "true" -> bluetraceLiveData.value = BluetraceState.SuccessState(true)
                                    else -> bluetraceLiveData.value = BluetraceState.SuccessState(false)
                                }
                            } else {
                                bluetraceLiveData.value = BluetraceState.SuccessState(false)
                            }
                        },
                        onError = {
                            bluetraceLiveData.value = BluetraceState.SuccessState(false)
                        }
                ).addTo(disposeBag)
    }

    override fun onCleared() {
        super.onCleared()
        disposeBag
    }

}