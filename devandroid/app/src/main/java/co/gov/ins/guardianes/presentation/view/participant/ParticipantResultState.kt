package co.gov.ins.guardianes.presentation.view.participant

sealed class ParticipantResultState {
    class Success(val data: String) : ParticipantResultState()
}