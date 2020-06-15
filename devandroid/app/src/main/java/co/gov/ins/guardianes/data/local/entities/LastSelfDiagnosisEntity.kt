package co.gov.ins.guardianes.data.local.entities

import androidx.room.Entity
import androidx.room.PrimaryKey

@Entity(tableName = "LastSelfDiagnosis")
data class LastSelfDiagnosisEntity(
    @PrimaryKey
    val id: String,
    val idUser: String,
    val idHousehold: String,
    val date: String,
    val diagnosis: String,
    val latitude: String,
    val longitude: String,
    val deviceId: String,
    val createdAt: String,
    val updateAt: String
)

