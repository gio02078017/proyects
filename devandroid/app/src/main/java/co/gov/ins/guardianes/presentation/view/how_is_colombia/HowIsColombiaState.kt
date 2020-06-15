package co.gov.ins.guardianes.presentation.view.how_is_colombia

import co.gov.ins.guardianes.domain.models.HowIsColombia

sealed class HowIsColombiaState {
    object Loading : HowIsColombiaState()
    class Error(val msg: String?) : HowIsColombiaState()
    class Success(val data: HowIsColombia) : HowIsColombiaState()
}