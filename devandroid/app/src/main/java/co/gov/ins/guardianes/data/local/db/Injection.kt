package co.gov.ins.guardianes.data.local.db

import android.content.Context
import co.gov.ins.guardianes.data.local.dao.*

object Injection {

    fun tipDataSource(context: Context): HealthTipDao =
        DBGuardians.getInstance(context).healthTipDao()

    fun participantsDataSource(context: Context): ParticipantsDao =
        DBGuardians.getInstance(context).participantsDao()

    fun howIsColombiaDataSource(context: Context): HowIsColombiaDao =
        DBGuardians.getInstance(context).howIsColombiaDao()

    fun scheduleDataSource(context: Context): ScheduleDao =
        DBGuardians.getInstance(context).schedulePhoneJoinDao()

    fun diagnosticDataSource(context: Context): DiagnosticDao =
        DBGuardians.getInstance(context).diagnosticDao()

    fun questionDataSource(context: Context): QuestionDao =
        DBGuardians.getInstance(context).questionDao()

    fun ruleDiagnosticDataSource(context: Context): RuleDiagnosticDao =
        DBGuardians.getInstance(context).ruleDiagnosticDao()

    fun ruleQuestionDataSource(context: Context): RuleQuestionDao =
        DBGuardians.getInstance(context).ruleQuestionDao()

    fun homeLoginDataSource(context: Context): HomeLoginDao =
        DBGuardians.getInstance(context).homeLoginDao()

    fun participantResultDataSource(context: Context): ParticipantResultDao =
        DBGuardians.getInstance(context).participantResultDao()

    fun codeQrDataSource(context: Context): CodeQrDao =
            DBGuardians.getInstance(context).codeQrDao()

    fun decretoDataSource(context: Context): DecretoDao =
            DBGuardians.getInstance(context).decretoDao()

    fun resolutionDataSource(context: Context): ResolutionDao =
            DBGuardians.getInstance(context).resolutionDao()
}