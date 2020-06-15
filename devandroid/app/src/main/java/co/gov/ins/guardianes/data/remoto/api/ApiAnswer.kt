package co.gov.ins.guardianes.data.remoto.api

import co.gov.ins.guardianes.data.remoto.models.AnswerRequest
import co.gov.ins.guardianes.data.remoto.models.BaseRequest
import io.reactivex.Single
import retrofit2.http.Body
import retrofit2.http.Header
import retrofit2.http.POST

interface ApiAnswer {

    @POST("v2.0/question")
    fun registerAnswer(
        @Header("Authorization") tokenApp: String,
        @Body answerRequest: AnswerRequest
    ): Single<BaseRequest<Unit>>
}