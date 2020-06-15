package co.gov.ins.guardianes.data.local.dao

import androidx.room.*
import co.gov.ins.guardianes.data.local.entities.DecretoEntity
import co.gov.ins.guardianes.data.local.entities.ResolutionEntity
import io.reactivex.Completable
import io.reactivex.Flowable

@Dao
interface ResolutionDao {

    @Query("SELECT * FROM Resolution")
    fun getResolutions(): Flowable<List<ResolutionEntity>>

    @Update
    fun updateResolutionSelect(vararg resolutionEntity: ResolutionEntity): Completable

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun insertResolutions(resolutionEntity: List<ResolutionEntity>): Completable


}