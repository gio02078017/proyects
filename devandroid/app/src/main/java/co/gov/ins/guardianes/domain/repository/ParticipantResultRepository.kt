package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.LastParticipantDiagnosis
import io.reactivex.Single

interface ParticipantResultRepository {

    fun queryParticipantResultDiagnosis(
        authorization: String,
        id: String
    ): Single<List<LastParticipantDiagnosis>>
}