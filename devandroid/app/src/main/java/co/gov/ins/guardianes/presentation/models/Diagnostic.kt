package co.gov.ins.guardianes.presentation.models

import android.os.Parcelable
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.util.Constants
import co.gov.ins.guardianes.util.Constants.Diagnostic.ALERT_RULE_3_1
import co.gov.ins.guardianes.util.Constants.Diagnostic.ALERT_RULE_3_2
import co.gov.ins.guardianes.util.Constants.Diagnostic.NORMAL_RULE_1
import co.gov.ins.guardianes.util.Constants.Diagnostic.WARNING_RULE_2_1
import co.gov.ins.guardianes.util.Constants.Diagnostic.WARNING_RULE_2_2
import co.gov.ins.guardianes.util.Constants.Diagnostic.WARNING_RULE_2_3
import co.gov.ins.guardianes.util.Constants.Diagnostic.WARNING_RULE_2_4
import kotlinx.android.parcel.Parcelize

@Parcelize
data class Diagnostic(
    val id: String,
    val text: String,
    val value: String,
    val categories: List<Categories>
) : Parcelable {

    val colorText
        get() = when (value) {
            Constants.Key.NORMAL -> {
                R.color.color_diagnostic_normal
            }
            Constants.Key.WARNING -> {
                R.color.color_diagnosticWarning
            }
            else -> {
                R.color.color_diagnosticAlert
            }
        }

    val title: Int
        get() {
            return when (id) {
                NORMAL_RULE_1 -> R.string.title_good
                WARNING_RULE_2_1 -> R.string.title_warning_1
                WARNING_RULE_2_2 -> R.string.title_warning_2
                WARNING_RULE_2_3 -> R.string.title_warning_3
                WARNING_RULE_2_4 -> R.string.title_warning_4
                ALERT_RULE_3_1 -> R.string.title_alert_1
                ALERT_RULE_3_2 -> R.string.title_alert_2
                else -> 0
            }
        }

    val description: Int
        get() {
            return when (id) {
                NORMAL_RULE_1 -> R.string.description_good
                WARNING_RULE_2_1 -> R.string.description_warning_1
                WARNING_RULE_2_2 -> R.string.description_warning_2
                WARNING_RULE_2_3 -> R.string.description_warning_3
                WARNING_RULE_2_4 -> R.string.description_warning_4
                ALERT_RULE_3_1 -> R.string.description_alert_1
                ALERT_RULE_3_2 -> R.string.description_alert_2
                else -> 0
            }
        }
}

@Parcelize
data class Categories(
    val id: Int,
    val text: String,
    val description: String,
    val image: String,
    val slug: String,
    val order: Int,
    val recommendations: List<Recommendations>,
    var isSelect: Boolean = false
) : Parcelable

@Parcelize
data class Recommendations(
    val id: Int,
    val text: String,
    val description: String,
    val slug: String,
    val order: Int
) : Parcelable