package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.repository.PlaceRepository

class PlaceUc(private val placeRepository: PlaceRepository) {

    fun getPlace() = placeRepository.getPlace()
}