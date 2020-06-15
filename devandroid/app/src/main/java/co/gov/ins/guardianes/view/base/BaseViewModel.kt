package co.gov.ins.guardianes.view.base

import androidx.lifecycle.ViewModel
import io.reactivex.disposables.CompositeDisposable

abstract class BaseViewModel : ViewModel() {

    protected val disposeBag: CompositeDisposable = CompositeDisposable()

    /**
     * Method released at the time of destroying the ViewModel
     */
    public override fun onCleared() {
        disposeBag.clear()
        super.onCleared()
    }


}