package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName

class RuleQuestion (
        val id: String,
        @SerializedName("idElemento")
        val idElement: String,
        @SerializedName("expresion")
        val expression: String
)

