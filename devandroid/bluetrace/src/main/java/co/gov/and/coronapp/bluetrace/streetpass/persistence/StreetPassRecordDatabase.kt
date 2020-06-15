package co.gov.and.coronapp.bluetrace.streetpass.persistence

import android.content.Context
import androidx.room.Database
import androidx.room.Room
import androidx.room.RoomDatabase
import co.gov.and.coronapp.bluetrace.idmanager.persistence.TemporaryIdDao
import co.gov.and.coronapp.bluetrace.idmanager.persistence.TemporaryIdEntity
import co.gov.and.coronapp.bluetrace.status.persistence.StatusRecord
import co.gov.and.coronapp.bluetrace.status.persistence.StatusRecordDao


@Database(
    entities = [StreetPassRecord::class, StatusRecord::class, TemporaryIdEntity::class],
    version = 2,
    exportSchema = true
)
abstract class StreetPassRecordDatabase : RoomDatabase() {

    abstract fun recordDao(): StreetPassRecordDao
    abstract fun statusDao(): StatusRecordDao
    abstract fun temporaryIdDao(): TemporaryIdDao

    companion object {
        // Singleton prevents multiple instances of database opening at the
        // same time.
        @Volatile
        private var INSTANCE: StreetPassRecordDatabase? = null

        fun getDatabase(context: Context): StreetPassRecordDatabase {
            val tempInstance = INSTANCE
            if (tempInstance != null) {
                return tempInstance
            }
            synchronized(this) {
                val instance = Room.databaseBuilder(
                    context,
                    StreetPassRecordDatabase::class.java,
                    "record_database"
                )
                        .fallbackToDestructiveMigration()
                        .build()
                INSTANCE = instance
                return instance
            }
        }
    }
}
