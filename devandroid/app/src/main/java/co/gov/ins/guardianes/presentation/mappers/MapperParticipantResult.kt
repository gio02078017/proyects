package co.gov.ins.guardianes.presentation.mappers

import co.gov.ins.guardianes.presentation.models.ParticipantResult
import co.gov.ins.guardianes.domain.models.LastParticipantDiagnosis as Domain

fun Domain.fromPresentation() =
        ParticipantResult(
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