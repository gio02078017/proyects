package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName

data class GenerateQrResponse(
        @SerializedName("qr")
        val codeQr: String,
        @SerializedName("expiration_date")
        val expirationDate: String?,
        @SerializedName("show_alert")
        val showAlert: Boolean,
        @SerializedName("alert_message")
        val alertMessage: String,
        @SerializedName("qr_type")
        val qrType: String,
        @SerializedName("qr_message")
        val qrMessage: List<String>,
        val validQr: Boolean,
        val validMessage: String,
        @SerializedName("exists_company")
        val existsCompany: Int
)