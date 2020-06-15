package co.gov.ins.guardianes.presentation.mappers

import co.gov.ins.guardianes.presentation.models.AnswerRequest
import co.gov.ins.guardianes.domain.models.AnswerRequest as DomainResponse



fun AnswerRequest.fromDomain() =
    DomainResponse(
        idUser = idUser,
        idHousehold = idHousehold,
        date = date,
        question = question,
        diagnosis = diagnosis,
        latitude = latitude,
        longitude = longitude,
         deviceId = deviceId
    )

fun DomainResponse.fromPresentation() =
        AnswerRequest(
                idUser,
                idHousehold,
                date,
                question,
                diagnosis,
                latitude,
                longitude,
                deviceId
    )

