package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.models.UserResponse
import co.gov.ins.guardianes.domain.repository.UserPreferences

class UserPreferencesUc(
    private val userPreferences: UserPreferences
) {

    fun getUserId() = userPreferences.getUserId()

    fun getUser() = userPreferences.getUser()

    fun setUser(user: UserResponse) = userPreferences.setUser(user)
}