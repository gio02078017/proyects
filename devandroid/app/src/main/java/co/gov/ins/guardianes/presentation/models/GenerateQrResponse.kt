package co.gov.ins.guardianes.presentation.models

data class GenerateQrResponse(
        val codeQr: String,
        var expirationDate: String?,
        val showAlert: Boolean,
        val alertMessage: String,
        val qrType: String,
        val qrMessage: List<String>,
        val validQr: Boolean,
        val validMessage: String,
        val existsCompany: Int
)