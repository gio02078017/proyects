package co.gov.ins.guardianes.data.local.models

import co.gov.ins.guardianes.R

data class ExceptionDecreto(
        val id: Int,
        val body: Int,
        val value: String,
        var isSelect: Boolean = false
)

val listDecreto = listOf(
        ExceptionDecreto(1, R.string.uno_531, "DE1"),
        ExceptionDecreto(2, R.string.dos_531, "DE2"),
        ExceptionDecreto(3, R.string.tres_531, "DE3"),
        ExceptionDecreto(4, R.string.cuatro_531, "DE4"),
        ExceptionDecreto(5, R.string.cinco_531, "DE5"),
        ExceptionDecreto(6, R.string.seis_531, "DE6"),
        ExceptionDecreto(7, R.string.siete_531, "DE7"),
        ExceptionDecreto(8, R.string.ocho_531, "DE8"),
        ExceptionDecreto(9, R.string.nueve_531, "DE9"),
        ExceptionDecreto(10, R.string.diez_531, "DE10"),
        ExceptionDecreto(11, R.string.once_531, "DE11"),
        ExceptionDecreto(12, R.string.doce_531, "DE12"),
        ExceptionDecreto(13, R.string.trece_531, "DE13"),
        ExceptionDecreto(14, R.string.catorce_531, "DE14"),
        ExceptionDecreto(15, R.string.quince_531, "DE15"),
        ExceptionDecreto(16, R.string.dieciseis_531, "DE16"),
        ExceptionDecreto(17, R.string.diecisiete_531, "DE17"),
        ExceptionDecreto(18, R.string.dieciocho_531, "DE18"),
        ExceptionDecreto(19, R.string.diecinueve_531, "DE19"),
        ExceptionDecreto(20, R.string.veinte_531, "DE20"),
        ExceptionDecreto(21, R.string.veintiuno_531, "DE21"),
        ExceptionDecreto(22, R.string.veintidos_531_new, "DE22"),
        ExceptionDecreto(23, R.string.veintitres_531, "DE23"),
        ExceptionDecreto(24, R.string.veinticuatro_531, "DE24"),
        ExceptionDecreto(25, R.string.veinticinco_531, "DE25"),
        ExceptionDecreto(26, R.string.veintiseis_531, "DE26"),
        ExceptionDecreto(27, R.string.veintisiete_531, "DE27"),
        ExceptionDecreto(28, R.string.veintiocho_531, "DE28"),
        ExceptionDecreto(29, R.string.veintinueve_531, "DE29"),
        ExceptionDecreto(30, R.string.treinta_531, "DE30"),
        ExceptionDecreto(31, R.string.treintayuno_531, "DE31"),
        ExceptionDecreto(32, R.string.treintaydos_531, "DE32"),
        ExceptionDecreto(33, R.string.treintaytres_531, "DE33"),
        ExceptionDecreto(34, R.string.treintaycuatro_531, "DE34"),
        ExceptionDecreto(35, R.string.treintaycinco_531, "DE35"),
        ExceptionDecreto(36, R.string.treintayseis_531, "DE36"),
        ExceptionDecreto(37, R.string.treintaysiete_531, "DE37"),
        ExceptionDecreto(38, R.string.treintayocho_531, "DE38"),
        ExceptionDecreto(39, R.string.treintaynueve_531, "DE39"),
        ExceptionDecreto(40, R.string.cuarenta_531, "DE40"),
        ExceptionDecreto(41, R.string.cuarentayuno_531, "DE41"),
        ExceptionDecreto(42, R.string.cuarentaydos_531, "DE42"),
        ExceptionDecreto(43, R.string.cuarentaytres_531, "DE43"),
        ExceptionDecreto(44, R.string.cuarentaycuatro_531, "DE44")
)
