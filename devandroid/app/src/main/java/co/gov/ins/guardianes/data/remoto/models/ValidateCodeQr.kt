package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName

data class ValidateCodeQr (
        @SerializedName("valid_qr")
        val validQr: Boolean,
        val message: String,
        @SerializedName("QR")
        val codeQr: GenerateQrResponse
)