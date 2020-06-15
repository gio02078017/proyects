package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.ExceptionDecreto
import co.gov.ins.guardianes.domain.models.ExceptionResolution

interface ExceptionRepository {

    fun getDecreto(): List<ExceptionDecreto>
    fun getResolution(): List<ExceptionResolution>


}