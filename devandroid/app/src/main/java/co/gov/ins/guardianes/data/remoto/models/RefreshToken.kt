package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName

data class RefreshToken(
        @SerializedName("refresh_token")
        val refreshToken: String = "",
        @SerializedName("phone_number")
        val phoneNumber: String = "",
        @SerializedName("document_number")
        val documentNumber: String = "",
        @SerializedName("document_type")
        val documentType: String = ""
)