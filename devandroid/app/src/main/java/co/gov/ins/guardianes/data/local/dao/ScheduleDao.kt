package co.gov.ins.guardianes.data.local.dao

import androidx.room.Dao
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query
import co.gov.ins.guardianes.data.local.entities.ScheduleEntity
import io.reactivex.Completable
import io.reactivex.Flowable


@Dao
interface ScheduleDao {

    @Query("SELECT * FROM schedule")
    fun getSchedule(): Flowable<List<ScheduleEntity>>

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun insertSchedules(schedules: List<ScheduleEntity>): Completable
}