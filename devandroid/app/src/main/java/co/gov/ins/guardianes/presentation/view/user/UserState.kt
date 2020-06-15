package co.gov.ins.guardianes.presentation.view.user


sealed class UserState {
    object Loading : UserState()
    class Error(val msg: String?) : UserState()
    object Success : UserState()
}