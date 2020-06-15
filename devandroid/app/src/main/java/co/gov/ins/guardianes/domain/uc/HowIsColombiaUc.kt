package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.models.HowIsColombia
import co.gov.ins.guardianes.domain.repository.howIsColombia.HowIsColombiaLocalRepository
import co.gov.ins.guardianes.domain.repository.howIsColombia.HowIsColombiaRepository
import io.reactivex.Flowable
import io.reactivex.Single
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers

class HowIsColombiaUc(
    private val howIsColombiaRepository: HowIsColombiaRepository,
    private val howIsColombiaLocalRepository: HowIsColombiaLocalRepository
) {

    fun getDataLocal(): Flowable<HowIsColombia> =
        howIsColombiaLocalRepository.getDataLocal().map {
            getDataRemote()
            it
        }

    fun getDataRemote() =
        howIsColombiaRepository.getData().flatMap {
            howIsColombiaLocalRepository.insertDataLocal(it)
                .subscribeOn(Schedulers.io())
                .subscribeBy()
            Single.just(it)
        }
}