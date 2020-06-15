package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName


data class Question(
    val id: String,
    @SerializedName("titulo")
    val title: String,
    @SerializedName("descripcion")
    val description: String,
    @SerializedName("campo")
    val field: String,
    val multiple: Boolean,
    val order: Int,
    @SerializedName("respuestas")
    val answers: List<Answer>
)

data class Answer(
    val id: String,
    @SerializedName("texto")
    val text: String,
    @SerializedName("valor")
    val value: String,
    val order: Int
)

