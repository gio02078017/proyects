package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.ValidateState
import io.reactivex.Single

interface BluetraceRepository {

    fun validateState (
            authorization: String
    ): Single<ValidateState>
}