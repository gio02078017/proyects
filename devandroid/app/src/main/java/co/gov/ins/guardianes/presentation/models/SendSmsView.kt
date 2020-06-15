package co.gov.ins.guardianes.presentation.models

import co.gov.ins.guardianes.util.Constants.Key.EMPTY_STRING

data class SendSmsView(
        val documentNumber: String = EMPTY_STRING,
        val documentType: String = EMPTY_STRING,
        val phoneAreaCode: String = EMPTY_STRING,
        val phone: String = EMPTY_STRING,
        var verificationId: String = EMPTY_STRING,
        var verificationCode: String = EMPTY_STRING
)