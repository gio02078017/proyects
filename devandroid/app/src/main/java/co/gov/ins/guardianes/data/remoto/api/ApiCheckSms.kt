package co.gov.ins.guardianes.data.remoto.api

import co.gov.ins.guardianes.data.remoto.models.SendSmsResponse
import co.gov.ins.guardianes.data.remoto.models.VerifyUserResponse
import co.gov.ins.guardianes.domain.models.SendSms
import io.reactivex.Single
import retrofit2.http.Body
import retrofit2.http.POST


interface ApiCheckSms {

    @POST("v1.0/sms/send")
    fun requestSms(@Body infoSms: SendSms): Single<SendSmsResponse>

    @POST("v2.0/sms/verify")
    fun verifySms(@Body dataSms: SendSms): Single<VerifyUserResponse>
}