package co.gov.ins.guardianes.domain.models

data class Permission(
        val id: String,
        val title: String,
        val subTitle: String,
        val icon: String,
        val accept: Boolean
)