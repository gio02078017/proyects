package co.gov.ins.guardianes.data.remoto.repository

import co.gov.ins.guardianes.data.remoto.api.ApiBluetrace
import co.gov.ins.guardianes.data.remoto.mappers.fromDomain
import co.gov.ins.guardianes.domain.models.ValidateState
import co.gov.ins.guardianes.domain.repository.BluetraceRepository
import io.reactivex.Single

class BluetraceRepositoryImpl (
        private val apiBluetrace: ApiBluetrace
) : BluetraceRepository {

    override fun validateState(authorization: String) : Single<ValidateState> =
            apiBluetrace.validateState(authorization).map {
                it.fromDomain()
            }

}