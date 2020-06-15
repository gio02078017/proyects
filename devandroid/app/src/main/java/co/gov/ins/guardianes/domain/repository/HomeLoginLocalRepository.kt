package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.LastSelfDiagnosis
import io.reactivex.Completable
import io.reactivex.Flowable

interface HomeLoginLocalRepository {

    fun getLastSelfDiagnosis(): Flowable<LastSelfDiagnosis>

    fun setLastSelfDiagnosis(lastSelfDiagnosis: LastSelfDiagnosis): Completable
}