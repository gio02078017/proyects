package co.gov.ins.guardianes.data.local.mappers

import co.gov.ins.guardianes.data.local.entities.HotLineEntity
import co.gov.ins.guardianes.data.local.entities.ScheduleEntity
import co.gov.ins.guardianes.data.remoto.models.HotLine
import co.gov.ins.guardianes.domain.models.Schedule


fun ScheduleEntity.fromDomain() =
    Schedule(
        entityName,
        hotLines.map {
            it.fromDomain()
        }
    )


fun Schedule.fromEntity() =
    ScheduleEntity(
        entityName = entityName,
        hotLines = hotLines.map {
            it.fromEntity()
        }
    )


fun HotLineEntity.fromDomain() =
    HotLine(
        id, name, phone
    )


fun HotLine.fromEntity() =
    HotLineEntity(
        id = id,
        name = name,
        phone = phone
    )

