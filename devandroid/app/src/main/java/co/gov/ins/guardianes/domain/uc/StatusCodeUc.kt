package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.models.LastSelfDiagnosis
import co.gov.ins.guardianes.domain.repository.CodeQrLocalRepository
import co.gov.ins.guardianes.domain.repository.UserPreferences
import io.reactivex.Single

class StatusCodeUc (
        private val userPreferences: UserPreferences,
        private val codeQrLocalRepository: CodeQrLocalRepository
) {

    fun getUser() = userPreferences.getUser()

    fun getInformationQr() =
            codeQrLocalRepository.getCodeQr()

    fun queryLastSelfDiagnosis(): Single<LastSelfDiagnosis> =
            codeQrLocalRepository.getLastSelftDiagnosis()
}