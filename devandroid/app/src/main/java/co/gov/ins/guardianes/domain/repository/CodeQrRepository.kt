package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.GenerateQrRequest
import co.gov.ins.guardianes.domain.models.GenerateQrResponse
import co.gov.ins.guardianes.domain.models.ValidateCodeQr
import io.reactivex.Single

interface CodeQrRepository {

    fun validateCodeQr(
        authorization: String,
        userId: String): Single<GenerateQrResponse>

    fun generateCodeQr(
        authorization: String,
        generateQrRequest: GenerateQrRequest): Single<GenerateQrResponse>
}