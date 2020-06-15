package co.gov.ins.guardianes.presentation.mappers

import co.gov.ins.guardianes.presentation.models.UserResponse
import co.gov.ins.guardianes.domain.models.UserResponse as DomainResponse


fun UserResponse.fromDomain() =
    DomainResponse(
        id = id,
        token = token,
        firstName = firstName,
        lastName = lastName,
        countryCode = countryCode,
        phoneNumber = phoneNumber,
        documentType = documentType,
        documentNumber = documentNumber
    )

fun DomainResponse.fromPresentation() =
    UserResponse(
        id, token, firstName, lastName, countryCode, phoneNumber, documentType, documentNumber
    )

