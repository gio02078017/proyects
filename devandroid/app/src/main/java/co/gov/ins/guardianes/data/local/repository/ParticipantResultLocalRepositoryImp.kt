package co.gov.ins.guardianes.data.local.repository

import co.gov.ins.guardianes.data.local.dao.ParticipantResultDao
import co.gov.ins.guardianes.data.local.mappers.fromDomain
import co.gov.ins.guardianes.data.local.mappers.fromEntity
import co.gov.ins.guardianes.domain.models.LastParticipantDiagnosis
import co.gov.ins.guardianes.domain.repository.ParticipantResultLocalRepository
import io.reactivex.Completable
import io.reactivex.Flowable


class ParticipantResultLocalRepositoryImp(
    private val participantResultDao: ParticipantResultDao
) : ParticipantResultLocalRepository {

    override fun getLastParticipantDiagnosis(): Flowable<List<LastParticipantDiagnosis>> =
        participantResultDao.getLastParticipantDiagnosis().map { list ->
            if (list.isNotEmpty()) {
                list.map {
                    it.fromDomain()
                }
            } else {
                throw Throwable("Empty")
            }
        }

    override fun setLastParticipantDiagnosis(lastParticipantDiagnosis: List<LastParticipantDiagnosis>): Completable =
        participantResultDao.insertLastParticipantDiagnosis(lastParticipantDiagnosis.map {
            it.fromEntity()
        })


}