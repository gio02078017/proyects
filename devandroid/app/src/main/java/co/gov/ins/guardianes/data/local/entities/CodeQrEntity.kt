package co.gov.ins.guardianes.data.local.entities

import androidx.room.Entity
import androidx.room.PrimaryKey
import androidx.room.TypeConverter
import androidx.room.TypeConverters
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken

@Entity(tableName = "CodeQr")
@TypeConverters(CodeQrConverters::class)
data class CodeQrEntity (
        @PrimaryKey
        val codeQr: String,
        val expirationDate: String?,
        val showAlert: Boolean,
        val alertMessage: String,
        val qrType: String,
        val qrMessage: List<String>,
        val validQr: Boolean,
        val validMessage: String,
        val existsCompany: Int
)

class CodeQrConverters {
        private val gson = Gson()

        @TypeConverter
        fun toQrMessage(data: String?): List<String> {
                if (data == null) {
                        return emptyList()
                }
                val listType = object : TypeToken<List<String>>() {}.type
                return gson.fromJson(data, listType)
        }

        @TypeConverter
        fun fromQrMessage(items: List<String>): String {
                return gson.toJson(items)
        }
}