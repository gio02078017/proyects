package co.gov.ins.guardianes.domain.repository.howIsColombia

import co.gov.ins.guardianes.domain.models.HowIsColombia
import io.reactivex.Single

interface HowIsColombiaRepository {
    fun getData(): Single<HowIsColombia>
}