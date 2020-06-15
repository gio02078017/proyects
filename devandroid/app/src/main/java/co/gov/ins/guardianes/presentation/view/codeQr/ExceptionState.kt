package co.gov.ins.guardianes.presentation.view.codeQr

import co.gov.ins.guardianes.presentation.models.ExceptionDecreto
import co.gov.ins.guardianes.presentation.models.ExceptionResolution

sealed class ExceptionState {
    object Loading : ExceptionState()
    class Error(val msg: String?) : ExceptionState()
    class SuccessDecreto(val data: List<ExceptionDecreto>) : ExceptionState()
    class SuccessResolution(val data: List<ExceptionResolution>) : ExceptionState()
}