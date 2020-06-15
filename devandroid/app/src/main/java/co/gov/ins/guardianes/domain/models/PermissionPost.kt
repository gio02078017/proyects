package co.gov.ins.guardianes.domain.models

data class PermissionPost(
        val Permissions: ArrayList<Permissions>
)

data class Permissions(
        val Accept: String,
        val PermissionsId: Int
)