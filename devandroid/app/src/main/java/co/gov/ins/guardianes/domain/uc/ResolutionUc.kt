package co.gov.ins.guardianes.domain.uc

import co.gov.ins.guardianes.domain.models.ExceptionResolution
import co.gov.ins.guardianes.domain.repository.*
import com.crashlytics.android.Crashlytics
import io.reactivex.Flowable
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch

class ResolutionUc(
        private val exceptionRepository: ExceptionRepository,
        private val exceptionLocalRepository: ExceptionLocalRepository
) {


   fun getResolutionsDB(): Flowable<List<ExceptionResolution>> = run {
       exceptionLocalRepository.getResolutions().retryWhen { error ->
            error.map { throwable ->
                if (throwable.message == "Empty") {
                    updateResolutionsDb()
                    Flowable.just(emptyList<ExceptionResolution>())
                } else throw throwable
            }
        }
    }

    fun updateResolutionsDb() {
            GlobalScope.launch(context = Dispatchers.IO) {
                exceptionLocalRepository.setResolutions(exceptionRepository.getResolution()).subscribeBy(
                        onError = {
                            Crashlytics.logException(it)
                        },
                        onComplete = {}
                )

            }
    }

    fun updateResolutionSelect(
            id: Int,
            body: Int,
            value: String,
            isSelect: Boolean
    ) = run {
        val exceptionResolution = ExceptionResolution(
                id,
                body,
                value,
                isSelect
        )
        exceptionLocalRepository.updateResolutionSelect(exceptionResolution).subscribeOn(Schedulers.io())
                .subscribeBy()
    }

}