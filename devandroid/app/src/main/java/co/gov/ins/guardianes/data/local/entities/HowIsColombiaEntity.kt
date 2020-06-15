package co.gov.ins.guardianes.data.local.entities

import androidx.room.Entity
import androidx.room.PrimaryKey


@Entity(tableName = "Colombia")
data class HowIsColombiaEntity(
    @PrimaryKey
    val id: String,
    val confirmedCases: Int,
    val confirmedCasesToday: Int,
    val recoveredPatients: Int,
    val deaths: Int,
    val usersSupporting: Int,
    val createAt: String = "",
    val updatedAt: String = ""
)