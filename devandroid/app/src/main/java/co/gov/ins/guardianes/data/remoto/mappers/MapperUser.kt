package co.gov.ins.guardianes.data.remoto.mappers

import co.gov.ins.guardianes.data.remoto.models.UserRequest
import co.gov.ins.guardianes.data.remoto.models.UserResponse
import co.gov.ins.guardianes.domain.models.UserRequest as DomainRequest
import co.gov.ins.guardianes.domain.models.UserResponse as DomainResponse

fun UserRequest.fromDomain() =
    DomainRequest(
        firstName,
        lastName, countryCode, phoneNumber, documentType, documentNumber, deviceId
    )


fun DomainRequest.fromData() =
    UserRequest(
        firstName,
        lastName, countryCode, phoneNumber, documentType, documentNumber, deviceId
    )


fun UserResponse.fromDomain() =
    DomainResponse(
        id,
        token,
        firstName,
        lastName, countryCode, phoneNumber, documentType, documentNumber
    )


fun DomainResponse.fromData() =
    UserResponse(
        id,
        token,
        firstName,
        lastName, countryCode, phoneNumber, documentType, documentNumber
    )



