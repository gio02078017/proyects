package co.gov.ins.guardianes.presentation.models

import android.os.Parcelable
import kotlinx.android.parcel.Parcelize

@Parcelize
data class UserResponse(
    val id: String,
    var token: String? = "",
    val firstName: String,
    val lastName: String,
    val countryCode: String,
    val phoneNumber: String,
    var documentType: String,
    val documentNumber: String
) : Parcelable