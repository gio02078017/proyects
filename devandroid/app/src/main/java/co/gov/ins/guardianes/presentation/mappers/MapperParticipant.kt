package co.gov.ins.guardianes.presentation.mappers

import co.gov.ins.guardianes.presentation.models.Participant
import co.gov.ins.guardianes.domain.models.Participant as Domain

fun Domain.fromPresentation() =
    Participant(
        id,
        firstName,
        lastName,
        relationship
    )