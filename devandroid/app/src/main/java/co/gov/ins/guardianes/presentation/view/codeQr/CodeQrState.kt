package co.gov.ins.guardianes.presentation.view.codeQr

import co.gov.ins.guardianes.presentation.models.GenerateQrResponse
import co.gov.ins.guardianes.presentation.models.ValidateCodeQr

sealed class CodeQrState {

    class Success(val data: GenerateQrResponse) : CodeQrState()
    class SuccessValidate(val data: GenerateQrResponse) : CodeQrState()
    class SuccessGenerate(val data: GenerateQrResponse) : CodeQrState()
    class Error(val msg: String?) : CodeQrState()
}