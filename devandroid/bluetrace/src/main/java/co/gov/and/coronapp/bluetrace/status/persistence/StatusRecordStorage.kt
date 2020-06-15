package co.gov.and.coronapp.bluetrace.status.persistence

import android.content.Context
import co.gov.and.coronapp.bluetrace.streetpass.persistence.StreetPassRecordDatabase

class StatusRecordStorage(val context: Context) {

    private val statusDao = StreetPassRecordDatabase.getDatabase(context).statusDao()

    suspend fun saveRecord(record: StatusRecord) {
        statusDao.insert(record)
    }

    fun nukeDb() {
        statusDao.nukeDb()
    }

    fun getAllRecords(): List<StatusRecord> {
        return statusDao.getCurrentRecords()
    }

    suspend fun purgeOldRecords(before: Long) {
        statusDao.purgeOldRecords(before)
    }
}
