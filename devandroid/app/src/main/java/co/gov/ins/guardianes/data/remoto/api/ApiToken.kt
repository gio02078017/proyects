package co.gov.ins.guardianes.data.remoto.api

import co.gov.ins.guardianes.data.remoto.models.RefreshToken
import co.gov.ins.guardianes.data.remoto.models.RefreshTokenRequest
import co.gov.ins.guardianes.data.remoto.models.TokenRequest
import co.gov.ins.guardianes.data.remoto.models.TokenResponse
import io.reactivex.Single
import retrofit2.http.Body
import retrofit2.http.POST

interface ApiToken {

    @POST("v2.0/authentication/change_old_token")
    fun createToken(@Body request: TokenRequest): Single<TokenResponse>

    @POST("v2.0/authentication/refresh-token")
    fun refreshToken(@Body request: RefreshTokenRequest): Single<TokenResponse>

    @POST("v2.0/authentication/new-refresh-token")
    fun newRefreshToken(@Body refreshToken: RefreshToken): Single<TokenResponse>
}