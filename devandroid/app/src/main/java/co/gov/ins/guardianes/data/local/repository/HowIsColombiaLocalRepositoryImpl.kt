package co.gov.ins.guardianes.data.local.repository

import co.gov.ins.guardianes.data.local.dao.HowIsColombiaDao
import co.gov.ins.guardianes.data.local.mappers.from
import co.gov.ins.guardianes.domain.models.HowIsColombia
import co.gov.ins.guardianes.domain.repository.howIsColombia.HowIsColombiaLocalRepository
import io.reactivex.Completable
import io.reactivex.Flowable

class HowIsColombiaLocalRepositoryImpl(
    private val howIsColombiaDao: HowIsColombiaDao
) : HowIsColombiaLocalRepository {

    override fun getDataLocal(): Flowable<HowIsColombia> =
        howIsColombiaDao.getDataColombia().map {
            it.from()
        }

    override fun insertDataLocal(howIsColombia: HowIsColombia) =
        howIsColombiaDao.insertDataColombia(howIsColombia.from())
}