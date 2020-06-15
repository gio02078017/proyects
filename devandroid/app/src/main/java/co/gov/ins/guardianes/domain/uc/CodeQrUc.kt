package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.models.GenerateQrRequest
import co.gov.ins.guardianes.domain.models.GenerateQrResponse
import co.gov.ins.guardianes.domain.repository.CodeQrLocalRepository
import co.gov.ins.guardianes.domain.repository.CodeQrRepository
import co.gov.ins.guardianes.domain.repository.TokenRepository
import co.gov.ins.guardianes.domain.repository.UserPreferences
import co.gov.ins.guardianes.util.ext.retryWithUpdatedTokenIfRequired
import io.reactivex.Flowable
import io.reactivex.Single

class CodeQrUc(
    private val codeQrRepository: CodeQrRepository,
    private val codeQrLocalRepository: CodeQrLocalRepository,
    private val tokenRepository: TokenRepository,
    private val userPreferences: UserPreferences
) {

    fun getCodeQrLocal(): Flowable<GenerateQrResponse> =
        codeQrLocalRepository.getCodeQr()

    fun validateCodeQr(userId: String) =
        Single.defer {
            codeQrRepository.validateCodeQr(
                userPreferences.getAuthorization(),
                userId
            ).flatMap {
                codeQrLocalRepository.setCodeQr(it)
                Single.just(it)
            }
        }.retryWithUpdatedTokenIfRequired(
            tokenRepository = tokenRepository,
            userPreferences = userPreferences
        )

    fun generateCodeQr(generateQrRequest: GenerateQrRequest) =
        codeQrRepository.generateCodeQr(
            userPreferences.getAuthorization(),
            generateQrRequest
        ).flatMap {
            codeQrLocalRepository.setCodeQr(it)
            Single.just(it)
        }.retryWithUpdatedTokenIfRequired(
            tokenRepository = tokenRepository,
            userPreferences = userPreferences
        )
}