package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName

data class HealthTip(
    val id: String,
    val title: String,
    @SerializedName("document_url")
    val documentUrl: String,
    val order: Int,
    @SerializedName("created_at")
    val createDate: String,
    @SerializedName("updated_at")
    val updateDate: String
)