package co.gov.ins.guardianes.presentation.view.add_participant

sealed class AddParticipantState {
    object Loading : AddParticipantState()
    class Error(val msg: String?) : AddParticipantState()
    object Success : AddParticipantState()
    object ChangeListeners : AddParticipantState()
}
