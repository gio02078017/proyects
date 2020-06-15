package co.gov.ins.guardianes.data.local.entities

import androidx.room.Entity
import androidx.room.PrimaryKey
import androidx.room.TypeConverter
import androidx.room.TypeConverters
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken

@Entity(tableName = "symptomDiagnostic")
@TypeConverters(DiagnosticConverters::class)
data class DiagnosticEntity(
    @PrimaryKey
    val id: String,
    val text: String,
    val description: String,
    val value: String,
    val categories: List<CategoriesEntity>
)

@Entity(tableName = "symptomCategoriesItem")
@TypeConverters(CategoriesConverters::class)
data class CategoriesEntity(
    @PrimaryKey
    val id: Int,
    val text: String,
    val description: String,
    val image: String,
    val slug: String,
    val order: Int,
    val recommendations: List<RecommendationsEntity>
)

@Entity(tableName = "symptomRecommendationsItem")
data class RecommendationsEntity(
    val id: Int,
    val text: String,
    val description: String,
    val slug: String,
    val order: Int
)

class DiagnosticConverters {
    private val gson = Gson()

    @TypeConverter
    fun toSymptomCategoriesItem(data: String?): List<CategoriesEntity> {
        if (data == null) {
            return emptyList()
        }
        val listType = object : TypeToken<List<CategoriesEntity>>() {}.type
        return gson.fromJson(data, listType)
    }

    @TypeConverter
    fun fromSymptomCategoriesItem(items: List<CategoriesEntity>): String {
        return gson.toJson(items)
    }
}

class CategoriesConverters {
    private val gson = Gson()

    @TypeConverter
    fun toSymptomRecommendationsItem(data: String?): List<RecommendationsEntity> {
        if (data == null) {
            return emptyList()
        }
        val listType = object : TypeToken<List<RecommendationsEntity>>() {}.type
        return gson.fromJson(data, listType)
    }

    @TypeConverter
    fun fromSymptomRecommendationsItem(items: List<RecommendationsEntity>): String {
        return gson.toJson(items)
    }
}