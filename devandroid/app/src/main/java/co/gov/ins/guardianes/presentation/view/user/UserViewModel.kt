package co.gov.ins.guardianes.presentation.view.user

import android.text.InputType
import android.text.method.DigitsKeyListener
import android.view.View
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.domain.uc.UserUc
import co.gov.ins.guardianes.util.ext.*
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.Observable
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.functions.Function6
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers
import kotlinx.android.synthetic.main.activity_register.*
import kotlinx.android.synthetic.main.activity_register.editLastName
import kotlinx.android.synthetic.main.activity_register.editName
import kotlinx.android.synthetic.main.activity_register.editNumberDocument
import kotlinx.android.synthetic.main.activity_register.lyLastName
import kotlinx.android.synthetic.main.activity_register.lyPhone
import kotlinx.android.synthetic.main.activity_register.radioTerm
import kotlinx.android.synthetic.main.activity_register.spinnerTypeDocument
import kotlinx.android.synthetic.main.activity_register.tvLastName
import kotlinx.android.synthetic.main.activity_register.tvLastNameError
import kotlinx.android.synthetic.main.activity_register.tvName
import kotlinx.android.synthetic.main.activity_register.tvNameError
import kotlinx.android.synthetic.main.activity_register.tvPhone
import kotlinx.android.synthetic.main.activity_register.tvPhoneError
import kotlinx.android.synthetic.main.user.*


class UserViewModel(private val userUc: UserUc) : BaseViewModel() {

    private val userLiveData = MutableLiveData<UserState>()
    val getUserLiveDataState: LiveData<UserState>
        get() = userLiveData

    fun registerUser(
        firstName: String,
        lastName: String,
        countryCode: String,
        phoneNumber: String,
        documentNumber: String,
        documentType: String
    ) {
        userUc.registerUser(
            firstName,
            lastName,
            countryCode,
            phoneNumber,
            documentNumber,
            documentType
        ).doOnSubscribe {
            userLiveData.postValue(UserState.Loading)
        }.subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribeBy(
                onComplete = {
                    userLiveData.value = UserState.Success
                },
                onError = {
                    userLiveData.value = UserState.Error(it.message)
                }
            ).addTo(disposeBag)
    }

    fun validateRegister(registerActivity: RegisterActivity) {

        val nameTextObservable = registerActivity.editName.textWatcherObserver().share()
        val lastNameTextObservable = registerActivity.editLastName.textWatcherObserver().share()
        val documentTypeObservable =
            registerActivity.spinnerTypeDocument.spinnerObservable().share()
        val documentNumberTextObservable =
            registerActivity.editNumberDocument.textWatcherObserver().share()
        val phoneObservable = registerActivity.etEditPhone.textWatcherObserver().share()
        val termObservable = registerActivity.radioTerm.radioButtonObservable().share()

        val observerNameValidationResult = Observable.create<Boolean> { emitter ->
            nameTextObservable.subscribe {
                when {
                    it.firstOrNull()?.toString()?.isBlank() ?: false -> {
                        registerActivity.editName.setText(registerActivity.etEditPhone.text?.trim())
                    }
                    it.isNotEmpty() -> {
                        if (!it.isValidateName()) {
                            registerActivity.showError(
                                registerActivity.tvName,
                                registerActivity.editName,
                                registerActivity.lyName
                            )
                            registerActivity.tvNameError.visibility = View.VISIBLE
                        } else {
                            registerActivity.showNormal(
                                registerActivity.tvName,
                                registerActivity.editName,
                                registerActivity.lyName
                            )
                            registerActivity.tvNameError.visibility = View.GONE
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
                        registerActivity.editLastName.setText(registerActivity.editLastName.text?.trim())
                    }
                    it.isNotEmpty() -> {
                        if (!it.isValidateName()) {
                            registerActivity.showError(
                                registerActivity.tvLastName,
                                registerActivity.editLastName,
                                registerActivity.lyLastName
                            )
                            registerActivity.tvLastNameError.visibility = View.VISIBLE
                        } else {
                            registerActivity.showNormal(
                                registerActivity.tvLastName,
                                registerActivity.editLastName,
                                registerActivity.lyLastName
                            )
                            registerActivity.tvLastNameError.visibility = View.GONE
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
                emitter.onNext(true)
                when (it) {
                    0, 1, 2, 5 -> {
                        registerActivity.editNumberDocument.setText("")
                        registerActivity.editNumberDocument.keyListener =
                            DigitsKeyListener.getInstance(registerActivity.getString(R.string.digits_number))
                        registerActivity.editNumberDocument.inputType =
                            (InputType.TYPE_CLASS_NUMBER)
                    }
                    3, 4 -> {
                        registerActivity.editNumberDocument.setText("")
                        registerActivity.editNumberDocument.keyListener =
                            DigitsKeyListener.getInstance(registerActivity.getString(R.string.digits_alphanumeric))
                        registerActivity.editNumberDocument.inputType =
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
                        registerActivity.editNumberDocument.setText(registerActivity.editNumberDocument.text?.trim())
                    }
                    it.isValidateDocumentNumber()
                            && registerActivity.spinnerTypeDocument.selectedItemPosition != 3
                            && registerActivity.spinnerTypeDocument.selectedItemPosition != 4 -> {
                        emitter.onNext(true)
                    }

                    (registerActivity.spinnerTypeDocument.selectedItemPosition == 3
                            || registerActivity.spinnerTypeDocument.selectedItemPosition == 4)
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
                        registerActivity.etEditPhone.setText(registerActivity.etEditPhone.text?.trim())
                    }
                    it.isNotEmpty() -> {
                        if (it.length >= 10) {
                            if (it.length >= 11) {
                                registerActivity.showError(
                                    registerActivity.tvPhone,
                                    registerActivity.etEditPhone,
                                    registerActivity.lyPhone
                                )
                                registerActivity.tvPhoneError.visibility = View.VISIBLE
                                emitter.onNext(false)
                            } else {
                                registerActivity.showNormal(
                                    registerActivity.tvPhone,
                                    registerActivity.etEditPhone,
                                    registerActivity.lyPhone
                                )
                                registerActivity.tvPhoneError.visibility = View.GONE
                                emitter.onNext(true)
                            }
                        } else {
                            emitter.onNext(false)
                        }
                    }
                }
            }
        }

        val disposable = Observable
            .combineLatest(observerNameValidationResult, observerLastNameValidationResult,
                observerDocumentNumberValidationResult, observerPhoneValidationResult,
                termObservable, observerDocumentTypeValidationResult,
                Function6<Boolean, Boolean, Boolean, Boolean, Boolean, Boolean, Boolean> { t1, t2, t3, t4, t5, _ ->
                    return@Function6 t1 && t2 && t3 && t4 && t5
                })
            .subscribe {
                registerActivity.btnRegister.isEnabled = it
            }

        disposeBag.add(disposable)
    }

    override fun onCleared() {
        super.onCleared()
        disposeBag.clear()
    }
}