package co.gov.ins.guardianes.presentation.mappers

import co.gov.ins.guardianes.presentation.models.Answer
import co.gov.ins.guardianes.presentation.models.Question
import co.gov.ins.guardianes.domain.models.Answer as SymptomDomain
import co.gov.ins.guardianes.domain.models.Question as Domain

fun Domain.fromDomain() = run {
    Question(
        id,
        title,
        description,
        field,
        multiple,
        order,
        answers.map {
            it.fromDomain()
        }
    )
}

fun SymptomDomain.fromDomain() = run {
    Answer(
        id,
        text,
        value,
        order
    )
}