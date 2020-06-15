package co.gov.ins.guardianes.data.local.repository

import co.gov.ins.guardianes.data.local.dao.HealthTipDao
import co.gov.ins.guardianes.data.local.mappers.fromDomain
import co.gov.ins.guardianes.data.local.mappers.fromEntities
import co.gov.ins.guardianes.domain.models.HealthTip
import co.gov.ins.guardianes.domain.repository.HealthTipLocalRepository
import io.reactivex.Completable
import io.reactivex.Flowable

class HealthTipLocalRepositoryImpl(
    private val healthTipDao: HealthTipDao
) : HealthTipLocalRepository {

    override fun getHealthTipLocal(): Flowable<List<HealthTip>> =
        healthTipDao.getTis().map { list ->
            if (list.isEmpty()) {
                throw Throwable("Empty")
            } else {
                list.map {
                    it.fromDomain()
                }
            }

        }

    override fun setHealthTipLocal(list: List<HealthTip>): Completable =
        healthTipDao.insertTips(list.map {
            it.fromEntities()
        })

}