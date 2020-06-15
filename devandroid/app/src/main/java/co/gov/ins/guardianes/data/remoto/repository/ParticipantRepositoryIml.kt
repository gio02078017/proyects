package co.gov.ins.guardianes.data.remoto.repository

import co.gov.ins.guardianes.data.remoto.api.ApiParticipants
import co.gov.ins.guardianes.data.remoto.mappers.fromDomain
import co.gov.ins.guardianes.domain.models.Participant
import co.gov.ins.guardianes.domain.repository.ParticipantRepository
import io.reactivex.Single

class ParticipantRepositoryIml(
    private val apiParticipants: ApiParticipants
) : ParticipantRepository {

    override fun getParticipants(authorization: String, id: String): Single<List<Participant>> =
        apiParticipants.getParticipants(authorization, id).flatMap { request ->
            if (!request.error) {
                Single.just(request.data.map {
                    it.fromDomain()
                })
            } else {
                Single.error(Throwable())
            }
        }


}

