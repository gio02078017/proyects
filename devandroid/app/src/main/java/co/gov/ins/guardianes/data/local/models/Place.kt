package co.gov.ins.guardianes.data.local.models

data class Place(
    var name: String? = null,
    val city: String?,
    val state: String?,
    val latitude: Double = 0.0,
    val longitude: Double = 0.0,
    val address: String? = null
)