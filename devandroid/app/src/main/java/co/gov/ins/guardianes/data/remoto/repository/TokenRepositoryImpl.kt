package co.gov.ins.guardianes.data.remoto.repository

import co.gov.ins.guardianes.data.remoto.api.ApiToken
import co.gov.ins.guardianes.data.remoto.models.RefreshToken
import co.gov.ins.guardianes.data.remoto.models.RefreshTokenRequest
import co.gov.ins.guardianes.data.remoto.models.TokenRequest
import co.gov.ins.guardianes.data.remoto.models.TokenResponse
import co.gov.ins.guardianes.domain.repository.TokenRepository
import io.reactivex.Single
import co.gov.ins.guardianes.domain.models.TokenResponse as TokenDomain

class TokenRepositoryImpl(
    private val apiToken: ApiToken
) : TokenRepository {

    override fun createToken(userId: String, token: String): Single<TokenDomain> =
        apiToken.createToken(TokenRequest(userId, token)).map {
            it.fromDomain()
        }

    override fun refreshToken(refreshToken: String): Single<String> =
        apiToken.refreshToken(RefreshTokenRequest(refreshToken)).map { it.token }


    override fun newRefreshToken(refreshToken: RefreshToken): Single<TokenDomain> =
            apiToken.newRefreshToken(refreshToken).map { it.fromDomain() }

    fun TokenResponse.fromDomain() = run {
        co.gov.ins.guardianes.domain.models.TokenResponse(
            token = token,
            refreshToken = refreshToken
        )
    }
}