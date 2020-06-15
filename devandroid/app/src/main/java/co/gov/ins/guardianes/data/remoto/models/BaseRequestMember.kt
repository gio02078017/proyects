package co.gov.ins.guardianes.data.remoto.models

data class BaseRequestMember(
    val member: Participant,
    val status: Boolean?,
    val error: Boolean,
    val message: String
)