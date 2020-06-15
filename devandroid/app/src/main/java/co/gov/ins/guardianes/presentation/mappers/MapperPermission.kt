package co.gov.ins.guardianes.presentation.mappers

import co.gov.ins.guardianes.domain.models.Permission
import co.gov.ins.guardianes.domain.models.Permissions
import co.gov.ins.guardianes.presentation.models.Permission as Domain
import co.gov.ins.guardianes.presentation.models.Permissions as DomainPermissions
import co.gov.ins.guardianes.data.remoto.models.Permissions as DataPermissions
import co.gov.ins.guardianes.domain.models.PermissionPost
import co.gov.ins.guardianes.data.remoto.models.PermissionPost as DomainPost
import co.gov.ins.guardianes.presentation.models.PermissionPost as PresentPost

fun Permission.fromDomain() =
    Domain(
        id, title, subTitle, icon, accept
    )

fun PermissionPost.fromData() =
        DomainPost(
        ArrayList(Permissions.map {
            it.fromData()
        })
    )

fun DomainPermissions.fromData() =
        Permissions(
                Accept, PermissionsId
        )


fun Permissions.fromData() =
        DataPermissions(
                Accept, PermissionsId
        )


fun PresentPost.fromPresentation() =
        PermissionPost (
              ArrayList(Permissions.map {
                    it.fromData()
                })
        )





