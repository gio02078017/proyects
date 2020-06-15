package co.gov.ins.guardianes.data.local.mappers

import co.gov.ins.guardianes.data.local.entities.DecretoEntity
import co.gov.ins.guardianes.domain.models.ExceptionDecreto


fun DecretoEntity.fromDomain() =
        ExceptionDecreto(id, body, value, isSelect
    )


fun ExceptionDecreto.fromEntity() =
        DecretoEntity(id, body, value, isSelect
    )




