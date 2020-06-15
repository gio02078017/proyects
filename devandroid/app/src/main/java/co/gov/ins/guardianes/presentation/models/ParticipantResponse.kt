package co.gov.ins.guardianes.presentation.models

data class ParticipantResponse(
        val id: String,
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