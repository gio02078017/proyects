package co.gov.ins.guardianes.presentation.models

data class SendSmsResponse(
    val verificationId: String,
    val responseCode: String
)