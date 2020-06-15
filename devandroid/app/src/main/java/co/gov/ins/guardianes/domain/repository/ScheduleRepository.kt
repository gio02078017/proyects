package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.Schedule
import io.reactivex.Single

interface ScheduleRepository {

    fun getLines(): Single<List<Schedule>>
}