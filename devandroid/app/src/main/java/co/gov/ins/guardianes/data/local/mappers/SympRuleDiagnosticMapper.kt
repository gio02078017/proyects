package co.gov.ins.guardianes.data.local.mappers

import co.gov.ins.guardianes.data.local.entities.RuleDiagnosticEntity
import co.gov.ins.guardianes.domain.models.RuleDiagnostic

fun RuleDiagnosticEntity.fromDomain() = run {
    RuleDiagnostic (
            id,
            idElement,
            expression
    )
}

fun RuleDiagnostic.fromEntity() = run {
    RuleDiagnosticEntity (
            id,
            idElement,
            expression
    )
}
