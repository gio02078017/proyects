package co.gov.ins.guardianes.data.remoto.models

import com.google.gson.annotations.SerializedName

data class Diagnostic(
    val id: String,
    @SerializedName("texto")
    val text: String,
    @SerializedName("descripcion")
    val description: String,
    @SerializedName("valor")
    val value: String,
    @SerializedName("categorias")
    val categories: List<Categories>
)

data class Categories(
    val id: Int,
    @SerializedName("texto")
    val text: String,
    @SerializedName("descripcion")
    val description: String,
    @SerializedName("imagen")
    val image: String,
    val slug: String,
    @SerializedName("orden")
    val order: Int,
    @SerializedName("recomendaciones")
    val recommendations: List<Recommendations>
)

data class Recommendations(
    val id: Int,
    @SerializedName("texto")
    val text: String,
    @SerializedName("descripcion")
    val description: String,
    val slug: String,
    @SerializedName("orden")
    val order: Int
)