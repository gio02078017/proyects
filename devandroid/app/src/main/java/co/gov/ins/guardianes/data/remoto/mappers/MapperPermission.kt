package co.gov.ins.guardianes.data.remoto.mappers

import co.gov.ins.guardianes.data.remoto.models.Permission
import co.gov.ins.guardianes.data.remoto.models.Permissions
import co.gov.ins.guardianes.domain.models.Permission as Domain
import co.gov.ins.guardianes.domain.models.Permissions as DomainPermission
import co.gov.ins.guardianes.data.remoto.models.PermissionPost
import co.gov.ins.guardianes.domain.models.PermissionPost as DomainPost

fun Permission.fromDomain() =
    Domain(
        id, title, subTitle, icon, accept
    )

fun PermissionPost.fromDomain() =
        DomainPost(
              ArrayList(Permissions.map {
                    it.fromDomain()
                })
        )


fun Permissions.fromDomain() =
        DomainPermission(
                Accept, PermissionsId
        )
