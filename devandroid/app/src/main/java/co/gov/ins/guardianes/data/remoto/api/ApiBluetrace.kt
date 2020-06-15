package co.gov.ins.guardianes.data.remoto.api

import co.gov.ins.guardianes.data.remoto.models.ValidateState
import io.reactivex.Single
import retrofit2.http.GET
import retrofit2.http.Header

interface ApiBluetrace {

    @GET("v1.0/Tracking")
    fun validateState(
            @Header("Authorization") authorization: String
    ): Single<ValidateState>
}