package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.models.Permission
import co.gov.ins.guardianes.domain.models.PermissionPost
import co.gov.ins.guardianes.domain.repository.PermissionsRepository
import co.gov.ins.guardianes.domain.repository.UserPreferences
import io.reactivex.Single

class PermissionsUc(
    private val permissionsRepository: PermissionsRepository,
    private val userPreferences: UserPreferences
) {

    fun getPermissions(): Single<List<Permission>> = run {
        permissionsRepository.getPermissions(userPreferences.getAuthorization()).flatMap {
            Single.just(it)
        }
    }

    fun postPermissions(permissionPost: PermissionPost) = run {
        permissionsRepository.postPermissions(userPreferences.getAuthorization(),  permissionPost)
    }

    fun setPermissions(boolean: Boolean) = userPreferences.setPermissions(boolean)


}