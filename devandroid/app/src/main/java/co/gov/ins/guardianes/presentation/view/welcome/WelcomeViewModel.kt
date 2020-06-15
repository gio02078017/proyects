package co.gov.ins.guardianes.presentation.view.welcome

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.domain.uc.SplashUc
import co.gov.ins.guardianes.domain.uc.TokenUc
import co.gov.ins.guardianes.domain.uc.UserPreferencesUc
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers

class WelcomeViewModel(
        private val splashUc: SplashUc,
        private val userPreferencesUc: UserPreferencesUc,
        private val tokenUc: TokenUc
) : BaseViewModel() {

    private val welcomeLiveData = MutableLiveData<WelcomeState>()
    val getWelcomeLiveDataState: LiveData<WelcomeState>
        get() = welcomeLiveData

    fun getUser() = userPreferencesUc.getUser()

    fun isTokenNew() = splashUc.isNewToken()

    fun isTokenRegister() = splashUc.isTokenRegister()

    fun getToken() {
        tokenUc.createToken()
                .doOnSubscribe {
                    welcomeLiveData.postValue(WelcomeState.Loading)
                }.subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy(
                        onComplete = {
                            welcomeLiveData.value = WelcomeState.SuccessToken
                        },
                        onError = {
                            welcomeLiveData.value = WelcomeState.Error(it.message)
                        }
                ).addTo(disposeBag)
    }

    override fun onCleared() {
        super.onCleared()
        disposeBag
    }
}