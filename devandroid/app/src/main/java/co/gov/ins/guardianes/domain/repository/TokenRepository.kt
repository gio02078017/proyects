package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.data.remoto.models.RefreshToken
import co.gov.ins.guardianes.domain.models.TokenResponse
import io.reactivex.Single

interface TokenRepository {

    fun createToken(userId: String, token: String): Single<TokenResponse>
    fun refreshToken(refreshToken: String): Single<String>
    fun newRefreshToken(dataRefreshToken: RefreshToken): Single<TokenResponse>
}