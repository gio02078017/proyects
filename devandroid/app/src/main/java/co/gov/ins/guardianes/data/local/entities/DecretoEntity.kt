package co.gov.ins.guardianes.data.local.entities

import androidx.room.Entity
import androidx.room.PrimaryKey

@Entity(tableName = "Decreto")
data class DecretoEntity(
    @PrimaryKey
    var id: Int,
    val body: Int,
    val value: String,
    val isSelect: Boolean = false
)
