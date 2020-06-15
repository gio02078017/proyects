package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.models.ValidateState
import co.gov.ins.guardianes.domain.repository.BluetraceRepository
import co.gov.ins.guardianes.domain.repository.UserPreferences
import io.reactivex.Single

class BluetraceUc(
        private val bluetraceRepository: BluetraceRepository,
        private val userPreferences: UserPreferences
) {

    fun getState() : Single<ValidateState> =
            bluetraceRepository.validateState(userPreferences.getAuthorization())

}