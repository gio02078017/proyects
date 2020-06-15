package co.gov.ins.guardianes.util.ext

import co.gov.ins.guardianes.util.Constants

fun Int.getTypeDocument() = when (this) {
    0 -> Constants.DocumentType.CC
    1 -> Constants.DocumentType.TI
    2 -> Constants.DocumentType.CE
    3 -> Constants.DocumentType.PEP
    4 -> Constants.DocumentType.PAS
    5 -> Constants.DocumentType.VISA
    else -> Constants.DocumentType.RC
}