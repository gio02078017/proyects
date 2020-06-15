package co.gov.ins.guardianes.presentation.view.permissions_and_privacy

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.domain.uc.PermissionsUc
import co.gov.ins.guardianes.presentation.mappers.fromDomain
import co.gov.ins.guardianes.presentation.mappers.fromPresentation
import co.gov.ins.guardianes.presentation.models.PermissionPost
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers

class PermissionsViewModel(private val permissionsUc: PermissionsUc) : BaseViewModel() {

    private val permissionLiveData = MutableLiveData<PermissionsState>()
    val getPermissionData: LiveData<PermissionsState>
        get() = permissionLiveData

    fun getPermissions() {
        permissionsUc.getPermissions()
            .doOnSubscribe {
                permissionLiveData.postValue(PermissionsState.Loading)
            }
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribeBy(
                onSuccess = { request ->
                    permissionLiveData.value = PermissionsState.Success(request.map {
                        it.fromDomain()
                    })
                },
                onError = {
                    permissionLiveData.value = PermissionsState.Error(it.message)
                }
            ).addTo(disposeBag)
    }

    fun postPermissions(permissionPost: PermissionPost){
        permissionsUc.postPermissions(permissionPost.fromPresentation())
                .doOnSubscribe {
                    permissionLiveData.postValue(PermissionsState.Loading)
                }
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy(
                        onComplete = {
                            permissionLiveData.value = PermissionsState.SuccessComplete
                            permissionsUc.setPermissions(true)
                        },
                        onError = {
                            permissionLiveData.value = PermissionsState.Error(it.message)
                        }
                ).addTo(disposeBag)
    }


    override fun onCleared() {
        super.onCleared()
        disposeBag.clear()
    }
}