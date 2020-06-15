package co.gov.ins.guardianes.domain.models

data class FormsResponse(
    val question: List<Question>,
    val diagnostics: List<Diagnostic>,
    val rulesQuestion: List<RuleQuestion>,
    val rulesDiagnostics: List<RuleDiagnostic>
)
