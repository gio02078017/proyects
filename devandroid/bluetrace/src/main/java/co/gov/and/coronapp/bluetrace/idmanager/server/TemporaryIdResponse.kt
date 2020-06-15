package co.gov.and.coronapp.bluetrace.idmanager.server

data class TemporaryIdResponse (
        val status: String,
        val refreshTime: Long,
        val tempIds: List<TemporaryId>
)