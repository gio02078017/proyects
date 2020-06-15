package co.gov.and.coronapp.bluetrace.idmanager.persistence

import androidx.room.*
import androidx.sqlite.db.SupportSQLiteQuery

@Dao
interface TemporaryIdDao {

    @Query("SELECT * from temporaryId_table ORDER BY startTime ASC")
    fun getCurrentTemporaryIds(): List<TemporaryIdEntity>

    @Query("SELECT * from temporaryId_table WHERE updateToServer = 1 ORDER BY startTime ASC")
    fun getTemporaryIdsToUpdate(): List<TemporaryIdEntity>

    @Query("SELECT * from temporaryId_table WHERE startTime < :now AND expiryTime > :now")
    fun getValidTemporaryId(now: Long): List<TemporaryIdEntity>

    @Query("DELETE FROM temporaryId_table")
    fun nukeDb()

    @Query("DELETE FROM temporaryId_table WHERE startTime < :before")
    suspend fun purgeOldTemporaryIds(before: Long)

    @RawQuery
    fun getTemporaryIdsViaQuery(query: SupportSQLiteQuery): List<TemporaryIdEntity>

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    suspend fun insertAll(temporaryIds: List<TemporaryIdEntity>)
}