package co.gov.ins.guardianes.presentation.view.bluetrace

sealed class BluetraceState {

    class SuccessState(val data: Boolean) : BluetraceState()

}