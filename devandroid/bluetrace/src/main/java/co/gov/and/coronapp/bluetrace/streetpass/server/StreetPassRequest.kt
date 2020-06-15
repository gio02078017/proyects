package co.gov.and.coronapp.bluetrace.streetpass.server

data class StreetPassRequest (
        val messages: List<StreetPassRecord>,
        val pin: String
)