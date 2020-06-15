package co.gov.and.coronapp.bluetrace.streetpass.server

import java.text.SimpleDateFormat
import java.util.*
import co.gov.and.coronapp.bluetrace.streetpass.persistence.StreetPassRecord as Persistence

fun Persistence.fromPersistence() = run {
    StreetPassRecord (
            tempid = msg,
            org = org,
            mp = modelP,
            mc = modelC,
            rs = rssi.toString(),
            o = if (txPower !== null) txPower.toString() else "",
            v = v.toString(),
            tm = timestamp.longToDateString()
    )
}

fun Long.longToDateString(): String {
    val date = Date(this)
    val dateFormat = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale("es", "MX"))
    return dateFormat.format(date)
}