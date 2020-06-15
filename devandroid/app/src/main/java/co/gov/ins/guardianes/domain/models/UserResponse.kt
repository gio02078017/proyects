package co.gov.ins.guardianes.domain.models

data class UserResponse(
    val id: String,
    val token: String?,
    val firstName: String,
    val lastName: String,
    val countryCode: String,
    val phoneNumber: String,
    val documentType: String,
    val documentNumber: String
)
