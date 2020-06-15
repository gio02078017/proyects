package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.models.ParticipantRequest
import co.gov.ins.guardianes.domain.repository.AddParticipantRepository
import co.gov.ins.guardianes.domain.repository.ParticipantLocalRepository
import co.gov.ins.guardianes.domain.repository.TokenRepository
import co.gov.ins.guardianes.domain.repository.UserPreferences
import co.gov.ins.guardianes.util.ext.retryWithUpdatedTokenIfRequired
import io.reactivex.Completable
import io.reactivex.Single

class AddParticipantUc(
    private val addParticipantRepository: AddParticipantRepository,
    private val userPreferences: UserPreferences,
    private val participantLocalRepository: ParticipantLocalRepository,
    private val tokenRepository: TokenRepository
) {

    fun registerParticipant(
        relationship: String,
        firstName: String,
        lastName: String,
        countryCode: String,
        phoneNumber: String,
        documentNumber: String,
        documentType: String
    ): Completable = run {

        val participantRequest = ParticipantRequest(
            idUser = userPreferences.getUserId(),
            appToken = userPreferences.getToken()?.token ?: "",
            relationship = relationship,
            firstName = firstName,
            lastName = lastName,
            countryCode = countryCode,
            phoneNumber = phoneNumber,
            documentNumber = documentNumber,
            documentType = documentType
        )

        Single.defer {
            addParticipantRepository.registerParticipant(
                userPreferences.getAuthorization(),
                participantRequest
            )
        }.retryWithUpdatedTokenIfRequired(
            tokenRepository = tokenRepository,
            userPreferences = userPreferences
        ).flatMapCompletable {
            participantLocalRepository.setParticipant(it).subscribe()
            Completable.complete()
        }
    }
}

