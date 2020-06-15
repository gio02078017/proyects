package co.gov.ins.guardianes.data.local.entities

import androidx.room.Entity
import androidx.room.PrimaryKey

@Entity(tableName = "symptomRuleQuestion")
data class RuleQuestionEntity(
    @PrimaryKey
    val id: String,
    val idElement: String,
    val expression: String
)