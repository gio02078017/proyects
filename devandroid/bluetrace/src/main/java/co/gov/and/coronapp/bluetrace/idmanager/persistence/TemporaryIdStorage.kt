package co.gov.and.coronapp.bluetrace.idmanager.persistence

import android.content.Context
import co.gov.and.coronapp.bluetrace.streetpass.persistence.StreetPassRecordDatabase

class TemporaryIdStorage(val context: Context) {

    private val temporaryIdDao = StreetPassRecordDatabase.getDatabase(context).temporaryIdDao()

    suspend fun saveTemporaryIds(temporaryIds: List<TemporaryIdEntity>) {
        temporaryIdDao.insertAll(temporaryIds)
    }

    fun nukeDb() {
        temporaryIdDao.nukeDb()
    }

    fun getAllTemporaryIds(): List<TemporaryIdEntity> {
        return temporaryIdDao.getCurrentTemporaryIds()
    }

    fun getValidTemporaryId(now: Long): List<TemporaryIdEntity> {
        return temporaryIdDao.getValidTemporaryId(now)
    }

    fun getTemporaryIdsToUpdate(): List<TemporaryIdEntity> {
        return temporaryIdDao.getTemporaryIdsToUpdate()
    }

    suspend fun purgeOldTemporaryIds(before: Long) {
        temporaryIdDao.purgeOldTemporaryIds(before)
    }
}