package co.gov.ins.guardianes.data.remoto.api

import co.gov.ins.guardianes.domain.models.HowIsColombia
import io.reactivex.Single
import retrofit2.http.GET

interface ApiHowIsColombia {

    @GET("v1.0/publicinformation/statistics")
    fun getDataStatistics(): Single<HowIsColombia>
}