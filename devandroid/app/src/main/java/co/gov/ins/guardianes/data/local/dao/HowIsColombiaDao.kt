package co.gov.ins.guardianes.data.local.dao

import androidx.room.Dao
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query
import co.gov.ins.guardianes.data.local.entities.HowIsColombiaEntity
import io.reactivex.Completable
import io.reactivex.Flowable

@Dao
interface HowIsColombiaDao {

    @Query("SELECT * FROM Colombia")
    fun getDataColombia(): Flowable<HowIsColombiaEntity?>

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun insertDataColombia(howIsColombiaEntity: HowIsColombiaEntity): Completable
}