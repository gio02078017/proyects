package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.AnswerRequest
import io.reactivex.Completable

interface AnswerRepository {

    fun registerAnswer(
        answerRequest: AnswerRequest,
        authorization: String
    ): Completable

}
