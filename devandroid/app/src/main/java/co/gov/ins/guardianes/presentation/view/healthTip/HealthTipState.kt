package co.gov.ins.guardianes.presentation.view.healthTip

import co.gov.ins.guardianes.presentation.models.HealthTip

sealed class HealthTipState {
    object Loading : HealthTipState()
    class Error(val msg: String?) : HealthTipState()
    class Success(val data: List<HealthTip>) : HealthTipState()
}