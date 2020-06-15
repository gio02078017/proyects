package co.gov.ins.guardianes.presentation.view.survey

import co.gov.ins.guardianes.presentation.models.Question

sealed class SymptomState {
    object Loading : SymptomState()
    class Success(val data: List<Question>) : SymptomState()
    class SuccessName(val name: String) : SymptomState()
    object SuccessAnswers : SymptomState()
    class GetRulesRisk(val risk: Int) : SymptomState()
    class Error(val message: String?) : SymptomState()
}
