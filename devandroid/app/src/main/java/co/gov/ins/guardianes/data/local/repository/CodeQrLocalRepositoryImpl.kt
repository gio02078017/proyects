package co.gov.ins.guardianes.data.local.repository

import co.gov.ins.guardianes.data.local.dao.CodeQrDao
import co.gov.ins.guardianes.data.local.mappers.fromDomain
import co.gov.ins.guardianes.data.local.mappers.fromEntity
import co.gov.ins.guardianes.domain.models.GenerateQrResponse
import co.gov.ins.guardianes.domain.models.LastSelfDiagnosis
import co.gov.ins.guardianes.domain.repository.CodeQrLocalRepository
import io.reactivex.Flowable
import io.reactivex.Single
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers

class CodeQrLocalRepositoryImpl (
        private val codeQrDao: CodeQrDao
) : CodeQrLocalRepository {

    override fun getCodeQr(): Flowable<GenerateQrResponse> =
            codeQrDao.getCodeQr().map {
                it.fromDomain()
            }

    override fun setCodeQr(codeQr: GenerateQrResponse) =
            codeQrDao.dropCodeQr()
                    .andThen(codeQrDao.insertCodeQr(codeQr.fromEntity()))
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeBy ()

    override fun getLastSelftDiagnosis(): Single<LastSelfDiagnosis> =
        codeQrDao.getLastSelfDiagnosis().flatMap {
            Single.just(it.last().fromDomain())
        }
}