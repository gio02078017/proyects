package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.repository.TokenRepository
import co.gov.ins.guardianes.domain.repository.UserPreferences
import io.reactivex.Completable

class TokenUc(
    private val tokenRepository: TokenRepository,
    private val userPreferences: UserPreferences
) {

    fun createToken() =
        tokenRepository.createToken(userPreferences.getUserId(), userPreferences.getUserToken())
            .flatMapCompletable {
                userPreferences.setToken(it)
                Completable.complete()
            }
}