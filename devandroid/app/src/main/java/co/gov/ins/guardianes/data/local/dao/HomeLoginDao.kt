package co.gov.ins.guardianes.data.local.dao

import androidx.room.Dao
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query
import co.gov.ins.guardianes.data.local.entities.LastSelfDiagnosisEntity
import io.reactivex.Completable
import io.reactivex.Flowable

@Dao
interface HomeLoginDao {

    @Query("SELECT * FROM LastSelfDiagnosis")
    fun getLastSelfDiagnosis(): Flowable<List<LastSelfDiagnosisEntity>>

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun insertLastSelfDiagnosis(lastSelfDiagnosisEntity: LastSelfDiagnosisEntity): Completable
}