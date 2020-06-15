package co.gov.ins.guardianes.data.remoto.repository

import co.gov.ins.guardianes.data.remoto.api.ApiCodeQr
import co.gov.ins.guardianes.data.remoto.mappers.fromData
import co.gov.ins.guardianes.data.remoto.mappers.fromDomain
import co.gov.ins.guardianes.data.remoto.models.GenerateQrResponse
import co.gov.ins.guardianes.domain.models.GenerateQrRequest
import co.gov.ins.guardianes.domain.repository.CodeQrRepository
import io.reactivex.Single

class CodeQrRepositoryImpl(
    private val apiCodeQr: ApiCodeQr
) : CodeQrRepository {

    override fun validateCodeQr(
        authorization: String,
        userId: String
    ): Single<co.gov.ins.guardianes.domain.models.GenerateQrResponse> =
        apiCodeQr.validateCodeQr(
            authorization,
            userId
        ).flatMap {
            val response = GenerateQrResponse(
                codeQr = it.codeQr.codeQr,
                expirationDate = it.codeQr.expirationDate,
                showAlert = it.codeQr.showAlert,
                alertMessage = it.codeQr.alertMessage,
                qrType = it.codeQr.qrType,
                qrMessage = it.codeQr.qrMessage,
                validQr = it.validQr,
                validMessage = it.message,
                existsCompany = it.codeQr.existsCompany
            )
            Single.just(response.fromDomain())
        }

    override fun generateCodeQr(
        authorization: String,
        generateQrRequest: GenerateQrRequest
    ): Single<co.gov.ins.guardianes.domain.models.GenerateQrResponse> =
        apiCodeQr.generateCodeQr(
            authorization,
            generateQrRequest.fromData()
        ).flatMap {
            val response = GenerateQrResponse(
                codeQr = it.codeQr,
                expirationDate = it.expirationDate,
                showAlert = it.showAlert,
                alertMessage = it.alertMessage,
                qrType = it.qrType,
                qrMessage = it.qrMessage,
                validQr = true,
                validMessage = "",
                existsCompany = it.existsCompany
            )
            Single.just(response.fromDomain())
        }
}