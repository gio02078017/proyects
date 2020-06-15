package co.gov.ins.guardianes.data.local.mappers

import co.gov.ins.guardianes.data.local.entities.HowIsColombiaEntity
import co.gov.ins.guardianes.domain.models.HowIsColombia

fun HowIsColombiaEntity.from() =
    HowIsColombia(
        id = id,
        confirmedCases = confirmedCases,
        confirmedCasesToday = confirmedCasesToday,
        recoveredPatients = recoveredPatients,
        deaths = deaths,
        usersSupporting = usersSupporting,
        createAt = createAt,
        updatedAt = updatedAt
    )


fun HowIsColombia.from() =
    HowIsColombiaEntity(
        id = id,
        confirmedCases = confirmedCases,
        confirmedCasesToday = confirmedCasesToday,
        recoveredPatients = recoveredPatients,
        deaths = deaths,
        usersSupporting = usersSupporting,
        createAt = createAt,
        updatedAt = updatedAt
    )


