package co.gov.ins.guardianes.data.remoto.api

import co.gov.ins.guardianes.data.remoto.models.GenerateQrRequest
import co.gov.ins.guardianes.data.remoto.models.GenerateQrResponse
import co.gov.ins.guardianes.data.remoto.models.ValidateCodeQr
import io.reactivex.Single
import retrofit2.http.*

interface ApiCodeQr {

    @GET("v6.0/QR/last/user/{user_id}")
    fun validateCodeQr(
        @Header("Authorization") authorization: String,
        @Path("user_id") id: String
    ): Single<ValidateCodeQr>

    @POST("v6.0/QR/generate")
    fun generateCodeQr(
        @Header("Authorization") authorization: String,
        @Body generateQrRequest: GenerateQrRequest
    ): Single<GenerateQrResponse>
}