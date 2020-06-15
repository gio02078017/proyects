package co.gov.ins.guardianes.presentation.mappers

import co.gov.ins.guardianes.presentation.models.GenerateQrRequest
import co.gov.ins.guardianes.presentation.models.GenerateQrResponse
import co.gov.ins.guardianes.presentation.models.LegalPerson
import co.gov.ins.guardianes.presentation.models.ValidateCodeQr
import co.gov.ins.guardianes.domain.models.LegalPerson as DomainLegalPerson
import co.gov.ins.guardianes.domain.models.GenerateQrRequest as DomainRequest
import co.gov.ins.guardianes.domain.models.GenerateQrResponse as DomainResponse
import co.gov.ins.guardianes.domain.models.ValidateCodeQr as DomainValidate

fun GenerateQrRequest.fromDomain() =
        DomainRequest(
                userId = userId,
                legalPerson = legalPerson.fromDomain(),
                options = options
        )

fun DomainRequest.fromPresentation() =
        GenerateQrRequest(
                userId,
                legalPerson.fromPresentation(),
                options
        )

fun LegalPerson.fromDomain() = run {
        DomainLegalPerson(
                typePersona, nameCompany, nit, workplace, nitClient
        )
}

fun DomainLegalPerson.fromPresentation() = run {
        LegalPerson(
                typePersona, nameCompany, nit, workplace, nitClient
        )
}

fun GenerateQrResponse.fromDomain() =
        DomainResponse(
                codeQr = codeQr,
                expirationDate = expirationDate,
                showAlert = showAlert,
                alertMessage = alertMessage,
                qrType = qrType,
                qrMessage = qrMessage,
                validQr = validQr,
                validMessage = validMessage,
                existsCompany = existsCompany
        )

fun DomainResponse.fromPresentation() =
        GenerateQrResponse(
                codeQr,
                expirationDate,
                showAlert,
                alertMessage,
                qrType,
                qrMessage,
                validQr,
                validMessage,
                existsCompany
        )

fun ValidateCodeQr.fromDomain() =
        DomainValidate(
                validQr = validQr,
                message = message,
                codeQr = codeQr.fromDomain()
        )

fun DomainValidate.fromPresentation() =
        ValidateCodeQr(
                validQr,
                message,
                codeQr.fromPresentation()
        )