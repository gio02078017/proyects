package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.LastSelfDiagnosis
import io.reactivex.Single

interface HomeLoginRepository {

    fun queryLastSelfDiagnosis(
        authorization: String,
        id: String
    ): Single<LastSelfDiagnosis>
}