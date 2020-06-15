package co.gov.ins.guardianes.data.remoto.mappers

import co.gov.ins.guardianes.data.remoto.models.ParticipantRequest
import co.gov.ins.guardianes.domain.models.ParticipantRequest as DomainRequest

fun ParticipantRequest.fromDomain() =
    DomainRequest(
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


fun DomainRequest.fromData() =
    ParticipantRequest(
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

