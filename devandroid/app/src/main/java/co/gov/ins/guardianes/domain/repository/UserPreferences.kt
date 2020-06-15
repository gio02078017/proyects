package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.TokenResponse
import co.gov.ins.guardianes.domain.models.UserResponse

interface UserPreferences {

    fun getUserId(): String

    fun getUser(): UserResponse?

    fun setUser(user: UserResponse): Boolean

    fun getUserToken(): String

    fun getDeviceId(): String

    fun clear()

    fun getAuthorization(): String

    fun setToken(tokenResponse: TokenResponse): Boolean

    fun setPermissions(boolean: Boolean): Boolean

    fun setCoachMark(boolean: Boolean): Boolean

    fun getPermissions(): Boolean

    fun getCoachMark(): Boolean

    fun getToken(): TokenResponse?
}