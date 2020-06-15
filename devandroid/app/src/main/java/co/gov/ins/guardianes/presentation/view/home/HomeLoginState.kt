package co.gov.ins.guardianes.presentation.view.home

sealed class HomeLoginState {
    class Success(val data: String) : HomeLoginState()
}