package co.gov.ins.guardianes.data.remoto.api

import co.gov.ins.guardianes.data.remoto.models.VersionApp
import io.reactivex.Single
import retrofit2.http.GET

interface ApiMenuHome {

    @GET("app/android")
    fun getAppVersion(): Single<VersionApp>
}