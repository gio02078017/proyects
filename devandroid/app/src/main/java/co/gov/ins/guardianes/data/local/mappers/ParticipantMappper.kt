package co.gov.ins.guardianes.data.local.mappers

import co.gov.ins.guardianes.data.local.entities.ParticipantEntity
import co.gov.ins.guardianes.domain.models.Participant as Domain

fun ParticipantEntity.fromDomain() = run {
    Domain(
        id,
        appToken,
        idUser,
        relationship,
        firstName,
        lastName,
        documentType,
        documentNumber,
        phoneNumber,
        countryCode
    )
}

fun Domain.fromEntity() = run {
    ParticipantEntity(
        id,
        appToken,
        idUser,
        relationship,
        firstName,
        lastName,
        documentType,
        documentNumber,
        phoneNumber,
        countryCode
    )
}