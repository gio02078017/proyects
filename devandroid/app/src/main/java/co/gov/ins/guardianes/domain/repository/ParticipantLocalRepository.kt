package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.Participant
import io.reactivex.Completable
import io.reactivex.Flowable

interface ParticipantLocalRepository {

    fun getParticipants(): Flowable<List<Participant>>

    fun getParticipantById(id: String): Flowable<Participant>

    fun setParticipants(list: List<Participant>): Completable

    fun setParticipant(participant: Participant): Completable
}