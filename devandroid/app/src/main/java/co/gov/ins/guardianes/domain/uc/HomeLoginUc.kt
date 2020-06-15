package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.models.LastSelfDiagnosis
import co.gov.ins.guardianes.domain.repository.HomeLoginLocalRepository
import co.gov.ins.guardianes.domain.repository.HomeLoginRepository
import co.gov.ins.guardianes.domain.repository.TokenRepository
import co.gov.ins.guardianes.domain.repository.UserPreferences
import co.gov.ins.guardianes.util.ext.retryWithUpdatedTokenIfRequired
import io.reactivex.Flowable
import io.reactivex.Single
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch

class HomeLoginUc(
        private val homeLoginRepository: HomeLoginRepository,
        private val homeLoginLocalRepository: HomeLoginLocalRepository,
        private val userPreferences: UserPreferences,
        private val tokenRepository: TokenRepository
) {
    private var isRequest: Boolean = false
    fun queryLastSelfDiagnosis(): Flowable<LastSelfDiagnosis> = run {
        isRequest = false
        homeLoginLocalRepository.getLastSelfDiagnosis()
                .retryWhen { error ->
                    error.map { throwable ->
                        if (throwable.message == "Empty") {
                            callService()
                        } else throw throwable
                    }
                }.flatMap {
                    callService()
                    Flowable.just(it)
                }
    }

    private fun callService() {
        if (!isRequest)
            GlobalScope.launch(context = Dispatchers.IO) {
                updateDb().doOnSubscribe {
                    isRequest = true
                }.subscribeBy(onError = {})
            }
    }

    private fun updateDb() =
            Single.defer {
                homeLoginRepository.queryLastSelfDiagnosis(
                        userPreferences.getAuthorization(),
                        userPreferences.getUserId()
                )
            }.retryWithUpdatedTokenIfRequired(
                    tokenRepository,
                    userPreferences
            ).map {
                homeLoginLocalRepository.setLastSelfDiagnosis(it)
                        .subscribeOn(Schedulers.io())
                        .subscribeBy()
                it
            }


}