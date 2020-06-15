package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.repository.MenuHomeRepository
import co.gov.ins.guardianes.domain.repository.UserPreferences

class MenuHomeUc(
    private val menuHomeRepository: MenuHomeRepository,
    private val userPreferences: UserPreferences
) {

    fun getListMenu() = menuHomeRepository.getMenuHome()

    fun getUser() = userPreferences.getUser()

    fun isToken() = !userPreferences.getToken()?.token.isNullOrEmpty()

    fun preferenceClear() = userPreferences.clear()

    fun getAppVersion() = menuHomeRepository.getAppVersion()

    fun getPermission() = userPreferences.getPermissions()

    fun getCoachMark() = userPreferences.getCoachMark()

    fun setCoachMark(boolean: Boolean) = userPreferences.setCoachMark(boolean)
}