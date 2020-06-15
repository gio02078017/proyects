package co.gov.ins.guardianes.data.local.dao

import androidx.room.Dao
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query
import co.gov.ins.guardianes.data.local.entities.RuleDiagnosticEntity
import io.reactivex.Completable
import io.reactivex.Flowable

@Dao
interface RuleDiagnosticDao {

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun insertRuleDiagnostics(ruleDiagnostics: List<RuleDiagnosticEntity>): Completable

    @Query("SELECT * FROM symptomRuleDiagnostic")
    fun getRuleDiagnostics(): Flowable<List<RuleDiagnosticEntity>>

}