package co.gov.ins.guardianes.data.local.repository

import co.gov.ins.guardianes.data.local.dao.ParticipantsDao
import co.gov.ins.guardianes.data.local.mappers.fromDomain
import co.gov.ins.guardianes.data.local.mappers.fromEntity
import co.gov.ins.guardianes.domain.models.Participant
import co.gov.ins.guardianes.domain.repository.ParticipantLocalRepository
import io.reactivex.Completable
import io.reactivex.Flowable

class ParticipantLocalRepositoryImpl(
    private val participantsDao: ParticipantsDao
) : ParticipantLocalRepository {

    override fun getParticipants(): Flowable<List<Participant>> =
            participantsDao.getParticipants().map { list ->
                if (list.isNotEmpty()) {
                    list.map {
                        it.fromDomain()
                    }
                } else {
                    throw Throwable("Empty")
                }
            }

    override fun getParticipantById(id: String): Flowable<Participant> =
        participantsDao.getParticipantsById(id).map { it.fromDomain() }

    override fun setParticipants(list: List<Participant>): Completable =
        participantsDao.insertParticipants(list.map {
            it.fromEntity()
        })

    override fun setParticipant(participant: Participant): Completable =
        participantsDao.insertParticipants(participant.fromEntity())
}