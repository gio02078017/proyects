package co.gov.ins.guardianes.presentation.models

data class ValidateCodeQr (
        val validQr: Boolean,
        val message: String,
        val codeQr: GenerateQrResponse
)