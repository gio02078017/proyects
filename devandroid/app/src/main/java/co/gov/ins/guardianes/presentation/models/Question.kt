package co.gov.ins.guardianes.presentation.models

import co.gov.ins.guardianes.util.Constants
import co.gov.ins.guardianes.util.Constants.Factor.HAVE_RECEIVED_MEDICAL_ATTENTION_ONCE
import co.gov.ins.guardianes.util.Constants.Factor.HAVE_VISITED_THE_DOCTOR_MORE_AT_ONCE
import co.gov.ins.guardianes.util.Constants.Factor.NO_HAVE_RECEIVED_MEDICAL_ATTENTION_ONCE

data class Question(
    val id: String,
    val title: String,
    val description: String,
    val field: String,
    val multiple: Boolean,
    val order: Int,
    val answers: List<Answer>
)

data class Answer(
    val id: String,
    val text: String,
    val value: String,
    val order: Int,
    var form: Int = 0,
    var isSelect: Boolean = false,
    var type: String = ""
) {
    val isSingle: Boolean
        get() = value.replace(
            " ",
            ""
        ) == Constants.Symptom.NONE || value == Constants.Symptom.NONE2
                || value.replace(
            " ",
            ""
        ) == Constants.Symptom.NONE || value == Constants.Symptom.NONE3
                || value.replace(
            " ",
            ""
        ) == NO_HAVE_RECEIVED_MEDICAL_ATTENTION_ONCE
                || value.replace(
            " ",
            ""
        ) == HAVE_RECEIVED_MEDICAL_ATTENTION_ONCE
                || value.replace(
            " ",
            ""
        ) == HAVE_VISITED_THE_DOCTOR_MORE_AT_ONCE
}