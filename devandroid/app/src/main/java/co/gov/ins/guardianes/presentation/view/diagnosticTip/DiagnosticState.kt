package co.gov.ins.guardianes.presentation.view.diagnosticTip

import co.gov.ins.guardianes.presentation.models.Diagnostic

sealed class DiagnosticState {
    class Success(val data: Diagnostic) : DiagnosticState()
    object Error : DiagnosticState()
}
