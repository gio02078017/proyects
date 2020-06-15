package co.gov.ins.guardianes.presentation.view.permissions_and_privacy

import co.gov.ins.guardianes.presentation.models.Permission

sealed class PermissionsState {
    object Loading : PermissionsState()
    class Error(val msg: String?) : PermissionsState()
    class Success(val data: List<Permission>) : PermissionsState()
    object SuccessComplete : PermissionsState()
}