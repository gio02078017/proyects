package co.gov.ins.guardianes.data.remoto.models

import co.gov.ins.guardianes.util.Constants
import com.google.gson.annotations.SerializedName

data class SendSmsResponse(
        @SerializedName("verification_id")
        val verificationId: String = Constants.Key.EMPTY_STRING,
        @SerializedName("response_code")
        val responseCode: String = Constants.Key.EMPTY_STRING
)