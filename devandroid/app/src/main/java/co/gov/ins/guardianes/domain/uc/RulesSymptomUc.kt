package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.presentation.models.Answer
import co.gov.ins.guardianes.util.Constants
import co.gov.ins.guardianes.util.Constants.Risk.FORM_SYMPTOM_2
import co.gov.ins.guardianes.util.Constants.Risk.FORM_SYMPTOM_3
import co.gov.ins.guardianes.util.Constants.Symptom.NONE
import co.gov.ins.guardianes.util.Constants.Symptom.NONE3

class RulesSymptomUc {

    fun validateRisks(listSymptom: List<Answer>, form: Int): Int {
        return when (form) {
            Constants.RULES.RULES_1 -> {
                rulesForm1(listSymptom)
            }
            Constants.RULES.RULES_2 -> {
                rulesForm2(listSymptom)
            }
            Constants.RULES.RULES_3 -> {
                rulesForm3(listSymptom)
            }
            Constants.RULES.RULES_4 -> {
                rulesForm4(listSymptom)
            }
            else -> Constants.Risk.ERROR
        }
    }

    private fun rulesForm1(listSymptom: List<Answer>): Int {
        return when {
            listSymptom.count { it.isSingle } > 1 -> Constants.AnswerRisk.GOOD

            listSymptom.any { it.value == Constants.Symptom.FEVER || it.value == Constants.Symptom.DIFFICULTY_BREATHING } ->
                Constants.AnswerRisk.ALERT_1

            listSymptom.count { it.type == Constants.Key.SYMPTOM } in 1..2 -> Constants.AnswerRisk.ALERT_1

            listSymptom.count { it.type == Constants.Key.SYMPTOM } in 3..7 -> Constants.AnswerRisk.ALERT_1

            listSymptom.count { it.type == Constants.Key.FACTOR } in 1..4 -> Constants.AnswerRisk.ALERT_1

            else -> Constants.Risk.ERROR
        }
    }

    private fun rulesForm2(listSymptom: List<Answer>): Int {
        return when {
            listSymptom.any { it.value == Constants.Symptom.FEVER } -> Constants.AnswerRisk.ALERT_2

            listSymptom.any { it.value == Constants.Symptom.DIFFICULTY_BREATHING } -> Constants.AnswerRisk.ALERT_2

            listSymptom.count { it.type == Constants.Key.SYMPTOM } in 3..7 -> Constants.AnswerRisk.ALERT_2

            listSymptom.any { it.value == Constants.Factor.WORKER_HEALTH } -> Constants.AnswerRisk.WARNING_2
            else -> Constants.AnswerRisk.WARNING_1
        }
    }

    private fun rulesForm3(listSymptom: List<Answer>): Int {
        return when {
            listSymptom.count {
                (it.form == FORM_SYMPTOM_2 && it.type == Constants.Key.SYMPTOM) && it.value != Constants.Symptom.FEVER_HIGH
            } > 0 && listSymptom.any { it.value == Constants.Symptom.FEVER_HIGH } ->
                Constants.AnswerRisk.ALERT_3

            listSymptom.any { it.value == Constants.Factor.NO_HAVE_RECEIVED_MEDICAL_ATTENTION_ONCE } &&
                    listSymptom.any { (it.form == FORM_SYMPTOM_2 && it.type == Constants.Key.SYMPTOM) && it.value != NONE } ->
                Constants.AnswerRisk.ALERT_3

            listSymptom.any { it.value == Constants.Factor.HAVE_VISITED_THE_DOCTOR_MORE_AT_ONCE } &&
                    listSymptom.any { it.form == FORM_SYMPTOM_2 && it.type == Constants.Key.SYMPTOM && it.value != NONE } ->
                Constants.AnswerRisk.ALERT_4

            else -> Constants.AnswerRisk.WARNING_3
        }
    }

    private fun rulesForm4(listSymptom: List<Answer>): Int {
        val filer = listSymptom.filter { it.form == 6 || it.form == 7 }
        println(filer)
        return when {
            filer.any { it.type == Constants.Key.SYMPTOM && it.value == NONE } &&
                    filer.any { it.type == Constants.Key.FACTOR && it.value == NONE3 } -> Constants.AnswerRisk.WARNING_4
            else -> Constants.AnswerRisk.ALERT_5
        }
    }
}