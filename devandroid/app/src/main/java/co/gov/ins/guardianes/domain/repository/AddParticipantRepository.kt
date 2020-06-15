package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.Participant
import co.gov.ins.guardianes.domain.models.ParticipantRequest
import io.reactivex.Single

interface AddParticipantRepository {

    fun registerParticipant(
        authorization: String,
        participantRequest: ParticipantRequest
    ): Single<Participant>

}