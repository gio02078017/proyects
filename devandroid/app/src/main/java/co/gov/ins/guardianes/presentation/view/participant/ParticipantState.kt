package co.gov.ins.guardianes.presentation.view.participant

import co.gov.ins.guardianes.presentation.models.LastDiagnostic
import co.gov.ins.guardianes.presentation.models.Participant

sealed class ParticipantState {
    object Loading : ParticipantState()
    class Error(val msg: String?) : ParticipantState()
    class Success(val data: List<Participant>) : ParticipantState()
    class SuccessDate(val data: List<LastDiagnostic>) : ParticipantState()
    class SuccessUser(val data: String) : ParticipantState()
}