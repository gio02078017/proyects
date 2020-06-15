package co.gov.ins.guardianes.data.remoto.repository


import co.gov.ins.guardianes.data.remoto.api.ApiAnswer
import co.gov.ins.guardianes.data.remoto.mappers.fromData
import co.gov.ins.guardianes.domain.models.AnswerRequest
import co.gov.ins.guardianes.domain.repository.AnswerRepository
import io.reactivex.Completable

class AnswerRepositoryImpl(private val apiAnswer: ApiAnswer) : AnswerRepository {

    override fun registerAnswer(answerRequest: AnswerRequest, authorization: String): Completable =
        apiAnswer.registerAnswer(authorization, answerRequest.fromData()).flatMapCompletable {
            if (!it.error) {
                Completable.complete()
            } else {
                Completable.error(Throwable(it.message))
            }
        }

}

