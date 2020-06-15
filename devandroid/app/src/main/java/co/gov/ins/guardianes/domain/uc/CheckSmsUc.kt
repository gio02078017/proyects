package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.models.SendSms
import co.gov.ins.guardianes.domain.models.TokenResponse
import co.gov.ins.guardianes.domain.repository.CheckSmsRepository
import co.gov.ins.guardianes.domain.repository.UserPreferences


class CheckSmsUc(
    private val checkSmsRepository: CheckSmsRepository,
    private val userPreferences: UserPreferences
) {
      fun requestSms(infoSms: SendSms) = checkSmsRepository.requestSms(infoSms)

    fun verifySms(dataSms: SendSms) = checkSmsRepository.verifySms(dataSms).map {
        userPreferences.setUser(it.user)
        userPreferences.setToken(TokenResponse(it.token, it.refreshToken))
        it.responseCode
    }
}