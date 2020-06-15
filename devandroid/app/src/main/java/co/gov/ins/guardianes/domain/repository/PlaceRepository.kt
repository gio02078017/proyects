package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.Place

interface PlaceRepository {

    fun getPlace(): List<Place>
}