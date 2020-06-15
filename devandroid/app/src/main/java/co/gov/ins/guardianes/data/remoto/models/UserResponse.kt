package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName


data class UserResponse(
    val id: String,
    val token: String?,
    @SerializedName("firstname")
    val firstName: String,
    @SerializedName("lastname")
    val lastName: String,
    @SerializedName("country_code")
    val countryCode: String,
    @SerializedName("phone_number")
    val phoneNumber: String,
    @SerializedName("document_type")
    var documentType: String,
    @SerializedName("document_number")
    val documentNumber: String
)
