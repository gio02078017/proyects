package co.gov.ins.guardianes.presentation.view.quarantineHome

import co.gov.ins.guardianes.domain.uc.FirebaseEventUc
import co.gov.ins.guardianes.view.base.BaseViewModel

class QuarantineHomeViewModel(
    private val firebaseEventUc: FirebaseEventUc
) : BaseViewModel() {

    fun createEvent(key: String) {
        firebaseEventUc.createEvent(key)
    }
}
