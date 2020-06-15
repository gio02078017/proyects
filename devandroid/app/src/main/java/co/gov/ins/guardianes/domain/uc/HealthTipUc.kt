package co.gov.ins.guardianes.domain.uc

import android.os.Looper
import co.gov.ins.guardianes.domain.models.HealthTip
import co.gov.ins.guardianes.domain.repository.HealthTipLocalRepository
import co.gov.ins.guardianes.domain.repository.HealthTipRepository
import co.gov.ins.guardianes.domain.repository.PreferencesUtilRepository
import io.reactivex.Flowable
import io.reactivex.Single
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import java.util.*

class HealthTipUc(
    private val healthTipRepository: HealthTipRepository,
    private val healthTipLocalRepository: HealthTipLocalRepository,
    private val preferencesUtilRepository: PreferencesUtilRepository
) {
    private var isRequest: Boolean = false
    fun getHealthTip(): Flowable<List<HealthTip>> = run {
        isRequest = false
        healthTipLocalRepository.getHealthTipLocal()
            .retryWhen { error ->
                error.map { throwable ->
                    if (throwable.message == "Empty") {
                        updateDb()
                        Flowable.just(emptyList<HealthTip>())
                    } else throw throwable
                }
            }
            .flatMap {
                if (isRemote()) {
                    updateDb()
                }
                Flowable.just(it)
            }
    }

    private fun updateDb() = run {
        if (!isRequest)
            GlobalScope.launch(context = Dispatchers.IO) {
                healthTipRepository.getHealthTip()
                    .doOnSubscribe {
                        isRequest = true
                    }
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.from(Looper.getMainLooper(), true))
                    .flatMap {
                        healthTipLocalRepository.setHealthTipLocal(it).subscribeOn(Schedulers.io())
                            .subscribeBy()
                        preferencesUtilRepository.setLastUpdateTip(Date().time)
                        Single.just(it)
                    }.subscribeBy(onError = {})
            }
    }

    private fun isRemote() = preferencesUtilRepository.getLastUpdateTip() < Date().time
}
