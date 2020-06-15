package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.models.ExceptionDecreto
import co.gov.ins.guardianes.domain.repository.*
import com.crashlytics.android.Crashlytics
import io.reactivex.Flowable
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch

class DecretoUc(
        private val exceptionRepository: ExceptionRepository,
        private val exceptionLocalRepository: ExceptionLocalRepository
) {


   fun getDecretosDB(): Flowable<List<ExceptionDecreto>> = run {
       exceptionLocalRepository.getDecretos().retryWhen { error ->
            error.map { throwable ->
                if (throwable.message == "Empty") {
                    updateDecretosDb()
                    Flowable.just(emptyList<ExceptionDecreto>())
                } else throw throwable
            }
        }.flatMap {
            Flowable.just(it)
        }
    }

    fun updateDecretosDb() {
            GlobalScope.launch(context = Dispatchers.IO) {
                exceptionLocalRepository.setDecretos(exceptionRepository.getDecreto()).subscribeBy(
                        onError = {
                            Crashlytics.logException(it)
                        },
                        onComplete = {}
                )

            }
    }

    fun updateDecretoSelect(
            id: Int,
            body: Int,
            value: String,
            isSelect: Boolean
    ) = run {
        val exceptionDecreto = ExceptionDecreto(
                id,
                body,
                value,
                isSelect
        )
        exceptionLocalRepository.updateDecretoSelect(exceptionDecreto).subscribeOn(Schedulers.io())
                .subscribeBy()
    }

}