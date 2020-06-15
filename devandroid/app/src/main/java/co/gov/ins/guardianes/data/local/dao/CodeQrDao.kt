package co.gov.ins.guardianes.data.local.dao

import androidx.room.*
import co.gov.ins.guardianes.data.local.entities.CodeQrEntity
import co.gov.ins.guardianes.data.local.entities.LastSelfDiagnosisEntity
import io.reactivex.Completable
import io.reactivex.Flowable
import io.reactivex.Single

@Dao
interface CodeQrDao {

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun insertCodeQr(codeQr: CodeQrEntity): Completable

    @Query("SELECT * FROM CodeQr")
    fun getCodeQr(): Flowable<CodeQrEntity>

    @Query("DELETE FROM CodeQr")
    fun dropCodeQr(): Completable

    @Query("SELECT * FROM LastSelfDiagnosis")
    fun getLastSelfDiagnosis(): Single<List<LastSelfDiagnosisEntity>>
}