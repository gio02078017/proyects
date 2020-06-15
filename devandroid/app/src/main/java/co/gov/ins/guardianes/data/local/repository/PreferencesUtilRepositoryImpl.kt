package co.gov.ins.guardianes.data.local.repository

import android.content.SharedPreferences
import co.gov.ins.guardianes.domain.repository.PreferencesUtilRepository
import co.gov.ins.guardianes.util.Constants
import java.util.*

class PreferencesUtilRepositoryImpl(private val sharedPreferences: SharedPreferences) :
    PreferencesUtilRepository {

    override fun getLastUpdateTip(): Long = run {
        sharedPreferences.getLong(Constants.Key.LAST_UPDATE_TIP, 0)
    }

    override fun setLastUpdateTip(date: Long): Boolean = run {
        val calendar = Calendar.getInstance().apply {
            timeInMillis = date
            set(Calendar.DATE, this.get(Calendar.DATE) + 1)
        }
        sharedPreferences.edit().putLong(Constants.Key.LAST_UPDATE_TIP, calendar.timeInMillis)
            .commit()
    }

    override fun getLastUpdateParticipants(): Long = run {
        sharedPreferences.getLong(Constants.Key.LAST_UPDATE_PARTICIPANTS, 0)
    }

    override fun setLastUpdateParticipants(date: Long): Boolean = run {
        val calendar = Calendar.getInstance().apply {
            timeInMillis = date
            set(Calendar.DATE, this.get(Calendar.DATE) + 1)
        }
        sharedPreferences.edit()
            .putLong(Constants.Key.LAST_UPDATE_PARTICIPANTS, calendar.timeInMillis).commit()
    }

    override fun getLastUpdateSchedule(): Long = run {
        sharedPreferences.getLong(Constants.Key.LAST_UPDATE_SCHEDULE, 0)
    }

    override fun setLastUpdateSchedule(date: Long): Boolean = run {
        val calendar = Calendar.getInstance().apply {
            timeInMillis = date
            set(Calendar.DATE, this.get(Calendar.DATE) + 1)
        }
        sharedPreferences.edit()
            .putLong(Constants.Key.LAST_UPDATE_SCHEDULE, calendar.timeInMillis).commit()
    }

    override fun getLastUpdateQuestion(): Long = run {
        sharedPreferences.getLong(Constants.Key.LAST_UPDATE_QUESTION, 0)
    }

    override fun setLastUpdateQuestion(date: Long): Boolean = run {
        val calendar = Calendar.getInstance().apply {
            timeInMillis = date
            set(Calendar.DATE, this.get(Calendar.DATE) + 1)
        }
        sharedPreferences.edit()
            .putLong(Constants.Key.LAST_UPDATE_QUESTION, calendar.timeInMillis).commit()
    }
}
