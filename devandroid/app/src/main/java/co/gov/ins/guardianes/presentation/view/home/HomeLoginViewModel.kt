package co.gov.ins.guardianes.presentation.view.home

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.domain.uc.FirebaseEventUc
import co.gov.ins.guardianes.domain.uc.HomeLoginUc
import co.gov.ins.guardianes.domain.uc.UserPreferencesUc
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers

class HomeLoginViewModel(
    private val firebaseEventUc: FirebaseEventUc,
    private val userPreferencesUc: UserPreferencesUc,
    private val homeLoginUc: HomeLoginUc
) : BaseViewModel() {

    private val _homeMutableLiveData = MutableLiveData<HomeLoginState>()
    val homeLiveData: LiveData<HomeLoginState>
        get() = _homeMutableLiveData

    fun createEvent(key: String) {
        firebaseEventUc.createEvent(key)
    }

    fun getUser() = userPreferencesUc.getUser()

    fun queryLastSelfDiagnosis() {
        homeLoginUc.queryLastSelfDiagnosis()
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribeBy(
                onNext = {
                    _homeMutableLiveData.value = HomeLoginState.Success(it.date)
                },
                onError = {

                }
            ).addTo(disposeBag)
    }

    override fun onCleared() {
        super.onCleared()
        disposeBag
    }
}
