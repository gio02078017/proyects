package co.gov.ins.guardianes.data.remoto.repository

import co.gov.ins.guardianes.data.remoto.api.ApiCheckSms
import co.gov.ins.guardianes.data.remoto.mappers.fromDomain
import co.gov.ins.guardianes.data.remoto.models.SendSmsResponse
import co.gov.ins.guardianes.data.remoto.models.VerifyUserResponse
import co.gov.ins.guardianes.domain.models.SendSms
import co.gov.ins.guardianes.domain.repository.CheckSmsRepository
import io.reactivex.Single
import co.gov.ins.guardianes.domain.models.SendSmsResponse as Domain
import co.gov.ins.guardianes.domain.models.VerifyUserResponse as DomainUser

class CheckSmsRepositoryImpl(private val apiCheckSms: ApiCheckSms) : CheckSmsRepository {

    override fun requestSms(infoSms: SendSms): Single<Domain> =
        apiCheckSms.requestSms(infoSms).flatMap { request ->
            Single.just(request.fromDomain())
        }

    override fun verifySms(dataSms: SendSms) =
        apiCheckSms
            .verifySms(dataSms).flatMap { request ->
                    if (request.success) {
                        Single.just(request.fromDomain())
                    } else {
                        Single.error(Throwable(request.message))
                    }
            }

    fun SendSmsResponse.fromDomain() =
        Domain(
            verificationId, responseCode
        )

    fun VerifyUserResponse.fromDomain() = DomainUser(
        responseCode, user.fromDomain(), token, refreshToken
    )
}