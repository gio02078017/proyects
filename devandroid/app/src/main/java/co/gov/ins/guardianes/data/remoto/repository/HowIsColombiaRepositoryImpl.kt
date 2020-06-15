package co.gov.ins.guardianes.data.remoto.repository

import co.gov.ins.guardianes.data.remoto.api.ApiHowIsColombia
import co.gov.ins.guardianes.domain.models.HowIsColombia
import co.gov.ins.guardianes.domain.repository.howIsColombia.HowIsColombiaRepository
import io.reactivex.Single

class HowIsColombiaRepositoryImpl(private val apiHowIsColombia: ApiHowIsColombia) :
    HowIsColombiaRepository {
    override fun getData(): Single<HowIsColombia> =
        apiHowIsColombia.getDataStatistics().flatMap {
            if (!it.error) {
                Single.just(it)
            } else {
                Single.error(Throwable())
            }
        }
}