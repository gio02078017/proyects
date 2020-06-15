package co.gov.ins.guardianes.data.local.mappers

import co.gov.ins.guardianes.data.local.entities.RuleQuestionEntity
import co.gov.ins.guardianes.domain.models.RuleQuestion

fun RuleQuestionEntity.fromDomain() = run {
    RuleQuestion (
            id,
            idElement,
            expression
    )
}

fun RuleQuestion.fromEntity() = run {
    RuleQuestionEntity (
            id,
            idElement,
            expression
    )
}