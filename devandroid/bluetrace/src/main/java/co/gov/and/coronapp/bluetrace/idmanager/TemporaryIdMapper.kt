package co.gov.and.coronapp.bluetrace.idmanager

import co.gov.and.coronapp.bluetrace.idmanager.persistence.TemporaryIdEntity
import co.gov.and.coronapp.bluetrace.idmanager.server.TemporaryId

fun TemporaryIdEntity.fromPersistence() = run {
    TemporaryID(
            startTime,
            tempID,
            expiryTime,
            updateToServer
    )
}

fun TemporaryID.toPersistence() = run {
    TemporaryIdEntity(
            startTime = startTime,
            tempID = tempID,
            expiryTime = expiryTime,
            updateToServer = updateToServer
    )
}

fun TemporaryId.fromServer() = run {
    TemporaryID(
            startTime,
            tempID,
            expiryTime,
            updateToServer = 0
    )
}

fun TemporaryID.toServer() = run {
    TemporaryId(
            startTime,
            tempID,
            expiryTime
    )
}