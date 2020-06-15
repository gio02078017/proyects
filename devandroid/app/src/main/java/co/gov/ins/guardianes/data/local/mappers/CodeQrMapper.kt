package co.gov.ins.guardianes.data.local.mappers

import co.gov.ins.guardianes.data.local.entities.CodeQrEntity
import co.gov.ins.guardianes.domain.models.GenerateQrResponse as Domain

fun CodeQrEntity.fromDomain() = run {
    Domain(
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

fun Domain.fromEntity() = run {
    CodeQrEntity(
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