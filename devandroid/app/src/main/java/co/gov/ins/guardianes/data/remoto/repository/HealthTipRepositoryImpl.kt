package co.gov.ins.guardianes.data.remoto.repository

import co.gov.ins.guardianes.data.remoto.api.ApiHealthTip
import co.gov.ins.guardianes.data.remoto.mappers.fromDomain
import co.gov.ins.guardianes.domain.models.HealthTip
import co.gov.ins.guardianes.domain.repository.HealthTipRepository
import io.reactivex.Single

class HealthTipRepositoryImpl(
    private val apiHealthTip: ApiHealthTip
) : HealthTipRepository {

    override fun getHealthTip(): Single<List<HealthTip>> =
        apiHealthTip.getHealthTip().flatMap { request ->
            if (!request.error) {
                Single.just(request.data.map {
                    it.fromDomain()
                })
            } else {
                Single.error(Throwable())
            }
        }
}