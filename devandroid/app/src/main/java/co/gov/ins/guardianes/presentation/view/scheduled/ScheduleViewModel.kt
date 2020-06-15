package co.gov.ins.guardianes.presentation.view.scheduled

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.domain.uc.ScheduleUc
import co.gov.ins.guardianes.presentation.mappers.fromSchedule
import co.gov.ins.guardianes.presentation.models.Schedule
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers

class ScheduleViewModel(private val scheduleUc: ScheduleUc) : BaseViewModel() {

    private val scheduleLiveData = MutableLiveData<ScheduleState>()
    val getScheduleData: LiveData<ScheduleState>
        get() = scheduleLiveData

    fun getSchedule() {
        scheduleUc.getLines()
            .doOnSubscribe {
                scheduleLiveData.postValue(ScheduleState.Loading)
            }
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribeBy(
                onNext = { request ->
                    val array: ArrayList<Schedule> = request.map {
                        it.fromSchedule()
                    } as ArrayList<Schedule>
                    scheduleLiveData.value = ScheduleState.Success(array)
                },
                onError = {
                    scheduleLiveData.value = ScheduleState.Error(it.message)
                }
            ).addTo(disposeBag)
    }

    override fun onCleared() {
        super.onCleared()
        disposeBag.clear()
    }
}