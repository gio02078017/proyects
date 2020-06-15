package co.gov.ins.guardianes.data.local.mappers

import co.gov.ins.guardianes.data.local.entities.QuestionEntity
import co.gov.ins.guardianes.domain.models.Answer
import co.gov.ins.guardianes.domain.models.Question
import  co.gov.ins.guardianes.data.local.entities.Answer as Entity

fun Question.fromEntity() = run {
    QuestionEntity(
        id = id,
        title = title,
        description = description,
        field = field,
        multiple = multiple,
        order = order,
        answer = answers.map {
            it.fromAnswerEntity()
        }
    )
}


fun QuestionEntity.fromDomain() = run {
    Question(
        id = id,
        title = title,
        description = description,
        field = field,
        multiple = multiple,
        order = order,
        answers = answer.map {
            it.fromAnswerDomain()
        }
    )
}

fun Answer.fromAnswerEntity() = run {
    Entity(
        id,
        text,
        value,
        order
    )
}

fun Entity.fromAnswerDomain() = run {
    Answer(
        id,
        text,
        value,
        order
    )
}
