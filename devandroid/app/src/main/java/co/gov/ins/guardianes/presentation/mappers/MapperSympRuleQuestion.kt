package co.gov.ins.guardianes.presentation.mappers

import co.gov.ins.guardianes.presentation.models.RuleQuestion
import co.gov.ins.guardianes.domain.models.RuleQuestion as Domain

fun RuleQuestion.fromDomain() = run {
    Domain (
            id,
            idElement,
            expression
    )
}