package co.gov.ins.guardianes.data.local.repository

import co.gov.ins.guardianes.data.local.dao.DecretoDao
import co.gov.ins.guardianes.data.local.dao.ResolutionDao
import co.gov.ins.guardianes.data.local.mappers.fromDomain
import co.gov.ins.guardianes.data.local.mappers.fromEntity
import co.gov.ins.guardianes.domain.models.ExceptionDecreto
import co.gov.ins.guardianes.domain.models.ExceptionResolution
import co.gov.ins.guardianes.domain.repository.ExceptionLocalRepository
import io.reactivex.Completable
import io.reactivex.Flowable

class ExceptionLocalRepositoryImpl(
    private val decretoDao: DecretoDao,
    private val resolutionDao: ResolutionDao
) : ExceptionLocalRepository {


    override fun getDecretos(): Flowable<List<ExceptionDecreto>> =
        decretoDao.getDecretos().map { response ->
            if (response.isEmpty()) {
                throw Throwable("Empty")
            } else{
                response.map {
                    it.fromDomain()
                }
            }
        }


    override fun setDecretos(exceptionDecreto: List<ExceptionDecreto>): Completable =
            decretoDao.insertDecretos(exceptionDecreto.map {
            it.fromEntity()
        })

    override fun updateDecretoSelect(exceptionDecreto: ExceptionDecreto): Completable =
            decretoDao.updateDecretoSelect(exceptionDecreto.fromEntity())

    override fun getResolutions(): Flowable<List<ExceptionResolution>> =
            resolutionDao.getResolutions().map { response ->
                if (response.isEmpty()) {
                    throw Throwable("Empty")
                } else{
                    response.map {
                        it.fromDomain()
                    }
                }
            }

    override fun setResolutions(exceptionResolution: List<ExceptionResolution>): Completable =
            resolutionDao.insertResolutions(exceptionResolution.map {
                it.fromEntity()
            })

    override fun updateResolutionSelect(exceptionResolution: ExceptionResolution): Completable =
            resolutionDao.updateResolutionSelect(exceptionResolution.fromEntity())

}