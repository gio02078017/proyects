package co.gov.ins.guardianes.domain.uc

import android.os.Looper
import android.util.Log
import co.gov.ins.guardianes.domain.models.Participant
import co.gov.ins.guardianes.domain.repository.*
import co.gov.ins.guardianes.util.ext.retryWithUpdatedTokenIfRequired
import com.crashlytics.android.Crashlytics
import io.reactivex.Flowable
import io.reactivex.Single
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import java.util.*

class ParticipantsUc(
    private val participantRepository: ParticipantRepository,
    private val userPreferences: UserPreferences,
    private val participantLocalRepository: ParticipantLocalRepository,
    private val preferencesUtilRepository: PreferencesUtilRepository,
    private val tokenRepository: TokenRepository
) {

    fun getDataLocal(): Flowable<List<Participant>> =
        participantLocalRepository.getParticipants().map {
            getDataRemote()
            it
        }

    fun getDataRemote() =
        participantRepository.getParticipants(
            userPreferences.getAuthorization(),
            userPreferences.getUserId()
        ).flatMap {
            participantLocalRepository.setParticipants(it)
                .subscribeOn(Schedulers.io())
                .subscribeBy()
            Single.just(it)
        }

    private var isRequest: Boolean = false
    fun getParticipants(): Flowable<List<Participant>> = run {
        isRequest = false
        participantLocalRepository.getParticipants()
            .retryWhen { error ->
                error.map { throwable ->
                    if (throwable.message == "Empty") {
                        callService()
                        throw throwable
                    } else throw throwable
                }
            }
    }

    private fun callService() {
        if (!isRequest)
            GlobalScope.launch(context = Dispatchers.IO) {
                updateDb().doOnSubscribe {
                    isRequest = true
                }.subscribeBy(
                    onError = {
                        Crashlytics.logException(it)
                    },
                    onSuccess = {
                        if (it.isNotEmpty())
                            insertDB(it)
                    }
                )
            }
    }

    private fun updateDb() =
        Single.defer {
            participantRepository.getParticipants(
                userPreferences.getAuthorization(),
                userPreferences.getUserId()
            )
        }.retryWithUpdatedTokenIfRequired(
            tokenRepository = tokenRepository,
            userPreferences = userPreferences
        )

    private fun insertDB(list: List<Participant>) =
        participantLocalRepository.setParticipants(list)
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.from(Looper.getMainLooper(), true))
            .subscribeBy(
                onComplete = {
                    preferencesUtilRepository.setLastUpdateParticipants(Date().time)
                },
                onError = {
                    Log.e("error ->>", "${it.message}")
                })

    fun getUserFromParticipant() = userPreferences.getUser()

    fun getParticipantById(id: String) =
        participantLocalRepository.getParticipantById(id)

}