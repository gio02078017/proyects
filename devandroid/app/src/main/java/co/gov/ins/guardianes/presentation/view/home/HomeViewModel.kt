package co.gov.ins.guardianes.presentation.view.home

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.data.remoto.models.RefreshToken
import co.gov.ins.guardianes.domain.models.TokenResponse
import co.gov.ins.guardianes.domain.models.VersionApp
import co.gov.ins.guardianes.domain.repository.TokenRepository
import co.gov.ins.guardianes.domain.repository.UserPreferences
import co.gov.ins.guardianes.domain.uc.FirebaseEventUc
import co.gov.ins.guardianes.domain.uc.MenuHomeUc
import co.gov.ins.guardianes.presentation.models.MenuHome
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers
import retrofit2.HttpException
import java.net.HttpURLConnection
import co.gov.ins.guardianes.domain.models.MenuHome as Domain

class HomeViewModel(
        private val menuHomeUc: MenuHomeUc,
        private val firebaseEventUc: FirebaseEventUc,
        private val tokenRepository: TokenRepository,
        private val userPreferences: UserPreferences
) : BaseViewModel() {

    private val itemLiveData = MutableLiveData<HomeState>()
    val getItemLiveData: LiveData<HomeState>
        get() = itemLiveData

    fun getListMenu() {
        itemLiveData.value = HomeState.Success(
                menuHomeUc.getListMenu().map {
                    it.fromPresentation()
                }
        )
    }

    fun createEvent(key: String) {
        firebaseEventUc.createEvent(key)
    }

    fun preferenceClear() {
        menuHomeUc.preferenceClear()
    }

    fun getUser() = menuHomeUc.getUser()

    fun isToken() = menuHomeUc.isToken()

    private fun Domain.fromPresentation() = MenuHome(title, icon)

    fun getPermission() = menuHomeUc.getPermission()

    fun setCoachMark(boolean: Boolean) = menuHomeUc.setCoachMark(boolean)

    fun getCoachMark() = menuHomeUc.getCoachMark()

    fun getVersion() {
        menuHomeUc.getAppVersion()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy(
                        onSuccess = {
                            itemLiveData.value = HomeState.SuccessVersion(it.fromPresentation())
                        },
                        onError = { }
                ).addTo(disposeBag)
    }

    private fun VersionApp.fromPresentation() =
            co.gov.ins.guardianes.presentation.models.VersionApp(
                    id, platform, minimumVersion
            )

    fun refreshToken(function: (result: Boolean) -> Unit) {
        val tokenRefresh = userPreferences.getToken()?.refreshToken
        if (!tokenRefresh.isNullOrEmpty()) {
            tokenRepository.refreshToken(tokenRefresh)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeBy(
                            onSuccess = {
                                userPreferences.setToken(TokenResponse(it, tokenRefresh))
                                function(true)
                            },
                            onError = { throwable ->
                                if (throwable is HttpException && throwable.code() == HttpURLConnection.HTTP_PRECON_FAILED) {
                                    changeToken(function)
                                }
                                function(false)
                            }
                    ).addTo(disposeBag)
        }
        function(false)

    }

    fun changeToken(function: (result: Boolean) -> Unit) {
        val tokenRefresh = userPreferences.getToken()?.refreshToken
        if (!tokenRefresh.isNullOrEmpty()) {
            val userData = userPreferences.getUser()
            var dataRefreshToken = RefreshToken()
            userData?.apply {
                dataRefreshToken = RefreshToken(
                        tokenRefresh!!,
                        phoneNumber,
                        documentNumber,
                        documentType
                )
            }

            tokenRepository.newRefreshToken(dataRefreshToken)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeBy(
                            onSuccess = {
                                userPreferences.setToken(it)
                                function(true)
                            },
                            onError = {
                                function(false)
                            }
                    ).addTo(disposeBag)
        }
        function(false)
    }

}