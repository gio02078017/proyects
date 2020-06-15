package co.gov.ins.guardianes.data.remoto.api

import co.gov.ins.guardianes.data.remoto.models.FormsResponse
import io.reactivex.Single
import retrofit2.http.GET
import retrofit2.http.Header

interface ApiSymptom {

    @GET("v2.0/form/full")
    fun getQuestions(@Header("Authorization") authorization: String): Single<FormsResponse>
}