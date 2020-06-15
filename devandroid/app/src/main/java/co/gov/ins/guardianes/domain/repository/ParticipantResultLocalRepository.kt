package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.LastParticipantDiagnosis
import io.reactivex.Completable
import io.reactivex.Flowable

interface ParticipantResultLocalRepository {

    fun getLastParticipantDiagnosis(): Flowable<List<LastParticipantDiagnosis>>

    fun setLastParticipantDiagnosis(lastParticipantDiagnosis: List<LastParticipantDiagnosis>): Completable
}