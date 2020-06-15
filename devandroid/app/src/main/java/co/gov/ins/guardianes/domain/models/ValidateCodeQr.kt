package co.gov.ins.guardianes.domain.models

data class ValidateCodeQr (
        val validQr: Boolean,
        val message: String,
        val codeQr: GenerateQrResponse
)