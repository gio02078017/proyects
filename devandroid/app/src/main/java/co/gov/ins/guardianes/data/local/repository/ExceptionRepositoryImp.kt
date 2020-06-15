package co.gov.ins.guardianes.data.local.repository

import co.gov.ins.guardianes.data.local.models.listDecreto
import co.gov.ins.guardianes.data.local.models.listResolution
import co.gov.ins.guardianes.domain.models.ExceptionDecreto
import co.gov.ins.guardianes.domain.models.ExceptionResolution
import co.gov.ins.guardianes.domain.repository.ExceptionRepository
import co.gov.ins.guardianes.data.local.models.ExceptionDecreto as DataDecreto
import co.gov.ins.guardianes.data.local.models.ExceptionResolution as DataResolution

class ExceptionRepositoryImp() : ExceptionRepository {


    override fun getDecreto() =
        listDecreto.map {
            it.fromDomain()
        }

    private fun DataDecreto.fromDomain() = ExceptionDecreto(id, body, value, isSelect)

    override fun getResolution() =
        listResolution.map {
            it.fromDomain()
        }

    private fun DataResolution.fromDomain() = ExceptionResolution(id, body, value, isSelect)
}