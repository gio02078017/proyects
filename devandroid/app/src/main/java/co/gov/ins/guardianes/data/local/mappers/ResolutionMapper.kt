package co.gov.ins.guardianes.data.local.mappers

import co.gov.ins.guardianes.data.local.entities.ResolutionEntity
import co.gov.ins.guardianes.domain.models.ExceptionResolution


fun ResolutionEntity.fromDomain() =
        ExceptionResolution(id, body, value, isSelect
    )


fun ExceptionResolution.fromEntity() =
        ResolutionEntity(id, body, value, isSelect
    )




