package co.gov.ins.guardianes.domain.models


data class Diagnostic(
    val id: String,
    val text: String,
    val description: String,
    val value: String,
    val categories: List<Categories>
)

data class Categories(
    val id: Int,
    val text: String,
    val description: String,
    val image: String,
    val slug: String,
    val order: Int,
    val recommendations: List<Recommendations>
)

data class Recommendations(
    val id: Int,
    val text: String,
    val description: String,
    val slug: String,
    val order: Int
)