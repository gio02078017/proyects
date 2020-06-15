package co.gov.ins.guardianes.data.remoto.models

data class BaseRequestUser<T>(
    val user: T,
    val status: Boolean?,
    val error: Boolean
)