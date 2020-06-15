package co.gov.ins.guardianes.data.local.mappers

import co.gov.ins.guardianes.data.local.entities.LastSelfDiagnosisEntity
import co.gov.ins.guardianes.domain.models.LastSelfDiagnosis as Domain


fun Domain.fromEntity() = run {
    LastSelfDiagnosisEntity(
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
}

fun LastSelfDiagnosisEntity.fromDomain() = run {
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
}