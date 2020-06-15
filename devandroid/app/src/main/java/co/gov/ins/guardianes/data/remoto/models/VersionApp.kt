package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName

data class VersionApp (
    val id: String,
    val platform: String,
    @SerializedName("minimum_version")
    val minimumVersion: String
)