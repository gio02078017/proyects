package co.gov.ins.guardianes.data.remoto.repository

import co.gov.ins.guardianes.data.remoto.api.ApiSchedule
import co.gov.ins.guardianes.data.remoto.mappers.fromDomain
import co.gov.ins.guardianes.domain.repository.ScheduleRepository
import io.reactivex.Single

class ScheduleRepositoryIml(private val apiSchedule: ApiSchedule) :
    ScheduleRepository {

    override fun getLines() =
            apiSchedule.getSchedules().flatMap { request ->
                if (!request.error) {
                    Single.just(request.data.map {
                        it.fromDomain()
                    })
                } else {
                    Single.error(Throwable())
                }
            }
}

