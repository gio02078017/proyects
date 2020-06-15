package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.FormsResponse
import io.reactivex.Single

interface SymptomRepository {
    fun getFormData(authorization: String): Single<FormsResponse>
}