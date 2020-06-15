package co.gov.ins.guardianes.domain.repository.howIsColombia

import co.gov.ins.guardianes.domain.models.HowIsColombia
import io.reactivex.Completable
import io.reactivex.Flowable
import io.reactivex.Single

interface HowIsColombiaLocalRepository {

    fun getDataLocal(): Flowable<HowIsColombia>

    fun insertDataLocal(howIsColombia: HowIsColombia): Completable
}