package co.gov.ins.guardianes.domain.models

data class Place(
    var name: String? = null,
    val city: String?,
    val state: String?,
    val latitude: Double,
    val longitude: Double,
    val address: String? = null
)