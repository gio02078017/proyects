package co.gov.ins.guardianes.data.remoto.api

import co.gov.ins.guardianes.data.remoto.models.Permission
import co.gov.ins.guardianes.data.remoto.models.PermissionPost
import io.reactivex.Completable
import io.reactivex.Single
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.POST

interface ApiPermission {

    @GET("v2.0/permission")
    fun getPermissions(@Header("Authorization") authorization: String): Single<List<Permission>>

    @POST("v2.0/permission/user")
    fun postPermissions(@Header("Authorization") authorization: String, @Body list: PermissionPost): Completable
}