package co.gov.ins.guardianes.data.remoto.repository

import co.gov.ins.guardianes.data.remoto.api.ApiParticipantResult
import co.gov.ins.guardianes.data.remoto.models.fromDomain
import co.gov.ins.guardianes.domain.models.LastParticipantDiagnosis
import co.gov.ins.guardianes.domain.repository.ParticipantResultRepository
import io.reactivex.Single

class ParticipantResultImp(
    val apiParticipantResult: ApiParticipantResult
) : ParticipantResultRepository {

    override fun queryParticipantResultDiagnosis(
        authorization: String,
        id: String
    ): Single<List<LastParticipantDiagnosis>> =
        apiParticipantResult.queryLastParticipantDiagnosis(authorization, id).map { response ->
            response.map {
                it.lastParticipantDiagnosis.fromDomain()
            }
        }
}
