package co.gov.ins.guardianes.data.local.dao

import androidx.room.Dao
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query
import co.gov.ins.guardianes.data.local.entities.DiagnosticEntity
import io.reactivex.Completable
import io.reactivex.Flowable

@Dao
interface DiagnosticDao {

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun insertDiagnostics(diagnostics: List<DiagnosticEntity>): Completable

    @Query("SELECT * FROM symptomDiagnostic")
    fun getDiagnostics(): Flowable<List<DiagnosticEntity>>

    @Query("SELECT * FROM symptomDiagnostic WHERE id ==:diagnosticId")
    fun getDiagnostic(diagnosticId: String): Flowable<DiagnosticEntity>

}