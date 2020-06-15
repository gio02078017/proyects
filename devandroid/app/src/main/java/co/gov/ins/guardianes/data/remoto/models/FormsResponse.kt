package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName

data class FormsResponse(
    val error: Boolean?,
    val message: String?,
    @SerializedName("preguntas")
    val question: List<Question>,
    @SerializedName("diagnosticos")
    val diagnostics: List<Diagnostic>,
    @SerializedName("reglas_preguntas")
    val rulesQuestion: List<RuleQuestion>,
    @SerializedName("reglas_diagnosticos")
    val rulesDiagnostics: List<RuleDiagnostic>
)
