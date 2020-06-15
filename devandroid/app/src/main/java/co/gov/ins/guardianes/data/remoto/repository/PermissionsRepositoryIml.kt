package co.gov.ins.guardianes.data.remoto.repository

import co.gov.ins.guardianes.data.remoto.api.ApiPermission
import co.gov.ins.guardianes.data.remoto.mappers.fromDomain
import co.gov.ins.guardianes.domain.models.PermissionPost
import co.gov.ins.guardianes.domain.repository.PermissionsRepository
import co.gov.ins.guardianes.presentation.mappers.fromData
import io.reactivex.Single

class PermissionsRepositoryIml(private val apiPermission: ApiPermission) :
    PermissionsRepository {

    override fun getPermissions(authorization: String) =
            apiPermission.getPermissions(authorization).flatMap { request ->
                Single.just(request.map {
                    it.fromDomain()
                })
            }

    override fun postPermissions(authorization: String, permissionPost: PermissionPost) =
            apiPermission.postPermissions(authorization, permissionPost.fromData())

}

