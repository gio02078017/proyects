package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName

data class GenerateQrRequest(
        @SerializedName("user_id")
        val userId: String,
        @SerializedName("legal_person")
        val legalPerson: LegalPerson,
        val options: List<String>
)


data class LegalPerson(
        @SerializedName("type_persona")
        val typePersona: String,
        @SerializedName("name_company")
        val nameCompany: String,
        val nit: String,
        val workplace: String,
        @SerializedName("nit_client")
        val nitClient: String
)