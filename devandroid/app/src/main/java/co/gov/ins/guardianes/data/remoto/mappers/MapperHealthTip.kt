package co.gov.ins.guardianes.data.remoto.mappers

import co.gov.ins.guardianes.data.remoto.models.HealthTip
import co.gov.ins.guardianes.domain.models.HealthTip as Domain

fun HealthTip.fromDomain() = run {
    Domain(
        id, title, documentUrl, order, createDate, updateDate
    )
}