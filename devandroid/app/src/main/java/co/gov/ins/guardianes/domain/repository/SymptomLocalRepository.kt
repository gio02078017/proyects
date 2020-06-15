package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.Diagnostic
import co.gov.ins.guardianes.domain.models.Question
import co.gov.ins.guardianes.domain.models.RuleDiagnostic
import co.gov.ins.guardianes.domain.models.RuleQuestion
import io.reactivex.Completable
import io.reactivex.Flowable
import io.reactivex.Single

interface SymptomLocalRepository {

    fun insertDiagnosticsLocal(diagnostics: List<Diagnostic>): Completable

    fun getDiagnosticsLocal(): Flowable<List<Diagnostic>>

    fun getDiagnosticLocal(diagnosticId: String): Flowable<Diagnostic>

    fun insertQuestionsLocal(questions: List<Question>): Completable

    fun getQuestionsLocal(): Flowable<List<Question>>

    fun insertRulesDiagnosticsLocal(RuleDiagnostics: List<RuleDiagnostic>): Completable

    fun getRulesDiagnosticLocal(): Flowable<List<RuleDiagnostic>>

    fun insertRuleQuestionsLocal(ruleQuestions: List<RuleQuestion>): Completable

    fun getRuleQuestionsLocal(): Flowable<List<RuleQuestion>>

}