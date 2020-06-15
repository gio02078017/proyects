package co.gov.ins.guardianes.domain.models

data class VerifyUserResponse(
    val responseCode: String,
    val user: UserResponse,
    val token: String,
    val refreshToken: String
)