package co.gov.ins.guardianes.presentation.models

data class VerifyUserResponse(
    val responseCode: String,
    val user: UserResponse,
    var token: String
)