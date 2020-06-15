package co.gov.ins.guardianes.presentation.view.add_participant

import android.text.InputType
import android.text.method.DigitsKeyListener
import android.view.View
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.domain.uc.AddParticipantUc
import co.gov.ins.guardianes.domain.uc.FirebaseEventUc
import co.gov.ins.guardianes.util.ext.*
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.Observable
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.functions.Function7
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers
import kotlinx.android.synthetic.main.user.*

class AddParticipantViewModel(
        private val addParticipantUc: AddParticipantUc,
        private val firebaseEventUc: FirebaseEventUc
) : BaseViewModel() {

    private val participantLiveData = MutableLiveData<AddParticipantState>()
    val getParticipantLiveDataState: LiveData<AddParticipantState>
        get() = participantLiveData

    fun registerParticipant(
            relationship: String,
            firstName: String,
            lastName: String,
            countryCode: String,
            phoneNumber: String,
            documentNumber: String,
            documentType: String
    ) {
        addParticipantUc.registerParticipant(
                relationship,
                firstName,
                lastName,
                countryCode,
                phoneNumber,
                documentNumber,
                documentType
        ).doOnSubscribe {
            participantLiveData.postValue(AddParticipantState.Loading)
        }.subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy(
                        onComplete = {
                            participantLiveData.value = AddParticipantState.Success
                        },
                        onError = {
                            participantLiveData.value = AddParticipantState.Error(it.message)
                        }
                ).addTo(disposeBag)
    }

    fun validateParticipant(addParticipantActivity: AddParticipantActivity) {

        val nameTextObservable = addParticipantActivity.editName.textWatcherObserver().share()
        val lastNameTextObservable =
                addParticipantActivity.editLastName.textWatcherObserver().share()
        val documentTypeObservable =
                addParticipantActivity.spinnerTypeDocument.spinnerObservable().share()

        val parentTextObservable = addParticipantActivity.editParent.textWatcherObserver().share()

        val documentNumberTextObservable =
                addParticipantActivity.editNumberDocument.textWatcherObserver().share()
        val phoneObservable = addParticipantActivity.editPhone.textWatcherObserver().share()
        val termObservable = addParticipantActivity.radioTerm.radioButtonObservable().share()

        val observerParentResult = Observable.create<Boolean> { emitter ->
            parentTextObservable.subscribe {
                when {
                    it.firstOrNull()?.toString()?.isBlank() ?: false -> {
                        addParticipantActivity.editParent.setText(addParticipantActivity.editParent.text?.trim())
                    }
                    it.isNotEmpty() -> {

                        participantLiveData.value = AddParticipantState.ChangeListeners
                        emitter.onNext(it.isValidateName())
                    }
                    else -> {
                        emitter.onNext(false)
                    }
                }
            }
        }

        val observerNameValidationResult = Observable.create<Boolean> { emitter ->
            nameTextObservable.subscribe {
                when {
                    it.firstOrNull()?.toString()?.isBlank() ?: false -> {
                        addParticipantActivity.editName.setText(addParticipantActivity.editName.text?.trim())
                    }
                    it.isNotEmpty() -> {
                        if (!it.isValidateName()) {
                            addParticipantActivity.showError(
                                    addParticipantActivity.tvName,
                                    addParticipantActivity.editName,
                                    addParticipantActivity.lyName
                            )
                            addParticipantActivity.tvNameError.visibility = View.VISIBLE
                        } else {
                            addParticipantActivity.showNormal(
                                    addParticipantActivity.tvName,
                                    addParticipantActivity.editName,
                                    addParticipantActivity.lyName
                            )
                            addParticipantActivity.tvNameError.visibility = View.GONE
                        }
                        emitter.onNext(it.isValidateName())
                    }
                    else -> {
                        emitter.onNext(false)
                    }
                }
            }
        }

        val observerLastNameValidationResult = Observable.create<Boolean> { emitter ->
            lastNameTextObservable.subscribe {
                when {
                    it.firstOrNull()?.toString()?.isBlank() ?: false -> {
                        addParticipantActivity.editLastName.setText(addParticipantActivity.editLastName.text?.trim())
                    }
                    it.isNotEmpty() -> {
                        if (!it.isValidateName()) {
                            addParticipantActivity.showError(
                                    addParticipantActivity.tvLastName,
                                    addParticipantActivity.editLastName,
                                    addParticipantActivity.lyLastName
                            )
                            addParticipantActivity.tvLastNameError.visibility = View.VISIBLE
                        } else {
                            addParticipantActivity.showNormal(
                                    addParticipantActivity.tvLastName,
                                    addParticipantActivity.editLastName,
                                    addParticipantActivity.lyLastName
                            )
                            addParticipantActivity.tvLastNameError.visibility = View.GONE
                        }
                        emitter.onNext(it.isValidateName())
                    }

                    else -> {
                        emitter.onNext(false)
                    }
                }
            }
        }

        val observerDocumentTypeValidationResult = Observable.create<Boolean> { emitter ->
            documentTypeObservable.subscribe {
                participantLiveData.value = AddParticipantState.ChangeListeners
                emitter.onNext(true)
                when (it) {
                    0, 1, 2, 5 -> {
                        addParticipantActivity.editNumberDocument.setText("")
                        addParticipantActivity.editNumberDocument.keyListener =
                                DigitsKeyListener.getInstance(addParticipantActivity.getString(R.string.digits_number))
                        addParticipantActivity.editNumberDocument.inputType =
                                (InputType.TYPE_CLASS_NUMBER)
                    }
                    3, 4 -> {
                        addParticipantActivity.editNumberDocument.setText("")
                        addParticipantActivity.editNumberDocument.keyListener =
                                DigitsKeyListener.getInstance(addParticipantActivity.getString(R.string.digits_alphanumeric))
                        addParticipantActivity.editNumberDocument.inputType =
                                (InputType.TYPE_CLASS_TEXT)
                    }
                    else -> {
                        emitter.onNext(false)
                    }
                }
            }
        }

        val observerDocumentNumberValidationResult = Observable.create<Boolean> { emitter ->
            documentNumberTextObservable.subscribe {
                when {
                    it.firstOrNull()?.toString()?.isBlank() ?: false -> {
                        addParticipantActivity.editNumberDocument.setText(addParticipantActivity.editNumberDocument.text?.trim())
                    }
                    it.isValidateDocumentNumber()
                            && addParticipantActivity.spinnerTypeDocument.selectedItemPosition != 3
                            && addParticipantActivity.spinnerTypeDocument.selectedItemPosition != 4 -> {
                        emitter.onNext(true)
                    }

                    (addParticipantActivity.spinnerTypeDocument.selectedItemPosition == 3
                            || addParticipantActivity.spinnerTypeDocument.selectedItemPosition == 4)
                            && it.isValidateDocumentNumberTwo() -> {
                        emitter.onNext(true)
                    }
                    else -> {
                        emitter.onNext(false)
                    }
                }
            }
        }

        val observerPhoneValidationResult = Observable.create<Boolean> { emitter ->
            phoneObservable.subscribe {
                when {
                    it.firstOrNull()?.toString()?.isBlank() ?: false -> {
                        addParticipantActivity.editPhone.setText(addParticipantActivity.editPhone.text?.trim())
                    }
                    it.isNotEmpty() -> {
                        if (it.length >= 10) {
                            if (it.length >= 11) {
                                addParticipantActivity.showError(
                                        addParticipantActivity.tvPhone,
                                        addParticipantActivity.editPhone,
                                        addParticipantActivity.lyPhone
                                )
                                addParticipantActivity.tvPhoneError.visibility = View.VISIBLE
                                emitter.onNext(false)
                            } else {
                                addParticipantActivity.showNormal(
                                        addParticipantActivity.tvPhone,
                                        addParticipantActivity.editPhone,
                                        addParticipantActivity.lyPhone
                                )
                                addParticipantActivity.tvPhoneError.visibility = View.GONE
                                emitter.onNext(true)
                            }
                        } else {
                            emitter.onNext(false)
                        }
                    }
                    else -> {
                        emitter.onNext(false)
                    }
                }
            }
        }

        val disposable = Observable
                .combineLatest(observerNameValidationResult, observerLastNameValidationResult,
                        observerDocumentNumberValidationResult, observerPhoneValidationResult,
                        termObservable, observerDocumentTypeValidationResult, observerParentResult,
                        Function7<Boolean, Boolean, Boolean, Boolean, Boolean, Boolean, Boolean, Boolean> { t1, t2, t3, t4, t5, _, t7 ->
                            return@Function7 t1 && t2 && t3 && t4 && t5 && t7
                        })
                .subscribe {
                    addParticipantActivity.btnAdd.isEnabled = it
                }

        disposeBag.add(disposable)
    }

    fun createEvent(key: String) {
        firebaseEventUc.createEvent(key)
    }

    override fun onCleared() {
        super.onCleared()
        disposeBag.clear()
    }
}