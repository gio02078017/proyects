package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.HealthTip
import io.reactivex.Completable
import io.reactivex.Flowable
import io.reactivex.Maybe

interface HealthTipLocalRepository {

    fun getHealthTipLocal(): Flowable<List<HealthTip>>

    fun setHealthTipLocal(list: List<HealthTip>): Completable
}