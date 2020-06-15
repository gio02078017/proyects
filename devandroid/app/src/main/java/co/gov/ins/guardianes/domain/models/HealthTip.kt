package co.gov.ins.guardianes.domain.models

data class HealthTip(
    val id: String,
    val title: String,
    val documentUrl: String,
    val order: Int,
    val createDate: String,
    val updateDate: String
)