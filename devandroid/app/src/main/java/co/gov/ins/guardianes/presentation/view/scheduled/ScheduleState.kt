package co.gov.ins.guardianes.presentation.view.scheduled

import co.gov.ins.guardianes.presentation.models.Schedule

sealed class ScheduleState {
    object Loading : ScheduleState()
    class Error(val msg: String?) : ScheduleState()
    class Success(val data: List<Schedule>) : ScheduleState()
}