package co.gov.ins.guardianes.data.local.dao

import androidx.room.Dao
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query
import co.gov.ins.guardianes.data.local.entities.LastParticipantDiagnosisEntity
import io.reactivex.Completable
import io.reactivex.Flowable

@Dao
interface ParticipantResultDao {

    @Query("SELECT * FROM LastParticipantDiagnosis")
    fun getLastParticipantDiagnosis(): Flowable<List<LastParticipantDiagnosisEntity>>

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun insertLastParticipantDiagnosis(lastParticipantDiagnosisEntity: List<LastParticipantDiagnosisEntity>): Completable
}