package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName

data class QuestionRequest (
        @SerializedName("id")
        val id : String,
        @SerializedName("respuestas")
        var answer : MutableList<String>

)

