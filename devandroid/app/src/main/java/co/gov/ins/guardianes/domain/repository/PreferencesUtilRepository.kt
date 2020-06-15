package co.gov.ins.guardianes.domain.repository

interface PreferencesUtilRepository {

    fun getLastUpdateTip(): Long

    fun setLastUpdateTip(date: Long): Boolean

    fun getLastUpdateParticipants(): Long

    fun setLastUpdateParticipants(date: Long): Boolean

    fun getLastUpdateSchedule(): Long

    fun setLastUpdateSchedule(date: Long): Boolean

    fun getLastUpdateQuestion(): Long

    fun setLastUpdateQuestion(date: Long): Boolean
}