package co.gov.ins.guardianes.data.remoto.api

import co.gov.ins.guardianes.data.remoto.models.BaseRequest
import co.gov.ins.guardianes.data.remoto.models.Schedule
import io.reactivex.Single
import retrofit2.http.GET

interface ApiSchedule {

    @GET("v1/PublicInformation/hotlines")
    fun getSchedules(): Single<BaseRequest<List<Schedule>>>
}