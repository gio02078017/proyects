package co.gov.ins.guardianes.util.ext

import android.graphics.Bitmap
import android.graphics.Color
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.util.Constants
import co.gov.ins.guardianes.util.Constants.QrType.GREEN
import co.gov.ins.guardianes.util.Constants.QrType.RED
import co.gov.ins.guardianes.util.Constants.QrType.YELLOW
import com.google.zxing.BarcodeFormat
import com.google.zxing.qrcode.QRCodeWriter
import org.joda.time.DateTimeZone
import org.joda.time.Hours
import org.joda.time.LocalDateTime
import org.joda.time.format.DateTimeFormat
import org.joda.time.format.ISODateTimeFormat
import java.text.Normalizer
import java.text.ParseException
import java.text.SimpleDateFormat
import java.util.*

fun String.toIconNormal() = run {
    when {
        contains("alerta_triangulo", false) -> {
            R.drawable.ic_alert_triangle_blue
        }

        contains("campana", false) -> {
            R.drawable.ic_bell_blue
        }

        contains("candado", false) -> {
            R.drawable.ic_padlock_blue
        }

        contains("casa", false) -> {
            R.drawable.ic_house_blue
        }

        contains("check_cuadrado", false) -> {
            R.drawable.ic_check_squared_blue
        }

        contains("alerta_triangulo", false) -> {
            R.drawable.ic_alert_triangle_blue
        }

        contains("corazon", false) -> {
            R.drawable.ic_heart_blue
        }

        contains("escudo", false) -> {
            R.drawable.ic_shield_blue
        }

        contains("gota", false) -> {
            R.drawable.ic_drop_blue
        }

        contains("informacion", false) -> {
            R.drawable.ic_info_blue
        }

        contains("mas_cuadrado", false) -> {
            R.drawable.ic_square_plus_blue
        }

        contains("ojo", false) -> {
            R.drawable.ic_eye_blue
        }

        contains("persona_equis", false) -> {
            R.drawable.ic_person_x_blue
        }

        contains("telefono", false) -> {
            R.drawable.ic_phone_blue
        }

        contains("termometro_plus", false) -> {
            R.drawable.ic_thermometer_plus_blue
        }

        contains("termometro", false) -> {
            R.drawable.ic_thermometer_blue
        }

        contains("mensaje", false) -> {
            R.drawable.ic_message_circle_blue
        }

        else -> {
            R.drawable.ic_bag_blue
        }
    }
}

fun String.toIcon() = run {
    when {
        contains("alerta_triangulo", false) -> {
            R.drawable.ic_alert_triangle
        }

        contains("campana", false) -> {
            R.drawable.ic_bell
        }

        contains("candado", false) -> {
            R.drawable.ic_padlock
        }

        contains("casa", false) -> {
            R.drawable.ic_house
        }

        contains("check_cuadrado", false) -> {
            R.drawable.ic_check_square
        }

        contains("alerta_triangulo", false) -> {
            R.drawable.ic_alert_triangle
        }

        contains("corazon", false) -> {
            R.drawable.ic_heart
        }

        contains("escudo", false) -> {
            R.drawable.ic_shield
        }

        contains("gota", false) -> {
            R.drawable.ic_drop
        }

        contains("informacion", false) -> {
            R.drawable.ic_info
        }

        contains("mas_cuadrado", false) -> {
            R.drawable.ic_square_plus
        }

        contains("ojo", false) -> {
            R.drawable.ic_eye
        }

        contains("persona_equis", false) -> {
            R.drawable.ic_person_x
        }

        contains("telefono", false) -> {
            R.drawable.ic_phone
        }

        contains("termometro_plus", false) -> {
            R.drawable.ic_thermometer_plus
        }

        contains("termometro", false) -> {
            R.drawable.ic_thermometer
        }

        contains("mensaje", false) -> {
            R.drawable.ic_message_circle
        }
        else -> {
            R.drawable.ic_bag
        }
    }
}


fun String.toEvent() = when (this) {
    Constants.DiagnosticResult.NORMAL -> Constants.EVENT.DIAGNOSTIC_NORMAL
    Constants.DiagnosticResult.WARNING -> Constants.EVENT.DIAGNOSTIC_WARNING
    Constants.DiagnosticResult.ALERT -> Constants.EVENT.DIAGNOSTIC_ALERT
    else -> ""
}

fun String.toQrTypeNoCompany() = when (this) {
    GREEN -> "Autorizad@,<br> no presentas síntomas"
    YELLOW -> "No autorizad@,<br> si presentas síntomas"
    RED -> "No autorizad@,<br> no cumples con ninguna excepción"
    else -> ""
}

fun String.toQrTypeCompany() = when (this) {
    GREEN -> "Autorizad@,<br> tu empresa ha indicado que estás <br> habilitado para ejercer tu labor"
    YELLOW -> "No autorizad@,<br> si presentas síntomas"
    RED -> "No autorizad@,<br> no cumples con ninguna excepción"
    else -> ""
}

fun String.toColorQrType() = when (this) {
    YELLOW -> R.color.amber_600
    GREEN -> R.color.teal_300
    RED -> R.color.pink_400
    else -> R.color.grey_400
}

fun String.toColorBackground() = when (this) {
    YELLOW -> R.drawable.framework_amber
    RED -> R.drawable.framework_pink
    GREEN -> R.drawable.framework_teal
    else -> -1
}

fun String.stringToDate(): Calendar {
    val dateConvert: Calendar = GregorianCalendar()
    val dateFormat = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale("es", "MX"))
    val date: Date
    try {
        dateFormat.parse(this)?.let {
            date = it
            dateConvert.time = date
        }
    } catch (e: ParseException) {
        e.printStackTrace()
    }
    return dateConvert
}

fun String.formatDate(): String {
    val fmt = DateTimeFormat.forPattern("dd/MM/yyyy HH:mm:ss")
    val date = LocalDateTime.parse(this)
    val localDate = fmt.parseLocalDateTime(date.toString(fmt))
    return localDate.toString(fmt)
}

fun String.to24HoursPassed(): Boolean = run {
    if (this.isEmpty())
        return@run true
    val fmt = ISODateTimeFormat.dateHourMinuteSecond()
    val localDate = fmt.parseLocalDateTime(this)

    val dateNow = LocalDateTime.now(DateTimeZone.forID("America/Bogota"))
    val localDateNow = fmt.parseLocalDateTime(dateNow.toString(fmt))

    Hours.hoursBetween(localDate, localDateNow).hours > 24
}

fun String.generateCodeQr(): Bitmap = run {

    val writer = QRCodeWriter()
    val bitMatrix = writer.encode(this, BarcodeFormat.QR_CODE, 512, 512)
    val width = bitMatrix.width
    val height = bitMatrix.height
    val bitmap = Bitmap.createBitmap(width, height, Bitmap.Config.RGB_565)

    for (x in 0 until width) {
        for (y in 0 until height) {
            bitmap.setPixel(x, y, if (bitMatrix.get(x, y)) Color.BLACK else Color.WHITE)
        }
    }

    bitmap
}

fun String.filterWord(list: List<String>) = run {
    val newList = mutableListOf<String>()

    list.forEach {text ->
        if (text.removeAccents().startsWith(this.removeAccents(), true))
            newList.add(text)
    }

    newList
}

fun String.removeAccents() = run {
    val normalized: String = Normalizer.normalize(this, Normalizer.Form.NFD)
    normalized.replace("[^\\p{ASCII}]".toRegex(), "").toLowerCase()
}