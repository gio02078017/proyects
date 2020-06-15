package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName

data class Schedule(
    @SerializedName("entity_name")
    val entityName: String,
    @SerializedName("hotlines")
    val hotLines: ArrayList<HotLine>
)

data class HotLine(
        val id: String,
        val name: String,
        val phone: String
)
