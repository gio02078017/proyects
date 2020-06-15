package co.gov.and.coronapp.bluetrace.idmanager.server

import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.Header
import retrofit2.http.POST

interface TemporaryIdApi {

    @POST("TempId/generate/")
    fun getTemporaryIds(
            @Header("Authorization") authorization: String
    ): Call<TemporaryIdResponse?>
}