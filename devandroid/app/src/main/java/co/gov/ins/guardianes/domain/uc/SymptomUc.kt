package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.models.Question
import co.gov.ins.guardianes.domain.repository.*
import co.gov.ins.guardianes.presentation.models.Answer
import co.gov.ins.guardianes.util.ext.retrySymptomsEmpty
import io.reactivex.Flowable


class SymptomUc(
    private val symptomRepository: SymptomRepository,
    private val symptomLocalRepository: SymptomLocalRepository,
    private val symptomRulesSymptomUc: RulesSymptomUc,
    private val preferencesUtilRepository: PreferencesUtilRepository,
    private val userPreferences: UserPreferences,
    private val tokenRepository: TokenRepository
) {
    fun getDataQuestionsLocal(): Flowable<List<Question>> = run {
        Flowable.defer {
            symptomLocalRepository.getQuestionsLocal()
        }.retrySymptomsEmpty(
            symptomRepository,
            tokenRepository,
            userPreferences,
            symptomLocalRepository
        )
    }

    fun getDiagnosticsById(id: String) =
        symptomLocalRepository.getDiagnosticLocal(id)

    fun getRulesSymptoms(answers: List<Answer>, face: Int) =
        symptomRulesSymptomUc.validateRisks(answers, face)
}