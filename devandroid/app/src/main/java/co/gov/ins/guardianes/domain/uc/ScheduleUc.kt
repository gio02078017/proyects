package co.gov.ins.guardianes.domain.uc

import android.os.Looper
import co.gov.ins.guardianes.domain.models.Schedule
import co.gov.ins.guardianes.domain.repository.PreferencesUtilRepository
import co.gov.ins.guardianes.domain.repository.ScheduleLocalRepository
import co.gov.ins.guardianes.domain.repository.ScheduleRepository
import com.crashlytics.android.Crashlytics
import io.reactivex.Flowable
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import java.util.*

class ScheduleUc(
    private val scheduleRepository: ScheduleRepository,
    private val scheduleLocalRepository: ScheduleLocalRepository,
    private val preferencesUtilRepository: PreferencesUtilRepository
) {

    private var isRequest: Boolean = false
    private fun getIsRemote() = preferencesUtilRepository.getLastUpdateSchedule() < Date().time

    fun getLines(): Flowable<List<Schedule>> = run {
        scheduleLocalRepository.getSchedule().retryWhen { error ->
            error.map { throwable ->
                if (throwable.message == "Empty") {
                    updateDb()
                    Flowable.just(emptyList<Schedule>())
                } else throw throwable
            }
        }.flatMap {
            if (getIsRemote()) {
                updateDb()
            }
            Flowable.just(it)
        }
    }

    private fun updateDb() {
        if (!isRequest)
            GlobalScope.launch(context = Dispatchers.IO) {
                scheduleRepository.getLines()
                    .doOnSubscribe {
                        isRequest = true
                    }
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.from(Looper.getMainLooper(), true))
                    .map { list ->
                        scheduleLocalRepository.setSchedule(list).subscribeOn(Schedulers.io())
                            .subscribeBy(
                                onError = {
                                    Crashlytics.logException(it)
                                },
                                onComplete = { preferencesUtilRepository.setLastUpdateSchedule(Date().time) })
                        list
                    }.subscribeBy(onError = {})
            }
    }
}