package co.gov.ins.guardianes.domain.models

import com.google.gson.annotations.SerializedName

data class HowIsColombia(
        val id: String = "",
        @SerializedName("confirmed_cases") val confirmedCases: Int = -1,
        @SerializedName("confirmed_cases_today") val confirmedCasesToday : Int = -1,
        @SerializedName("recovered_patients") val recoveredPatients: Int = -1,
        val deaths: Int = -1,
        @SerializedName("users_supporting") val usersSupporting: Int = -1,
        val success: Boolean = false,
        val error: Boolean = false,
        @SerializedName("created_at") val createAt: String = "",
        @SerializedName("update_at") val updatedAt: String = ""
)