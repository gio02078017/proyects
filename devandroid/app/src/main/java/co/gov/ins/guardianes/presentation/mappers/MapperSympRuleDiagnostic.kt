package co.gov.ins.guardianes.presentation.mappers

import co.gov.ins.guardianes.presentation.models.RuleDiagnostic
import co.gov.ins.guardianes.domain.models.RuleDiagnostic as Domain

fun RuleDiagnostic.fromDomain() = run {
    Domain (
            id,
            idElement,
            expression
    )
}