package co.gov.ins.guardianes.presentation.view.smsCheck

import co.gov.ins.guardianes.presentation.models.SendSmsResponse


sealed class CheckSmsState {
    object Loading : CheckSmsState()
    object UserFail : CheckSmsState()
    class Error(val msg: String?) : CheckSmsState()
    class SuccessVerify(val data: String) : CheckSmsState()
    class Success(val data: SendSmsResponse) : CheckSmsState()
}