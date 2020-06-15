package co.gov.and.coronapp.bluetrace.idmanager.server

data class TemporaryId (
        val startTime: Long,
        val tempID: String,
        val expiryTime: Long
)