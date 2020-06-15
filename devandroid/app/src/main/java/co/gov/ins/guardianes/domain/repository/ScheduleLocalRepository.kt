package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.Schedule
import io.reactivex.Completable
import io.reactivex.Flowable

interface ScheduleLocalRepository {

    fun getSchedule(): Flowable<List<Schedule>>

    fun setSchedule(schedule: List<Schedule>): Completable
}