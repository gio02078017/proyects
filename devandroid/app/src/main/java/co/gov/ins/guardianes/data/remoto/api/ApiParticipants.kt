package co.gov.ins.guardianes.data.remoto.api

import co.gov.ins.guardianes.data.remoto.models.BaseRequest
import co.gov.ins.guardianes.data.remoto.models.Participant
import io.reactivex.Single
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.Path

interface ApiParticipants {

    @GET("v2.0/household/user/{id}")
    fun getParticipants(
        @Header("Authorization") authorization: String,
        @Path("id") id: String
    ): Single<BaseRequest<List<Participant>>>
}


