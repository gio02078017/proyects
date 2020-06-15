package co.gov.ins.guardianes.domain.models


data class UserRequest(
    val firstName: String,
    val lastName: String,
    val countryCode: String,
    var phoneNumber: String,
    var documentType: String,
    var documentNumber: String,
    val deviceId: String
)