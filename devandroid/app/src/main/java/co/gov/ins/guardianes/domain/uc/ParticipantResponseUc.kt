package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.models.LastParticipantDiagnosis
import co.gov.ins.guardianes.domain.repository.ParticipantResultLocalRepository
import co.gov.ins.guardianes.domain.repository.ParticipantResultRepository
import co.gov.ins.guardianes.domain.repository.TokenRepository
import co.gov.ins.guardianes.domain.repository.UserPreferences
import co.gov.ins.guardianes.util.ext.retryWithUpdatedTokenIfRequired
import com.crashlytics.android.Crashlytics
import io.reactivex.Flowable
import io.reactivex.Single
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch

class ParticipantResponseUc(
    private val participantResultRepository: ParticipantResultRepository,
    private val participantResultLocalRepository: ParticipantResultLocalRepository,
    private val userPreferences: UserPreferences,
    private val tokenRepository: TokenRepository
) {

    private var isRequest: Boolean = false
    fun getDataLocal() = run {
        isRequest = false
        participantResultLocalRepository.getLastParticipantDiagnosis().retryWhen { error ->
            error.map { throwable ->
                if (throwable.message == "Empty") {
                    callService()
                    Flowable.just(emptyList<LastParticipantDiagnosis>())
                } else throw throwable
            }
        }.map {
            callService()
            it
        }
    }

    private fun callService() {
        if (!isRequest)
            GlobalScope.launch(context = Dispatchers.IO) {
                updateDb().doOnSubscribe {
                    isRequest = true
                }.subscribeBy(
                    onSuccess = {
                        participantResultLocalRepository.setLastParticipantDiagnosis(it)
                            .subscribeOn(Schedulers.io())
                            .subscribeBy()
                    },
                    onError = {
                        Crashlytics.logException(it)
                    }
                )
            }
    }

    private fun updateDb() =
        Single.defer {
            participantResultRepository.queryParticipantResultDiagnosis(
                userPreferences.getAuthorization(),
                userPreferences.getUserId()
            )
        }.retryWithUpdatedTokenIfRequired(
            tokenRepository = tokenRepository,
            userPreferences = userPreferences
        )

}
