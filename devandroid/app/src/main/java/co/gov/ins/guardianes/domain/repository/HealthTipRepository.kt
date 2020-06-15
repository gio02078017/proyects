package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.HealthTip
import io.reactivex.Single

interface HealthTipRepository {

    fun getHealthTip(): Single<List<HealthTip>>
}