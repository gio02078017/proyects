package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.Permission
import co.gov.ins.guardianes.domain.models.PermissionPost
import io.reactivex.Completable
import io.reactivex.Single

interface PermissionsRepository {

    fun getPermissions(authorization: String): Single<List<Permission>>

    fun postPermissions(authorization: String, permissionPost: PermissionPost): Completable
}