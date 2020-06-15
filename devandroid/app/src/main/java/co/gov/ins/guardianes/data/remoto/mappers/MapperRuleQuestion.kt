package co.gov.ins.guardianes.data.remoto.mappers

import co.gov.ins.guardianes.data.remoto.models.RuleQuestion
import co.gov.ins.guardianes.domain.models.RuleQuestion as Domain

fun RuleQuestion.fromDomain() = run {
    Domain(
        id,
        idElement,
        expression
    )
}