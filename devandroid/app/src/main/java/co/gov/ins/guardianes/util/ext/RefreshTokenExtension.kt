package co.gov.ins.guardianes.util.ext

import co.gov.ins.guardianes.data.remoto.models.RefreshToken
import co.gov.ins.guardianes.domain.models.TokenResponse
import co.gov.ins.guardianes.domain.repository.TokenRepository
import co.gov.ins.guardianes.domain.repository.UserPreferences
import io.reactivex.Completable
import io.reactivex.Flowable
import io.reactivex.Single
import io.reactivex.functions.BiFunction
import io.reactivex.rxkotlin.subscribeBy
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import retrofit2.HttpException
import java.net.HttpURLConnection

private const val MAX_RETRIES = 3

fun <T> Single<T>.retryWithUpdatedTokenIfRequired(
        tokenRepository: TokenRepository,
        userPreferences: UserPreferences
): Single<T> =
        retryWhen { handler ->
            handler.zipWith(
                    Flowable.range(1, MAX_RETRIES),
                    BiFunction<Throwable, Int, Pair<Throwable, Int>> { error, count ->
                        Pair(error, count)
                    }
            ).flatMap { pair ->
                val error = pair.first
                val count = pair.second
                val tokenRefresh = userPreferences.getToken()?.refreshToken
                if (count <= MAX_RETRIES) {
                    if (error is HttpException && error.code() == HttpURLConnection.HTTP_UNAUTHORIZED && !tokenRefresh.isNullOrEmpty()) {
                        tokenRepository.refreshToken(
                                refreshToken = tokenRefresh
                        ).doAfterSuccess {
                            userPreferences.setToken(TokenResponse(it, tokenRefresh))
                        }.doOnError { throwable ->
                            if (throwable is HttpException) {
                                if (throwable.code() == HttpURLConnection.HTTP_PRECON_FAILED) {
                                    val userData = userPreferences.getUser()
                                    var dataRefreshToken = RefreshToken()

                                    userData?.apply {
                                        dataRefreshToken = RefreshToken(
                                                tokenRefresh,
                                                phoneNumber,
                                                documentNumber,
                                                documentType
                                        )
                                    }

                                    newRefreshToken(dataRefreshToken, tokenRepository, userPreferences)
                                }
                            }
                        }
                                .toFlowable()
                    } else {
                        Flowable.error(error)
                    }
                } else {
                    Flowable.error(Throwable("Number of retries reached"))
                }
            }
        }

fun newRefreshToken(userData: RefreshToken, tokenRepository: TokenRepository, userPreferences: UserPreferences) {
    GlobalScope.launch(context = Dispatchers.IO) {

        tokenRepository
                .newRefreshToken(userData)
                .subscribeBy(
                        onError = {
                        },
                        onSuccess = {
                            userPreferences.setToken(TokenResponse(it.token, it.refreshToken))
                            Flowable.just(it)
                        }
                )
    }
}

fun Completable.retryWithUpdatedTokenIfRequired(
        tokenRepository: TokenRepository,
        userPreferences: UserPreferences
): Completable =
        this.retryWhen { handler ->
            handler.zipWith(
                    Flowable.range(1, MAX_RETRIES),
                    BiFunction<Throwable, Int, Pair<Throwable, Int>> { error, count ->
                        Pair(error, count)
                    }
            ).flatMap { pair ->
                val error = pair.first
                val count = pair.second
                val tokenRefresh = userPreferences.getToken()?.refreshToken
                if (count <= MAX_RETRIES) {
                    if (error is HttpException && error.code() == HttpURLConnection.HTTP_UNAUTHORIZED && !tokenRefresh.isNullOrEmpty()) {
                        tokenRepository.refreshToken(
                                refreshToken = tokenRefresh
                        ).doAfterSuccess {
                            userPreferences.setToken(TokenResponse(it, tokenRefresh))
                        }.doOnError { throwable ->
                            if (throwable is HttpException) {
                                if (throwable.code() == HttpURLConnection.HTTP_PRECON_FAILED) {
                                    val userData = userPreferences.getUser()
                                    var dataRefreshToken = RefreshToken()

                                    userData?.apply {
                                        dataRefreshToken = RefreshToken(
                                                tokenRefresh,
                                                phoneNumber,
                                                documentNumber,
                                                documentType
                                        )
                                    }

                                    newRefreshToken(dataRefreshToken, tokenRepository, userPreferences)
                                }
                            }
                        }.toFlowable()
                    } else {
                        Flowable.error(error)
                    }
                } else {
                    Flowable.error(IllegalStateException("Number of retries reached"))
                }
            }
        }

