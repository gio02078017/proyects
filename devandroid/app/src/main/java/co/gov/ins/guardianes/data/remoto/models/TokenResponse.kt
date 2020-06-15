package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName

data class TokenResponse(
    @SerializedName("bearer_token")
    val token: String,
    @SerializedName("refresh_token")
    val refreshToken: String,
    val result: Int
)


data class TokenRequest(
    @SerializedName("user_id")
    val userId: String,
    @SerializedName("old_token")
    val token: String
)

data class RefreshTokenRequest(
        @SerializedName("refresh_token")
        val token: String
)
