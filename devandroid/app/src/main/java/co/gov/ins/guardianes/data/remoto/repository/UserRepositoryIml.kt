package co.gov.ins.guardianes.data.remoto.repository


import co.gov.ins.guardianes.data.remoto.api.ApiUser
import co.gov.ins.guardianes.data.remoto.mappers.fromData
import co.gov.ins.guardianes.data.remoto.mappers.fromDomain
import co.gov.ins.guardianes.domain.models.UserRequest
import co.gov.ins.guardianes.domain.models.UserResponse
import co.gov.ins.guardianes.domain.repository.UserRepository
import io.reactivex.Single

class UserRepositoryIml(private val apiUser: ApiUser) :
    UserRepository {

    override fun registerUser(userRequest: UserRequest): Single<UserResponse> =
        apiUser.registerUser(userRequest.fromData()).flatMap { request ->
            if (!request.error) {
                Single.just(request.user.fromDomain())
            } else {
                Single.error(Throwable())
            }
        }

}

