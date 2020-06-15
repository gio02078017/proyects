package co.gov.ins.guardianes.presentation.view.participant

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.domain.models.LastParticipantDiagnosis
import co.gov.ins.guardianes.domain.models.UserResponse
import co.gov.ins.guardianes.domain.uc.*
import co.gov.ins.guardianes.presentation.mappers.fromPresentation
import co.gov.ins.guardianes.presentation.models.LastDiagnostic
import co.gov.ins.guardianes.presentation.models.Participant
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers
import java.util.*

class ParticipantsViewModel(
    private val participantsUc: ParticipantsUc,
    private val participantsResponseUc: ParticipantResponseUc,
    private val userPreferencesUc: UserPreferencesUc,
    private val firebaseEventUc: FirebaseEventUc,
    private val homeLoginUc: HomeLoginUc
) : BaseViewModel() {

    private val participantsLiveData = MutableLiveData<ParticipantState>()
    val getParticipantsLiveDataState: LiveData<ParticipantState>
        get() = participantsLiveData

    fun getParticipants() {
        participantsUc.getDataRemote()
                .doOnSubscribe {
                    participantsLiveData.postValue(ParticipantState.Loading)
                }
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy(
                        onSuccess = { request ->
                            getUser()?.let { participant ->
                                val array: ArrayList<Participant> = request.map {
                                    it.fromPresentation()
                                } as ArrayList<Participant>
                                array.add(0, participant)
                                participantsLiveData.value = ParticipantState.Success(array)
                            }
                        },
                        onError = {
                            participantsLiveData.value = ParticipantState.Error(it.message)
                            getLocal()
                        }
                ).addTo(disposeBag)
    }

    private fun getLocal() {
        participantsUc.getDataLocal()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy(
                        onNext = { request ->
                            getUser()?.let { participant ->
                                val array: ArrayList<Participant> = request.map {
                                    it.fromPresentation()
                                } as ArrayList<Participant>
                                array.add(0, participant)
                                participantsLiveData.value = ParticipantState.Success(array)
                            }
                        },
                        onError =
                        {
                            if (it.message == "Empty") {
                                val array = ArrayList<Participant>()
                                getUser()?.let { participant ->
                                    array.add(0, participant)
                                }
                                participantsLiveData.value = ParticipantState.Success(array)
                            } else {
                                participantsLiveData.value = ParticipantState.Error(it.message)
                            }
                        }
                ).addTo(disposeBag)
    }

    fun queryLastSelfDiagnosis() {
        participantsResponseUc.getDataLocal()
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribeBy(
                onNext = {response ->
                    participantsLiveData.value = ParticipantState.SuccessDate(response.map {
                        it.fromPresentation()
                    })
                },
                onError = {
                    participantsLiveData.value = ParticipantState.Error(it.message)
                }
            ).addTo(disposeBag)
    }

    fun queryLastUserDiagnosis() {
        homeLoginUc.queryLastSelfDiagnosis()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy(
                        onNext = {
                            participantsLiveData.value = ParticipantState.SuccessUser(it.date)
                        },
                        onError = {

                        }
                ).addTo(disposeBag)
    }

    fun getUser() =
        participantsUc.getUserFromParticipant()?.fromParticipant()

    fun getMainUser() = userPreferencesUc.getUser()

    fun createEvent(key: String) {
        firebaseEventUc.createEvent(key)
    }

    private fun UserResponse.fromParticipant() = Participant(
        id, firstName, lastName, relationship = ""
    )

    private fun LastParticipantDiagnosis.fromPresentation() =
        LastDiagnostic(
            idHousehold,
            date
        )

    override fun onCleared() {
        super.onCleared()
        disposeBag
    }
}