package co.gov.ins.guardianes.presentation.view.place

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.viewModelScope
import co.gov.ins.guardianes.domain.uc.PlaceUc
import co.gov.ins.guardianes.presentation.models.Place
import co.gov.ins.guardianes.view.base.BaseViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import co.gov.ins.guardianes.domain.models.Place as Domain

class PlaceViewModel(private val placeUc: PlaceUc) : BaseViewModel() {

    private val placeLiveData = MutableLiveData<List<Place>>()
    val getPlaceLiveData: LiveData<List<Place>>
        get() = placeLiveData

    fun launchDataLoad() {
        viewModelScope.launch {
            withContext(Dispatchers.IO) {
                val listPLace = placeUc.getPlace()
                delay(1000L)
                placeLiveData.postValue(listPLace.map {
                    it.fromPresentation()
                })
            }
        }
    }

    private fun Domain.fromPresentation() =
        Place(
            name, city, state, latitude, longitude, address
        )
}