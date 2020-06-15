package co.gov.ins.guardianes.presentation.models

data class Permission(
        val id: String,
        val title: String,
        val subTitle: String,
        val icon: String,
        var accept: Boolean
)