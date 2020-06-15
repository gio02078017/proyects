package co.gov.ins.guardianes.domain.models

data class SendSmsResponse(
    val verificationId: String,
    val responseCode: String
)