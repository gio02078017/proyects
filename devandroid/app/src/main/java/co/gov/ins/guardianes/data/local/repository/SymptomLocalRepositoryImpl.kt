package co.gov.ins.guardianes.data.local.repository

import co.gov.ins.guardianes.data.local.dao.DiagnosticDao
import co.gov.ins.guardianes.data.local.dao.QuestionDao
import co.gov.ins.guardianes.data.local.dao.RuleDiagnosticDao
import co.gov.ins.guardianes.data.local.dao.RuleQuestionDao
import co.gov.ins.guardianes.data.local.mappers.fromDomain
import co.gov.ins.guardianes.data.local.mappers.fromEntity
import co.gov.ins.guardianes.domain.models.Diagnostic
import co.gov.ins.guardianes.domain.models.Question
import co.gov.ins.guardianes.domain.models.RuleDiagnostic
import co.gov.ins.guardianes.domain.models.RuleQuestion
import co.gov.ins.guardianes.domain.repository.SymptomLocalRepository
import io.reactivex.Completable
import io.reactivex.Flowable

class SymptomLocalRepositoryImpl(
    private val diagnosticDao: DiagnosticDao,
    private val questionDao: QuestionDao,
    private val ruleDiagnosticDao: RuleDiagnosticDao,
    private val ruleQuestionDao: RuleQuestionDao
) : SymptomLocalRepository {

    override fun getDiagnosticsLocal(): Flowable<List<Diagnostic>> =
        diagnosticDao.getDiagnostics().flatMap { request ->
            if (request.isNullOrEmpty()) {
                Flowable.error(Throwable("Empty"))
            } else {
                Flowable.just(request.map {
                    it.fromDomain()
                })
            }
        }

    override fun getDiagnosticLocal(diagnosticId: String): Flowable<Diagnostic> =
        diagnosticDao.getDiagnostic(diagnosticId).map {
            it.fromDomain()
        }

    override fun insertDiagnosticsLocal(diagnostics: List<Diagnostic>) =
        diagnosticDao.insertDiagnostics(
            diagnostics.map {
                it.fromEntity()
            }
        )

    override fun getQuestionsLocal(): Flowable<List<Question>> =
        questionDao.getQuestions().flatMap { request ->
            if (request.isNullOrEmpty()) {
                Flowable.error(Throwable("Empty"))
            } else {
                Flowable.just(request.map {
                    it.fromDomain()
                })
            }
        }

    override fun insertQuestionsLocal(questions: List<Question>) =
        questionDao.insertQuestions(
            questions.map {
                it.fromEntity()
            })

    override fun getRulesDiagnosticLocal(): Flowable<List<RuleDiagnostic>> =
        ruleDiagnosticDao.getRuleDiagnostics().map { list ->
            list.map {
                it.fromDomain()
            }
        }

    override fun insertRulesDiagnosticsLocal(RuleDiagnostics: List<RuleDiagnostic>): Completable =
        ruleDiagnosticDao.insertRuleDiagnostics(
            RuleDiagnostics.map {
                it.fromEntity()
            }
        )

    override fun getRuleQuestionsLocal(): Flowable<List<RuleQuestion>> =
        ruleQuestionDao.getRuleQuestions().map { list ->
            list.map {
                it.fromDomain()
            }
        }

    override fun insertRuleQuestionsLocal(ruleQuestions: List<RuleQuestion>): Completable =
        ruleQuestionDao.insertRuleQuestions(
            ruleQuestions.map {
                it.fromEntity()
            }
        )
}