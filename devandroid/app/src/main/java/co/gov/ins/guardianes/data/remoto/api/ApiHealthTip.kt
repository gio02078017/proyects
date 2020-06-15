package co.gov.ins.guardianes.data.remoto.api

import co.gov.ins.guardianes.data.remoto.models.BaseRequest
import co.gov.ins.guardianes.data.remoto.models.HealthTip
import io.reactivex.Single
import retrofit2.http.GET

interface ApiHealthTip {

    @GET("v1.0/publicinformation/healthtips")
    fun getHealthTip(): Single<BaseRequest<List<HealthTip>>>
}