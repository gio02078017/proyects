package co.gov.ins.guardianes.data.remoto.mappers

import co.gov.ins.guardianes.data.remoto.models.RuleDiagnostic
import co.gov.ins.guardianes.domain.models.RuleDiagnostic as Domain

fun RuleDiagnostic.fromDomain() = run {
    Domain(
        id,
        idElement,
        expression
    )
}