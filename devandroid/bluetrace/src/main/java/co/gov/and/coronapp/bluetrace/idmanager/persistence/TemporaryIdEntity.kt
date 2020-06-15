package co.gov.and.coronapp.bluetrace.idmanager.persistence

import androidx.room.ColumnInfo
import androidx.room.Entity
import androidx.room.PrimaryKey

@Entity(tableName = "temporaryId_table")
data class TemporaryIdEntity (

    @PrimaryKey
    @ColumnInfo(name = "tempID")
    var tempID: String = "",

    @ColumnInfo(name = "startTime")
    var startTime: Long = 0,

    @ColumnInfo(name = "expiryTime")
    var expiryTime: Long = 0,

    @ColumnInfo(name = "updateToServer")
    var updateToServer: Int = 0
)