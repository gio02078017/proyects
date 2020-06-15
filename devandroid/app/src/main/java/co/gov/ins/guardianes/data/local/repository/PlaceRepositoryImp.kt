package co.gov.ins.guardianes.data.local.repository

import android.content.Context
import co.gov.ins.guardianes.data.local.models.Place
import co.gov.ins.guardianes.domain.repository.PlaceRepository
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import co.gov.ins.guardianes.domain.models.Place as Domain

class PlaceRepositoryImp(private val context: Context) : PlaceRepository {

    override fun getPlace(): List<Domain> {
        var jsonString = ""
        try {
            jsonString =
                context.assets.open("health_units.json").bufferedReader().use { it.readText() }
        } catch (ioException: Exception) {
            ioException.printStackTrace()
        }
        val listPersonType = object : TypeToken<List<Place>>() {}.type
        val list: ArrayList<Place> = Gson().fromJson(jsonString, listPersonType)
        return list.map {
            it.fromDomain()
        }
    }

    private fun Place.fromDomain() = Domain(
        name, city, state, latitude, longitude, address
    )

}