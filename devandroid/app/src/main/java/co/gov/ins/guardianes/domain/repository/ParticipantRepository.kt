package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.Participant
import io.reactivex.Single

interface ParticipantRepository {

    fun getParticipants(
        authorization: String,
        id: String
    ): Single<List<Participant>>
}