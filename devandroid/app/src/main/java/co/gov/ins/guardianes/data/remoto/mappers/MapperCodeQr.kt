package co.gov.ins.guardianes.data.remoto.mappers

import co.gov.ins.guardianes.data.remoto.models.GenerateQrRequest
import co.gov.ins.guardianes.data.remoto.models.GenerateQrResponse
import co.gov.ins.guardianes.data.remoto.models.LegalPerson
import co.gov.ins.guardianes.data.remoto.models.ValidateCodeQr
import co.gov.ins.guardianes.domain.models.GenerateQrRequest as DomainRequest
import co.gov.ins.guardianes.domain.models.LegalPerson as DomainLegalPerson
import co.gov.ins.guardianes.domain.models.GenerateQrResponse as DomainResponse
import co.gov.ins.guardianes.domain.models.ValidateCodeQr as DomainValidate

fun GenerateQrRequest.fromDomain() = run {
    DomainRequest(
            userId,
            legalPerson.fromDomain(),
            options
    )
}

fun DomainRequest.fromData() = run {
    GenerateQrRequest(
            userId,
            legalPerson.fromData(),
            options
    )
}


fun LegalPerson.fromDomain() = run {
    DomainLegalPerson(
            typePersona, nameCompany, nit, workplace, nitClient
    )
}

fun DomainLegalPerson.fromData() = run {
    LegalPerson(
       typePersona, nameCompany, nit, workplace, nitClient
    )
}

fun GenerateQrResponse.fromDomain() = run {
    DomainResponse(
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
}

fun DomainResponse.fromData() = run {
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
}

fun ValidateCodeQr.fromDomain() = run {
    DomainValidate(
            validQr,
            message,
            codeQr.fromDomain()
    )
}

fun DomainValidate.fromData() = run {
    ValidateCodeQr(
            validQr,
            message,
            codeQr.fromData()
    )
}