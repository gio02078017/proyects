package co.gov.ins.guardianes.data.local.entities

import androidx.room.Entity
import androidx.room.PrimaryKey

@Entity(tableName = "Tip")
data class HealthTipEntity(
    @PrimaryKey
    val id: String,
    val title: String,
    val documentUrl: String,
    val order: Int,
    val createDate: String,
    val updateDate: String
)