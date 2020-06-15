package co.gov.ins.guardianes.data.remoto.mappers

import co.gov.ins.guardianes.data.remoto.models.AnswerRequest
import co.gov.ins.guardianes.domain.models.AnswerRequest as DomainRequest


fun AnswerRequest.fromDomain() =
    DomainRequest(
            idUser,
            idHousehold,
            date,
            question,
            diagnosis,
            latitude,
            longitude,
            deviceId
    )


fun DomainRequest.fromData() =
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







