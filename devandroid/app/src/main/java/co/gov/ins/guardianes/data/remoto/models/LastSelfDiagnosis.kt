package co.gov.ins.guardianes.data.remoto.models

import co.gov.ins.guardianes.domain.models.LastSelfDiagnosis as Domain

data class LastSelfDiagnosis(
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

fun LastSelfDiagnosis.fromDomain() =
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