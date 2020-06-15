package co.gov.ins.guardianes.data.remoto.api

import co.gov.ins.guardianes.data.remoto.models.BaseRequestMember
import co.gov.ins.guardianes.data.remoto.models.ParticipantRequest
import io.reactivex.Single
import retrofit2.http.Body
import retrofit2.http.Header
import retrofit2.http.POST

interface ApiAddParticipants {

    @POST("v2.0/household/create")
    fun registerParticipant(
        @Header("Authorization") authorization: String,
        @Body participantRequest: ParticipantRequest
    ): Single<BaseRequestMember>
}

