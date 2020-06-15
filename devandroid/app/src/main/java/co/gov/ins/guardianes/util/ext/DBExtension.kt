package co.gov.ins.guardianes.util.ext

import android.os.Looper
import co.gov.ins.guardianes.domain.models.FormsResponse
import co.gov.ins.guardianes.domain.repository.SymptomLocalRepository
import co.gov.ins.guardianes.domain.repository.SymptomRepository
import co.gov.ins.guardianes.domain.repository.TokenRepository
import co.gov.ins.guardianes.domain.repository.UserPreferences
import io.reactivex.Flowable
import io.reactivex.Single
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.functions.BiFunction
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch

private const val MAX_RETRIES = 2

fun <T> Flowable<T>.retrySymptomsEmpty(
    symptomRepository: SymptomRepository,
    tokenRepository: TokenRepository,
    userPreferences: UserPreferences,
    symptomLocalRepository: SymptomLocalRepository
): Flowable<T> =
    retryWhen { handler ->
        handler.zipWith(
            Flowable.range(1, MAX_RETRIES),
            BiFunction<Throwable, Int, Pair<Throwable, Int>> { error, count ->
                Pair(error, count)
            }.also {
                Single.defer {
                    symptomRepository.getFormData(userPreferences.getAuthorization())
                }.retryWithUpdatedTokenIfRequired(
                    tokenRepository,
                    userPreferences
                ).map {
                    saveDB(it, symptomLocalRepository)
                }.subscribeBy()
            }
        ).flatMap { pair ->
            val error = pair.first
            val count = pair.second
            (if (count <= MAX_RETRIES) {
                if (error.message == "Empty" || count == 1) {
                    Single.defer {
                        symptomRepository.getFormData(userPreferences.getAuthorization())
                    }.retryWithUpdatedTokenIfRequired(
                        tokenRepository,
                        userPreferences
                    ).map {
                        saveDB(it, symptomLocalRepository)
                        it
                    }.toFlowable()
                } else {
                    Flowable.error(error)
                }
            } else {
                Flowable.error(Throwable("Number of retries reached"))
            })
        }
    }

private fun saveDB(
    response: FormsResponse,
    symptomLocalRepository: SymptomLocalRepository
) = run {
    GlobalScope.launch(context = Dispatchers.IO) {
        symptomLocalRepository.insertRuleQuestionsLocal(response.rulesQuestion)
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.from(Looper.getMainLooper(), true))
            .andThen(
                symptomLocalRepository.insertRulesDiagnosticsLocal(response.rulesDiagnostics)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.from(Looper.getMainLooper(), true))
            )
            .andThen(
                symptomLocalRepository.insertQuestionsLocal(response.question)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.from(Looper.getMainLooper(), true))
            )
            .andThen(
                symptomLocalRepository.insertDiagnosticsLocal(response.diagnostics)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.from(Looper.getMainLooper(), true))
            )
            .subscribeBy()
    }
}


