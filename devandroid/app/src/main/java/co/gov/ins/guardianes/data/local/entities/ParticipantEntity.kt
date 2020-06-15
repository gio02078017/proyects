package co.gov.ins.guardianes.data.local.entities

import androidx.room.Entity
import androidx.room.PrimaryKey

@Entity(tableName = "Participant")
data class ParticipantEntity(
    @PrimaryKey
    var id: String,
    val appToken: String,
    var idUser:String,
    var relationship: String,
    var firstName: String,
    var lastName: String,
    var documentType: String,
    val documentNumber: String,
    val phoneNumber: String,
    val countryCode: String
)
