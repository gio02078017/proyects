package co.gov.ins.guardianes.presentation.models


data class ExceptionResolution(
        val id: Int,
        val body: Int,
        val value: String,
        var isSelect: Boolean
)