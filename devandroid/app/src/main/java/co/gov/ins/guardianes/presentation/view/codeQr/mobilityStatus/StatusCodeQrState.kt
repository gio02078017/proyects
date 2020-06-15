package co.gov.ins.guardianes.presentation.view.codeQr.mobilityStatus

import co.gov.ins.guardianes.presentation.models.GenerateQrResponse

sealed class StatusCodeQrState {
    class InformationQr(val data: GenerateQrResponse): StatusCodeQrState()
    class QrInvalid(val data: GenerateQrResponse): StatusCodeQrState()
    class IsShowAlertDialig(val isShow: Boolean): StatusCodeQrState()
}