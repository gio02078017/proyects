package co.gov.ins.guardianes.data.local.dao

import androidx.room.Dao
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query
import co.gov.ins.guardianes.data.local.entities.ParticipantEntity
import io.reactivex.Completable
import io.reactivex.Flowable

@Dao
interface ParticipantsDao {

    @Query("SELECT * FROM Participant")
    fun getParticipants(): Flowable<List<ParticipantEntity>>

    @Query("SELECT * FROM Participant WHERE id ==:idParticipant")
    fun getParticipantsById(idParticipant: String): Flowable<ParticipantEntity>

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun insertParticipants(participants: List<ParticipantEntity>): Completable

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    fun insertParticipants(participant: ParticipantEntity): Completable
}