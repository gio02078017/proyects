package co.gov.and.coronapp.bluetrace.streetpass.server

import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.Header
import retrofit2.http.POST

interface StreetPassApi {

    @POST("History/")
    fun sendRecords(
            @Header("Authorization") authorization: String,
            @Body request: StreetPassRequest
    ): Call<StreetPassResponse?>
}