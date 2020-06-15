package co.gov.ins.guardianes.data.local.mappers

import co.gov.ins.guardianes.data.local.entities.LastParticipantDiagnosisEntity
import co.gov.ins.guardianes.domain.models.LastParticipantDiagnosis as Domain


fun Domain.fromEntity() = run {
    LastParticipantDiagnosisEntity(
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

fun LastParticipantDiagnosisEntity.fromDomain() = run {
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