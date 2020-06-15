package co.gov.ins.guardianes.data.remoto.repository

import co.gov.ins.guardianes.data.remoto.api.ApiAddParticipants
import co.gov.ins.guardianes.data.remoto.mappers.fromData
import co.gov.ins.guardianes.data.remoto.mappers.fromDomain
import co.gov.ins.guardianes.domain.models.Participant
import co.gov.ins.guardianes.domain.models.ParticipantRequest
import co.gov.ins.guardianes.domain.repository.AddParticipantRepository
import io.reactivex.Single

class AddParticipantRepositoryIml(private val apiAddParticipants: ApiAddParticipants) :
    AddParticipantRepository {
    override fun registerParticipant(
        authorization: String,
        participantRequest: ParticipantRequest
    ): Single<Participant> =
        apiAddParticipants.registerParticipant(authorization, participantRequest.fromData())
            .flatMap { request ->
                if (!request.error) {
                    Single.just(request.member.fromDomain())
                } else {
                    Single.error(Throwable(request.message))
                }
            }
}