package co.gov.ins.guardianes.presentation.mappers

import co.gov.ins.guardianes.presentation.models.HealthTip
import co.gov.ins.guardianes.domain.models.HealthTip as Domain

fun Domain.fromPresentation() = run {
    HealthTip(
        id, title, documentUrl, order
    )
}