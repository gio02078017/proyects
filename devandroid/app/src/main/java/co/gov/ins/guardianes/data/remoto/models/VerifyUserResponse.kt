package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName

data class VerifyUserResponse(
    @SerializedName("verification_id")
    val verificationId: String,
    @SerializedName("response_code")
    val responseCode: String,
    val user: UserResponse,
    @SerializedName("bearer_token")
    val token: String,
    @SerializedName("refresh_token")
    val refreshToken: String,
    val success: Boolean,
    val message: String?
)