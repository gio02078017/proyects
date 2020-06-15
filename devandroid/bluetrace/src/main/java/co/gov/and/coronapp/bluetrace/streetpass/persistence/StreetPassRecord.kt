package co.gov.and.coronapp.bluetrace.streetpass.persistence

import android.os.Parcelable
import androidx.room.ColumnInfo
import androidx.room.Entity
import androidx.room.PrimaryKey
import kotlinx.android.parcel.Parcelize

@Parcelize
@Entity(tableName = "record_table")
class StreetPassRecord constructor(
    @ColumnInfo(name = "v")
    var v: Int,

    @ColumnInfo(name = "msg")
    var msg: String,

    @ColumnInfo(name = "org")
    var org: String,

    @ColumnInfo(name = "modelP")
    val modelP: String,

    @ColumnInfo(name = "modelC")
    val modelC: String,

    @ColumnInfo(name = "rssi")
    val rssi: Int,

    @ColumnInfo(name = "txPower")
    val txPower: Int?,

    @ColumnInfo(name = "aux")
    val aux: String?

) : Parcelable {

    @PrimaryKey(autoGenerate = true)
    @ColumnInfo(name = "id")
    var id: Int = 0

    @ColumnInfo(name = "timestamp")
    var timestamp: Long = System.currentTimeMillis()

}
