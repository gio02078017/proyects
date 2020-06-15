package co.gov.ins.guardianes.data.local.dao

import androidx.room.*
import co.gov.ins.guardianes.data.local.entities.DecretoEntity
import io.reactivex.Completable
import io.reactivex.Flowable

@Dao
interface DecretoDao {

    @Query("SELECT * FROM Decreto")
    fun getDecretos(): Flowable<List<DecretoEntity>>

    @Update
    fun updateDecretoSelect(vararg decretoEntity: DecretoEntity): Completable

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun insertDecretos(decretoEntity: List<DecretoEntity>): Completable


}