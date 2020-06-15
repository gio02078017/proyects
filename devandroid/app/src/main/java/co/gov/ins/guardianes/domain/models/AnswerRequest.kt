package co.gov.ins.guardianes.domain.models

import co.gov.ins.guardianes.data.remoto.models.QuestionRequest


data class AnswerRequest (
        val idUser : String,
        val idHousehold : String,
        val date : String,
        val question : ArrayList<QuestionRequest>,
        val diagnosis : String,
        val latitude: String,
        val longitude: String,
        val deviceId: String
)
