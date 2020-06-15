package co.gov.ins.guardianes.domain.models

import co.gov.ins.guardianes.data.remoto.models.HotLine


data class Schedule(
    val entityName: String,
    val hotLines: List<HotLine>
)