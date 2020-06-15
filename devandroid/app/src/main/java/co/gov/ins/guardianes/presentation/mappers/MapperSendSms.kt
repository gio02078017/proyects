package co.gov.ins.guardianes.presentation.mappers

import co.gov.ins.guardianes.domain.models.SendSms
import co.gov.ins.guardianes.presentation.models.SendSmsView


fun SendSmsView.fromDomain() = SendSms(
        documentType = documentType,
        documentNumber = documentNumber,
        phoneAreaCode = phoneAreaCode,
        phone = phone,
        verificationCode = verificationCode,
        verificationId = verificationId
)
