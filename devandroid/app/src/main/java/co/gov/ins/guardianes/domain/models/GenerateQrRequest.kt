package co.gov.ins.guardianes.domain.models

data class GenerateQrRequest(
        val userId: String,
        val legalPerson: LegalPerson,
        val options: List<String>
)

data class LegalPerson(
        val typePersona: String,
        val nameCompany: String,
        val nit: String,
        val workplace: String,
        val nitClient: String
)