package co.gov.and.coronapp.bluetrace.streetpass.persistence

import android.content.Context

class StreetPassRecordStorage(val context: Context) {

    private val recordDao = StreetPassRecordDatabase.getDatabase(context).recordDao()

    suspend fun saveRecord(record: StreetPassRecord) {
        recordDao.insert(record)
    }

    fun nukeDb() {
        recordDao.nukeDb()
    }

    fun getAllRecords(): List<StreetPassRecord> {
        return recordDao.getCurrentRecords()
    }

    fun getRecordsToSend(nin: String): List<StreetPassRecord> {
        return recordDao.getRecordsToSend(nin)
    }

    fun getRecordsByAux(aux: String, startTime: Long): Int {
        return recordDao.getRecordsByAux(aux, startTime)
    }

    suspend fun purgeOldRecords(before: Long) {
        recordDao.purgeOldRecords(before)
    }
}
