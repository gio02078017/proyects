package co.gov.ins.guardianes.data.local.models

import co.gov.ins.guardianes.R


data class ExceptionResolution(
        val id: Int,
        val body: Int,
        val value: String,
        var isSelect: Boolean = false
)

val listResolution = listOf(
        ExceptionResolution(1, R.string.first_464, "RES1"),
        ExceptionResolution(2, R.string.second_464, "RES2"),
        ExceptionResolution(3, R.string.three_464, "RES3"),
        ExceptionResolution(4, R.string.four_464, "RES4"),
        ExceptionResolution(5, R.string.five_464, "RES5"),
        ExceptionResolution(6, R.string.six_464, "RES6"),
        ExceptionResolution(7, R.string.seven_464, "RES7"),
        ExceptionResolution(8, R.string.eight_464, "RES8")
)