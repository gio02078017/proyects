package co.gov.ins.guardianes.domain.uc


import co.gov.ins.guardianes.data.remoto.models.QuestionRequest
import co.gov.ins.guardianes.domain.models.AnswerRequest
import co.gov.ins.guardianes.domain.repository.AnswerRepository
import co.gov.ins.guardianes.domain.repository.TokenRepository
import co.gov.ins.guardianes.domain.repository.UserPreferences
import co.gov.ins.guardianes.util.ext.retryWithUpdatedTokenIfRequired
import io.reactivex.Completable

class AnswerUc(
    private val answerRepository: AnswerRepository,
    private val userPreferences: UserPreferences,
    private val tokenRepository: TokenRepository
) {

    fun registerAnswer(
        idHousehold: String,
        date: String,
        question: ArrayList<QuestionRequest>,
        diagnosis: String,
        latitude: String,
        longitude: String
    ) = run {
        val answer = AnswerRequest(
            userPreferences.getUserId(),
            idHousehold,
            date,
            question,
            diagnosis,
            latitude,
            longitude,
            userPreferences.getDeviceId()
        )
        Completable.defer {
            answerRepository.registerAnswer(answer, userPreferences.getAuthorization())
        }.retryWithUpdatedTokenIfRequired(
            tokenRepository, userPreferences
        )
    }
}