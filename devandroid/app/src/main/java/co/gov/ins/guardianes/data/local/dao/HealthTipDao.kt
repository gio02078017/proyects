package co.gov.ins.guardianes.data.local.dao

import androidx.room.Dao
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query
import co.gov.ins.guardianes.data.local.entities.HealthTipEntity
import io.reactivex.Completable
import io.reactivex.Flowable
import io.reactivex.Maybe

@Dao
interface HealthTipDao {

    @Query("SELECT * FROM Tip")
    fun getTis(): Flowable<List<HealthTipEntity>>

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun insertTips(healthTipEntity: List<HealthTipEntity>): Completable
}