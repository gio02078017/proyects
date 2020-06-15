package co.gov.ins.guardianes.domain.models


data class Question(
    val id: String,
    val title: String,
    val description: String,
    val field: String,
    val multiple: Boolean,
    val order: Int,
    val answers: List<Answer>
)

data class Answer(
    val id: String,
    val text: String,
    val value: String,
    val order: Int
)
