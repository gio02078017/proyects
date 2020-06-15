package co.gov.ins.guardianes.data.remoto.models

data class BaseRequest<T>(
    val data: T,
    val success: Boolean?,
    val error: Boolean,
    val message: String?
)