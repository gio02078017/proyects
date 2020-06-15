package co.gov.ins.guardianes.data.local.entities

import androidx.room.Entity
import androidx.room.PrimaryKey
import androidx.room.TypeConverter
import androidx.room.TypeConverters

import com.google.gson.Gson
import com.google.gson.reflect.TypeToken

@Entity(tableName = "symptomQuestion")
@TypeConverters(QuestionConverters::class)
data class QuestionEntity(
    @PrimaryKey
    val id: String,
    val title: String,
    val description: String,
    val field: String,
    val multiple: Boolean,
    val order: Int,
    val answer: List<Answer>
)

@Entity(tableName = "symptomQuestionItem")
data class Answer(
    @PrimaryKey
    val id: String,
    val text: String,
    val value: String,
    val order: Int
)

class QuestionConverters {
    private val gson = Gson()

    @TypeConverter
    fun toSympQuestionItem(data: String?): List<Answer> {
        if (data == null) {
            return emptyList()
        }
        val listType = object : TypeToken<List<Answer>>() {}.type
        return gson.fromJson(data, listType)
    }

    @TypeConverter
    fun fromSympQuestionItem(items: List<Answer>): String {
        return gson.toJson(items)
    }
}