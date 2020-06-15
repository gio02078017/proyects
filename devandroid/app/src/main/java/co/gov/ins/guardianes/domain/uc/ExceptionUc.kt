package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.repository.ExceptionRepository

class ExceptionUc(
    private val exceptionRepository: ExceptionRepository
) {

    fun getListDecreto() = exceptionRepository.getDecreto()
    fun getListResolution() = exceptionRepository.getResolution()

}