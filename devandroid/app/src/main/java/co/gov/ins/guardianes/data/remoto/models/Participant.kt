package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName

data class Participant(
    var id: String,
    @SerializedName("app_token")
    var appToken: String,
    @SerializedName("user")
    var idUser: String,
    @SerializedName("relationship")
    var relationship: String,
    @SerializedName("firstname")
    var firstName: String,
    @SerializedName("lastname")
    var lastName: String,
    @SerializedName("document_type")
    var documentType: String,
    @SerializedName("document_number")
    val documentNumber: String,
    @SerializedName("phone_number")
    val phoneNumber: String,
    @SerializedName("country_code")
    val countryCode: String
)
