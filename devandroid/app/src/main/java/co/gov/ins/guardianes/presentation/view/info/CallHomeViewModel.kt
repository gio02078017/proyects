package co.gov.ins.guardianes.presentation.view.info

import co.gov.ins.guardianes.domain.uc.FirebaseEventUc
import co.gov.ins.guardianes.view.base.BaseViewModel

class CallHomeViewModel(
    private val firebaseEventUc: FirebaseEventUc
) : BaseViewModel() {

    fun createEvent(key: String) {
        firebaseEventUc.createEvent(key)
    }
}
