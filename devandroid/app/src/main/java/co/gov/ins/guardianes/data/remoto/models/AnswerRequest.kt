package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName

data class AnswerRequest (
        @SerializedName("id_usuario")
        val idUser : String,
        @SerializedName("id_household")
        val idHousehold : String,
        @SerializedName("fecha")
        val date : String,
        @SerializedName("preguntas")
        val question : ArrayList<QuestionRequest>,
        @SerializedName("diagnostico")
        val diagnosis : String,
        val latitude: String,
        val longitude: String,
        @SerializedName("device_id")
        val deviceId: String
)
