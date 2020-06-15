package co.gov.ins.guardianes.data.remoto.mappers

import co.gov.ins.guardianes.data.remoto.models.ValidateState
import co.gov.ins.guardianes.domain.models.ValidateState as Domain

fun ValidateState.fromDomain() = run {
    Domain(
            status, message
    )
}