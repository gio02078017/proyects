package co.gov.ins.guardianes.domain.models

data class GenerateQrResponse(
        val codeQr: String,
        val expirationDate: String?,
        val showAlert: Boolean,
        val alertMessage: String,
        val qrType: String,
        val qrMessage: List<String>,
        val validQr: Boolean,
        val validMessage: String,
        val existsCompany: Int
)