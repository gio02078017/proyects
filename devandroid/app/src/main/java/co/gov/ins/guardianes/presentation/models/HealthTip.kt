package co.gov.ins.guardianes.presentation.models

import android.os.Parcelable
import kotlinx.android.parcel.Parcelize

@Parcelize
data class HealthTip(
    val id: String,
    val title: String,
    val document: String,
    val order: Int
) : Parcelable