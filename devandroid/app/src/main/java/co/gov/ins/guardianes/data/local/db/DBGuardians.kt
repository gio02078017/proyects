package co.gov.ins.guardianes.data.local.db

import android.content.Context
import androidx.room.Database
import androidx.room.Room
import androidx.room.RoomDatabase
import androidx.room.TypeConverters
import co.gov.ins.guardianes.data.local.dao.*
import co.gov.ins.guardianes.data.local.entities.*

@Database(
    version = 8,
    entities = [
        HealthTipEntity::class,
        ParticipantEntity::class,
        HowIsColombiaEntity::class,
        ScheduleEntity::class,
        HotLineEntity::class,
        DiagnosticEntity::class,
        QuestionEntity::class,
        RuleDiagnosticEntity::class,
        RuleQuestionEntity::class,
        LastSelfDiagnosisEntity::class,
        LastParticipantDiagnosisEntity::class,
        CodeQrEntity::class,
        DecretoEntity::class,
        ResolutionEntity::class
    ]
)
@TypeConverters(
    Converters::class,
    QuestionConverters::class,
    DiagnosticConverters::class,
    CategoriesConverters::class,
    CodeQrConverters::class
)
abstract class DBGuardians : RoomDatabase() {

    abstract fun healthTipDao(): HealthTipDao
    abstract fun participantsDao(): ParticipantsDao
    abstract fun howIsColombiaDao(): HowIsColombiaDao
    abstract fun schedulePhoneJoinDao(): ScheduleDao
    abstract fun diagnosticDao(): DiagnosticDao
    abstract fun questionDao(): QuestionDao
    abstract fun ruleDiagnosticDao(): RuleDiagnosticDao
    abstract fun ruleQuestionDao(): RuleQuestionDao
    abstract fun homeLoginDao(): HomeLoginDao
    abstract fun codeQrDao(): CodeQrDao
    abstract fun participantResultDao(): ParticipantResultDao
    abstract fun decretoDao(): DecretoDao
    abstract fun resolutionDao(): ResolutionDao

    companion object {

        private const val nameDB = "Guardians.db"

        @Volatile
        private var INSTANCE: DBGuardians? = null

        fun getInstance(context: Context): DBGuardians =
            INSTANCE ?: synchronized(this) {
                INSTANCE ?: buildDatabase(context).also { INSTANCE = it }
            }

        private fun buildDatabase(context: Context) =
            Room.databaseBuilder(
                context.applicationContext,
                DBGuardians::class.java, nameDB
            ).fallbackToDestructiveMigration()
                .build()
    }
}