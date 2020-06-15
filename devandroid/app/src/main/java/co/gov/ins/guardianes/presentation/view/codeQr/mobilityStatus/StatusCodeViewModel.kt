package co.gov.ins.guardianes.presentation.view.codeQr.mobilityStatus

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.domain.uc.StatusCodeUc
import co.gov.ins.guardianes.presentation.mappers.fromPresentation
import co.gov.ins.guardianes.util.Constants.QrType.RED
import co.gov.ins.guardianes.util.ext.stringToDate
import co.gov.ins.guardianes.util.ext.to24HoursPassed
import co.gov.ins.guardianes.view.base.BaseViewModel
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.rxkotlin.addTo
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers
import java.util.*

class StatusCodeViewModel(
        private val statusCodeUc: StatusCodeUc
) : BaseViewModel() {

    private val _statusQrMutableLiveData = MutableLiveData<StatusCodeQrState>()
    val statusQrLiveData: LiveData<StatusCodeQrState>
        get() = _statusQrMutableLiveData

    fun getUser() = statusCodeUc.getUser()

    fun queryLastSelfDiagnosis() {
        statusCodeUc.queryLastSelfDiagnosis()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy(
                        onSuccess = {
                            val isShowAlert = it.date.to24HoursPassed()
                            _statusQrMutableLiveData.value = StatusCodeQrState.IsShowAlertDialig(isShowAlert)
                        },
                        onError = {
                            _statusQrMutableLiveData.value = StatusCodeQrState.IsShowAlertDialig(false)
                        }
                ).addTo(disposeBag)
    }

    fun getInformationQr() {
        statusCodeUc.getInformationQr()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeBy(
                        onNext = {
                            val dataQr = it.fromPresentation()
                            if (dataQr.validQr || dataQr.qrType == RED) {
                                if (it.expirationDate != null) {
                                    dataQr.expirationDate = formatDate(it.expirationDate.stringToDate())
                                }
                                _statusQrMutableLiveData.value = StatusCodeQrState.InformationQr(dataQr)
                            } else {
                                _statusQrMutableLiveData.value = StatusCodeQrState.QrInvalid(dataQr)
                            }
                        },
                        onError = {

                        }
                ).addTo(disposeBag)
    }

    override fun onCleared() {
        super.onCleared()
        disposeBag.clear()
    }

    private fun formatDate(date: Calendar) =
            "${date.get(Calendar.DAY_OF_MONTH)}/${date.get(Calendar.MONTH) + 1}/${date.get(Calendar.YEAR)} " +
                    "${date.get(Calendar.HOUR_OF_DAY)}:${String.format("%02d", date.get(Calendar.MINUTE))}"
}