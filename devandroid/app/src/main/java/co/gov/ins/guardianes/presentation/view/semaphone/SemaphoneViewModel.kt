package co.gov.ins.guardianes.presentation.view.semaphone

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.domain.uc.ParticipantsUc
import co.gov.ins.guardianes.domain.uc.UserPreferencesUc
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers

class SemaphoneViewModel(
    private val userPreferencesUc: UserPreferencesUc,
    private val participantsUc: ParticipantsUc
) : BaseViewModel() {

    private val nameUserLiveData = MutableLiveData<String>()
    val getNameUserLiveData: LiveData<String>
        get() = nameUserLiveData

    fun getUser() {
        val user = userPreferencesUc.getUser()
        nameUserLiveData.value = user?.firstName ?: ""
    }

    fun getFamily(id: String) {
        participantsUc.getParticipantById(id).subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribeBy(
                onNext = {
                    nameUserLiveData.value = it.firstName
                },
                onError = {

                }
            ).addTo(disposeBag)
    }

    override fun onCleared() {
        super.onCleared()
        disposeBag.clear()
    }
}