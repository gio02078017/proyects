package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.repository.UserPreferences

class SplashUc(
    private val userPreferences: UserPreferences
) {

    fun isTokenRegister() = userPreferences.getUserToken().isNotEmpty()

    fun isNewToken() = !userPreferences.getToken()?.token.isNullOrEmpty()
}