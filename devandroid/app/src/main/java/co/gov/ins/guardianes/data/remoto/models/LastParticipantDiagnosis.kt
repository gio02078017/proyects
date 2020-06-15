package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName
import co.gov.ins.guardianes.domain.models.LastParticipantDiagnosis as Domain

data class LastParticipantDiagnosis(
    val id: String = "",
    val idUser: String = "",
    val idHousehold: String = "",
    val date: String = "",
    val diagnosis: String = "",
    val latitude: String = "",
    val longitude: String = "",
    val deviceId: String = "",
    val createdAt: String = "",
    val updateAt: String = ""
)

fun LastParticipantDiagnosis.fromDomain() =
    Domain(
        id,
        idUser,
        idHousehold,
        date,
        diagnosis,
        latitude,
        longitude,
        deviceId,
        createdAt,
        updateAt
    )

data class LastParticipantResponse(
    @SerializedName("id_household")
    val idHousehold: String,
    @SerializedName("result")
    val lastParticipantDiagnosis: LastParticipantDiagnosis

)


