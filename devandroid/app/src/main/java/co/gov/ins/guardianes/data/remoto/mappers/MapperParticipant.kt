package co.gov.ins.guardianes.data.remoto.mappers

import co.gov.ins.guardianes.data.remoto.models.Participant
import co.gov.ins.guardianes.domain.models.Participant as Domain

fun Participant.fromDomain() =
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
