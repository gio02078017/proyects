package co.gov.ins.guardianes.data.remoto.mappers

import co.gov.ins.guardianes.data.remoto.models.Answer
import co.gov.ins.guardianes.data.remoto.models.Question
import co.gov.ins.guardianes.domain.models.Answer as AnswersDomain
import co.gov.ins.guardianes.domain.models.Question as Domain

fun Question.fromDomain() = run {
    Domain(
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

fun Answer.fromDomain() = run {
    AnswersDomain(
        id,
        text,
        value,
        order
    )
}