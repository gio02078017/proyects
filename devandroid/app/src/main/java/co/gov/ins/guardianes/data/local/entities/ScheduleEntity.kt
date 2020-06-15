package co.gov.ins.guardianes.data.local.entities

import androidx.room.Entity
import androidx.room.PrimaryKey
import androidx.room.TypeConverter
import androidx.room.TypeConverters
import com.google.gson.Gson

import com.google.gson.reflect.TypeToken

@Entity(tableName = "line")
data class HotLineEntity(
    @PrimaryKey
    val id: String,
    val name: String,
    val phone: String
)

@Entity(tableName = "schedule")
@TypeConverters(Converters::class)
data class ScheduleEntity(
    @PrimaryKey(autoGenerate = true)
    var id: Long = 0,
    val entityName: String,
    val hotLines: List<HotLineEntity>
)

class Converters {
    private val gson = Gson()

    @TypeConverter
    fun toHotLine(data: String?): List<HotLineEntity> {
        if (data == null) {
            return emptyList()
        }
        val listType = object : TypeToken<List<HotLineEntity>>() {}.type
        return gson.fromJson(data, listType)
    }

    @TypeConverter
    fun fromHotLine(movies: List<HotLineEntity>): String {
        return gson.toJson(movies)
    }
}