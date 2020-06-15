package co.gov.ins.guardianes.domain.uc


import co.gov.ins.guardianes.domain.models.UserRequest
import co.gov.ins.guardianes.domain.repository.UserPreferences
import co.gov.ins.guardianes.domain.repository.UserRepository
import io.reactivex.Completable

class UserUc(
    private val userRepository: UserRepository,
    private val userPreferences: UserPreferences
) {

    fun registerUser(
        firstName: String,
        lastName: String,
        countryCode: String,
        phoneNumber: String,
        documentNumber: String,
        documentType: String
    ) = run {
        val user = UserRequest(
            firstName,
            lastName,
            countryCode,
            phoneNumber,
            documentType,
            documentNumber,
            userPreferences.getDeviceId()
        )
        userRepository.registerUser(user).flatMapCompletable {
            userPreferences.setUser(it)
            Completable.complete()
        }
    }
}