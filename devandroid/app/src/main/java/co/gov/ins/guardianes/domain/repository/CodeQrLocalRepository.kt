package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.GenerateQrResponse
import co.gov.ins.guardianes.domain.models.LastSelfDiagnosis
import io.reactivex.Flowable
import io.reactivex.Single
import io.reactivex.disposables.Disposable

interface CodeQrLocalRepository {

    fun getCodeQr(): Flowable<GenerateQrResponse>

    fun setCodeQr(codeQr: GenerateQrResponse): Disposable

    fun getLastSelftDiagnosis(): Single<LastSelfDiagnosis>
}