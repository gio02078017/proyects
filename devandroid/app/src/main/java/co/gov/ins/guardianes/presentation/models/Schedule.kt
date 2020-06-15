package co.gov.ins.guardianes.presentation.models

import co.gov.ins.guardianes.data.remoto.models.HotLine


data class Schedule(
    val entityName: String,
    val hotLines: List<HotLine>
)