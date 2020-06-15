package co.gov.ins.guardianes.presentation.models


data class Participant(
        var id: String,
        val firstName: String,
        val lastName: String,
        val relationship: String
) {
    val name
        get() = "$firstName $lastName"
}