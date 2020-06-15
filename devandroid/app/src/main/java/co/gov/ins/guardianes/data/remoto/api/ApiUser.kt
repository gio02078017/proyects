package co.gov.ins.guardianes.data.remoto.api

import co.gov.ins.guardianes.data.remoto.models.BaseRequestUser
import co.gov.ins.guardianes.data.remoto.models.UserRequest
import co.gov.ins.guardianes.data.remoto.models.UserResponse
import io.reactivex.Single
import retrofit2.http.Body
import retrofit2.http.POST

interface ApiUser {
    @POST("v1.0/user/first-register")
    fun registerUser(
        @Body userRequest: UserRequest
    ): Single<BaseRequestUser<UserResponse>>
}