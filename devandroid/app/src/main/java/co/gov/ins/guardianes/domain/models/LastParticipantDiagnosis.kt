package co.gov.ins.guardianes.domain.models

data class LastParticipantDiagnosis(
        val id: String,
        val idUser: String,
        val idHousehold: String,
        val date: String,
        val diagnosis: String,
        val latitude: String,
        val longitude: String,
        val deviceId: String,
        val createdAt: String,
        val updateAt: String
)