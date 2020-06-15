package co.gov.ins.guardianes.util.ext

import android.annotation.SuppressLint
import android.content.Context
import android.os.Build
import android.text.Html
import android.text.Spanned
import android.widget.EditText
import android.widget.LinearLayout
import android.widget.TextView
import androidx.core.content.ContextCompat
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.util.Constants
import org.joda.time.DateTimeZone
import org.joda.time.LocalDateTime
import org.joda.time.format.DateTimeFormat
import java.text.SimpleDateFormat
import java.util.*

fun Context.fromHtml(text: String): Spanned = run {
    if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.N) {
        Html.fromHtml(
            text,
            Html.FROM_HTML_MODE_COMPACT
        )

    } else {
        Html.fromHtml(text)
    }
}

fun Context.getColorCompat(color: Int) =
    ContextCompat.getColor(this, color)

fun String.isValidateName(): Boolean {
    return matches(Constants.Regex.IS_VALID_NAME.toRegex())
}

fun String.isValidateDocumentNumber(): Boolean {
    return matches(Constants.Regex.NUMBER_DONT_START_WITH_0.toRegex())
}

fun String.isValidateDocumentNumberTwo(): Boolean {
    return matches(Constants.Regex.IS_NUMBER_TWO.toRegex())
}

fun Context.getDrawableCompat(idDrawable: Int) =
    ContextCompat.getDrawable(this, idDrawable)

fun Context.showError(textView: TextView?, editText: EditText, layout: LinearLayout?) = run {
    textView?.setTextColor(ContextCompat.getColor(this, R.color.show_error))
    editText.setTextColor(ContextCompat.getColor(this, R.color.show_error))
    if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP)
        layout?.background = this.getDrawable(R.drawable.input_background_error)
}

fun Context.showNormal(textView: TextView?, editText: EditText, layout: LinearLayout?) = run {
    textView?.setTextColor(ContextCompat.getColor(this, R.color.blue_black_btn_help))
    editText.setTextColor(ContextCompat.getColor(this, R.color.blue_black_btn_help))
    if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP)
        layout?.background = this.getDrawable(R.drawable.input_background)
}

fun Context.getDateNow(): String {
    val fmt = DateTimeFormat.forPattern("dd/MM/yyyy HH:mm:ss")
    val dateNow = LocalDateTime.now(DateTimeZone.forID("America/Bogota"))
    val localDateNow = fmt.parseLocalDateTime(dateNow.toString(fmt))
    return localDateNow.toString(fmt)
}

@SuppressLint("SimpleDateFormat")
fun Context.getDateFormat(): String{
    val sdf = SimpleDateFormat("dd/MM/yyyy")
    return sdf.format(Date())
}
