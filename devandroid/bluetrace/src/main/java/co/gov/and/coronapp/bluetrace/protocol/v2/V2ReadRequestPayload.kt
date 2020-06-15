package co.gov.and.coronapp.bluetrace.protocol.v2

import com.google.gson.Gson
import com.google.gson.GsonBuilder
import co.gov.and.coronapp.bluetrace.streetpass.PeripheralDevice

//acting as peripheral
class V2ReadRequestPayload(
    val v: Int,
    val id: String,
    val o: String,
    peripheral: PeripheralDevice,
    val aux: String?
) {
    val mp = peripheral.modelP

    fun getPayload(): ByteArray {
        return gson.toJson(this).toByteArray(Charsets.UTF_8)
    }

    companion object {
        val gson: Gson = GsonBuilder()
            .disableHtmlEscaping().create()

        fun fromPayload(dataBytes: ByteArray): V2ReadRequestPayload {
            val dataString = String(dataBytes, Charsets.UTF_8)
            return gson.fromJson(dataString, V2ReadRequestPayload::class.java)
        }
    }
}
