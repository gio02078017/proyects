package co.gov.ins.guardianes.data.remoto.api

import co.gov.ins.guardianes.data.remoto.models.LastParticipantResponse
import io.reactivex.Single
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.Path

interface ApiParticipantResult {

    @GET("v2.0/result/{user_id}/household")
    fun queryLastParticipantDiagnosis(
        @Header("Authorization") authorization: String,
        @Path("user_id") id: String
    ): Single<List<LastParticipantResponse>>
}
