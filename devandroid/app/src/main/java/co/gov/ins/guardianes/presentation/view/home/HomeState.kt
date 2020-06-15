package co.gov.ins.guardianes.presentation.view.home

import co.gov.ins.guardianes.presentation.models.MenuHome
import co.gov.ins.guardianes.presentation.models.VersionApp

sealed class HomeState {
    class Success(val data: List<MenuHome>) : HomeState()
    class SuccessVersion(val data: VersionApp) : HomeState()
}