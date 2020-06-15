package co.gov.ins.guardianes.data.local.repository

import co.gov.ins.guardianes.data.local.dao.ScheduleDao
import co.gov.ins.guardianes.data.local.mappers.fromDomain
import co.gov.ins.guardianes.data.local.mappers.fromEntity
import co.gov.ins.guardianes.domain.models.Schedule
import co.gov.ins.guardianes.domain.repository.ScheduleLocalRepository
import io.reactivex.Completable
import io.reactivex.Flowable

class ScheduleLocalRepositoryImpl(
    private val scheduleDao: ScheduleDao
) : ScheduleLocalRepository {

    override fun getSchedule(): Flowable<List<Schedule>> =
        scheduleDao.getSchedule().map { response ->
            if (response.isEmpty()) {
                throw Throwable("Empty")
            } else {
                response.map {
                    it.fromDomain()
                }
            }
        }

    override fun setSchedule(schedule: List<Schedule>): Completable =
        scheduleDao.insertSchedules(schedule.map {
            it.fromEntity()
        })
}