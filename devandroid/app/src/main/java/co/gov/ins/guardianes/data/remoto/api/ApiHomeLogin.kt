package co.gov.ins.guardianes.data.remoto.api

import co.gov.ins.guardianes.data.remoto.models.LastSelfDiagnosis
import io.reactivex.Single
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.Path

interface ApiHomeLogin {

    @GET("v2.0/result/{user_id}")
    fun queryLastSelfDiagnosis(
        @Header("Authorization") authorization: String,
        @Path("user_id") id: String
    ): Single<LastSelfDiagnosis>
}