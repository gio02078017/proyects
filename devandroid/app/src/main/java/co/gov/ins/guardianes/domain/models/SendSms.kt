package co.gov.ins.guardianes.domain.models

import com.google.gson.annotations.SerializedName

data class SendSms(
    @SerializedName("document_number")
    val documentNumber: String,
    @SerializedName("document_type")
    val documentType: String,
    @SerializedName("phone_area_code")
    val phoneAreaCode: String,
    @SerializedName("phone")
    val phone: String,
    @SerializedName("verification_id")
    var verificationId: String,
    @SerializedName("verification_code")
    var verificationCode: String
)