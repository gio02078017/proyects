package co.gov.ins.guardianes.presentation.view.welcome

sealed class WelcomeState {
    object Loading : WelcomeState()
    class Error(val msg: String?) : WelcomeState()
    object SuccessToken : WelcomeState()
}