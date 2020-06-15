package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.ExceptionDecreto
import co.gov.ins.guardianes.domain.models.ExceptionResolution
import io.reactivex.Completable
import io.reactivex.Flowable

interface ExceptionLocalRepository {

    fun getDecretos(): Flowable<List<ExceptionDecreto>>

    fun setDecretos(exceptionDecreto: List<ExceptionDecreto>): Completable

    fun updateDecretoSelect(exceptionDecreto: ExceptionDecreto): Completable

    fun getResolutions(): Flowable<List<ExceptionResolution>>

    fun setResolutions(exceptionResolution: List<ExceptionResolution>): Completable

    fun updateResolutionSelect(exceptionResolution: ExceptionResolution): Completable
}