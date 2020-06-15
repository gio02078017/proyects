package co.gov.ins.guardianes.data.local.mappers

import co.gov.ins.guardianes.data.local.entities.HealthTipEntity
import co.gov.ins.guardianes.domain.models.HealthTip as Domain

fun HealthTipEntity.fromDomain() = run {
    Domain(
        id, title, documentUrl, order, createDate, updateDate
    )
}

fun Domain.fromEntities() = run {
    HealthTipEntity(
        id, title, documentUrl, order, createDate, updateDate
    )
}