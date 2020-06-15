package co.gov.ins.guardianes.presentation.view.survey

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.data.remoto.models.QuestionRequest
import co.gov.ins.guardianes.domain.uc.*
import co.gov.ins.guardianes.presentation.mappers.fromDomain
import co.gov.ins.guardianes.presentation.models.Answer
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers

class SymptomViewModel(
    private val symptomUc: SymptomUc,
    private val answerUc: AnswerUc,
    private val firebaseEventUc: FirebaseEventUc,
    private val userPreferencesUc: UserPreferencesUc,
    private val participantsUc: ParticipantsUc
) : BaseViewModel() {

    private val _symptomLiveData = MutableLiveData<SymptomState>()
    val symptomLiveData: LiveData<SymptomState>
        get() = _symptomLiveData

    fun getDataForm() {
        symptomUc.getDataQuestionsLocal()
            .doOnSubscribe {
                _symptomLiveData.postValue(SymptomState.Loading)
            }
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribeBy(
                onNext = { request ->
                    _symptomLiveData.value = SymptomState.Success(request.map {
                        it.fromDomain()
                    }.sortedBy { it.order })
                },
                onError = {
                    if (it.message != "Empty")
                        _symptomLiveData.value = SymptomState.Error(it.message)
                }
            ).addTo(disposeBag)
    }

    fun registerAnswer(
        idHousehold: String,
        date: String,
        question: ArrayList<QuestionRequest>,
        diagnosis: String,
        latitude: String,
        longitude: String
    ) {
        answerUc.registerAnswer(
            idHousehold,
            date,
            question,
            diagnosis,
            latitude,
            longitude
        ).subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribeBy(
                onComplete = {
                    _symptomLiveData.value = SymptomState.SuccessAnswers
                },
                onError = {
                    _symptomLiveData.value = SymptomState.Error(it.message)
                }
            ).addTo(disposeBag)
    }

    fun getRulesSymptoms(
        answers: List<Answer>,
        face: Int
    ) {
        _symptomLiveData.value =
            SymptomState.GetRulesRisk(symptomUc.getRulesSymptoms(answers, face))
    }

    fun createEvent(key: String) = firebaseEventUc.createEvent(key)

    fun getUser() {
        val user = userPreferencesUc.getUser()
        _symptomLiveData.value = SymptomState.SuccessName(user?.firstName ?: "")
    }

    fun getFamily(id: String) {
        participantsUc.getParticipantById(id).subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribeBy(
                onNext = {
                    _symptomLiveData.value = SymptomState.SuccessName(it.firstName)
                },
                onError = {
                    _symptomLiveData.value = SymptomState.Error(it.message)
                }
            ).addTo(disposeBag)
    }

    override fun onCleared() {
        super.onCleared()
        disposeBag.clear()
    }
}