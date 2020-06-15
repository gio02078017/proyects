package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.SendSms
import co.gov.ins.guardianes.domain.models.SendSmsResponse
import co.gov.ins.guardianes.domain.models.VerifyUserResponse
import io.reactivex.Single

interface CheckSmsRepository {

    fun requestSms(infoSms: SendSms): Single<SendSmsResponse>
    fun verifySms(dataSms: SendSms): Single<VerifyUserResponse>
}