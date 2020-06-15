package co.gov.ins.guardianes.presentation.models

import android.os.Parcelable
import kotlinx.android.parcel.Parcelize

@Parcelize
data class Place(
    var name: String? = null,
    val city: String?,
    val state: String?,
    val latitude: Double,
    val longitude: Double,
    val address: String? = null
) : Parcelable