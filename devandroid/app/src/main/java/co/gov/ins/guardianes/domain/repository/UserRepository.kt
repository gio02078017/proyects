package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.UserRequest
import co.gov.ins.guardianes.domain.models.UserResponse
import io.reactivex.Single

interface UserRepository {

    fun registerUser(userRequest: UserRequest): Single<UserResponse>

}
