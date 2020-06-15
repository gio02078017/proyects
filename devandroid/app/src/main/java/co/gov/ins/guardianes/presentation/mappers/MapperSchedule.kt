package co.gov.ins.guardianes.presentation.mappers

import co.gov.ins.guardianes.presentation.models.Schedule
import co.gov.ins.guardianes.domain.models.Schedule as Domain

fun Domain.fromSchedule() =
    Schedule(
            entityName,
            hotLines
    )