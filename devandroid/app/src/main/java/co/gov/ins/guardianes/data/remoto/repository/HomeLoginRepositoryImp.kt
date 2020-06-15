package co.gov.ins.guardianes.data.remoto.repository

import co.gov.ins.guardianes.data.remoto.api.ApiHomeLogin
import co.gov.ins.guardianes.data.remoto.models.fromDomain
import co.gov.ins.guardianes.domain.models.LastSelfDiagnosis
import co.gov.ins.guardianes.domain.repository.HomeLoginRepository
import io.reactivex.Single

class HomeLoginRepositoryImp(val apiHomeLogin: ApiHomeLogin) : HomeLoginRepository {

    override fun queryLastSelfDiagnosis(
        authorization: String,
        id: String
    ): Single<LastSelfDiagnosis> =
        apiHomeLogin.queryLastSelfDiagnosis(authorization, id).flatMap {
            Single.just(it.fromDomain())
        }
}