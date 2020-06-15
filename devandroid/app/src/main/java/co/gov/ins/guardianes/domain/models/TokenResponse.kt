package co.gov.ins.guardianes.domain.models

data class TokenResponse(
    val token: String,
    val refreshToken: String
)