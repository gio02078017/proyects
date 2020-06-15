package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName

class RuleDiagnostic (
        val id: String,
        @SerializedName("idElemento")
        val idElement: String,
        @SerializedName("expresion")
        val expression: String
)