package co.gov.ins.guardianes.data.remoto.mappers

import co.gov.ins.guardianes.data.remoto.models.Schedule
import co.gov.ins.guardianes.domain.models.Schedule as Domain

fun Schedule.fromDomain() =
    Domain(
        entityName,
        hotLines
    )
