package co.gov.ins.guardianes.data.local.repository

import co.gov.ins.guardianes.data.local.dao.HomeLoginDao
import co.gov.ins.guardianes.data.local.mappers.fromDomain
import co.gov.ins.guardianes.data.local.mappers.fromEntity
import co.gov.ins.guardianes.domain.models.LastSelfDiagnosis
import co.gov.ins.guardianes.domain.repository.HomeLoginLocalRepository
import io.reactivex.Completable
import io.reactivex.Flowable

class HomeLoginLocalRepositoryImpl(
    private val homeLoginDao: HomeLoginDao
) : HomeLoginLocalRepository {

    override fun getLastSelfDiagnosis(): Flowable<LastSelfDiagnosis> =
        homeLoginDao.getLastSelfDiagnosis().map {
            if (it.isNotEmpty()) {
                it.last().fromDomain()
            } else {
                throw Throwable("Empty")
            }
        }

    override fun setLastSelfDiagnosis(lastSelfDiagnosis: LastSelfDiagnosis): Completable =
        homeLoginDao.insertLastSelfDiagnosis(lastSelfDiagnosis.fromEntity())

}