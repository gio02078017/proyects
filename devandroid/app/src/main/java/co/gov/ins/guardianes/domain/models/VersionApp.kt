package co.gov.ins.guardianes.domain.models

data class VersionApp(
    val id: String,
    val platform: String,
    val minimumVersion: String
)