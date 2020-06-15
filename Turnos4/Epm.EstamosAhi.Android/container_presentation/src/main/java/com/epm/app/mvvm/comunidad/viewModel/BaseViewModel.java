package com.epm.app.mvvm.comunidad.viewModel;

import android.arch.lifecycle.MutableLiveData;
import android.arch.lifecycle.ViewModel;
import android.util.Log;
import com.epm.app.R;
import com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory.GuideProceduresAndRequirementsCategoryItem;

import app.epm.com.utilities.helpers.ErrorMessage;

import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.DisposableManager;
import io.reactivex.Observable;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.disposables.Disposable;
import io.reactivex.observers.DisposableObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;

public abstract class BaseViewModel extends ViewModel {

    protected MutableLiveData<ErrorMessage> error;
    private MutableLiveData<String> errorMessage;
    int errorUnauhthorized;
    protected MutableLiveData<String> titleMessage;
    protected MutableLiveData<Boolean> expiredToken;
    protected MutableLiveData<String> messageSuccesful;
    protected MutableLiveData<Boolean> progressDialog;
    protected Object response;
    protected final CompositeDisposable disposables;

    public BaseViewModel() {
        error = new MutableLiveData<>();
        expiredToken = new MutableLiveData<>();
        errorMessage = new MutableLiveData<>();
        titleMessage = new MutableLiveData<>();
        messageSuccesful = new MutableLiveData<>();
        progressDialog = new MutableLiveData<>();
        disposables = new CompositeDisposable();
    }

    @Override
    protected void onCleared() {
        DisposableManager.dispose();
        disposables.dispose();
        super.onCleared();
    }

    public void validateShowError(ErrorMessage errorMessage) {
        if (ValidateServiceCode.getErrorCode() == Constants.UNAUTHORIZED_ERROR_CODE) {
            this.error.setValue(errorMessage);
            this.errorUnauhthorized = errorMessage.getMessage();
            expiredToken.setValue(true);
        } else {
            showErrorService(errorMessage);
        }
        progressDialog.setValue(false);
    }

    public void showErrorService(ErrorMessage errorMessage) {
        this.error.setValue(errorMessage);
    }

    protected <T> void fetchService(Observable<T> result, IValidateInternet validateInternet) {
        if (validateInternet.isConnected() && result != null) {
            progressDialog.setValue(true);
            disposables.add(getDisposable(result));
        } else {
            validateShowError(new ErrorMessage(R.string.title_appreciated_user, R.string.text_validate_internet));
        }

    }

    private <T> Disposable getDisposable(Observable<T> result) {
        return result.subscribeOn(Schedulers.io())
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableObserver<T>() {
                    @Override
                    public void onNext(T response) {
                        progressDialog.setValue(false);
                        validateIfResponseIsNull(response);
                    }

                    @Override
                    public void onError(Throwable e) {
                        setError(e);
                    }

                    @Override
                    public void onComplete() {
                        Log.e("complete ", "complete");
                    }

                });
    }


    protected void setError(Throwable e) {
        if ("timeout".equals(e.getMessage())) {
            validateShowError(new ErrorMessage(R.string.title_appreciated_user, R.string.timeout));
        } else {
            indentifyException(e);
        }
    }

    private void indentifyException(Throwable e) {
        if (e instanceof HttpException) {
            ValidateServiceCode.captureServiceErrorCode(((HttpException) e).code());
            validateShowError(new ErrorMessage(ValidateServiceCode.getTitleError(), ValidateServiceCode.getError()));
        } else {
            validateShowError(new ErrorMessage(ValidateServiceCode.getTitleError404(), ValidateServiceCode.getError404()));
        }
    }


    private <T> void validateIfResponseIsNull(T response) {
        setResponse(response);
        if (isResponseNull(response)) {
            handleResponse(response);
        } else {
            validateShowError(new ErrorMessage(ValidateServiceCode.getTitleError(), ValidateServiceCode.getError()));
        }
    }

    protected void handleResponse(Object responseService) {

    }

    private static <T> boolean isResponseNull(T response) {
        return response != null;
    }


    protected void setResponse(Object response) {
        this.response = response;
    }


    public MutableLiveData<String> getTitleMessage() {
        return titleMessage;
    }


    public MutableLiveData<String> getMessageSuccesful() {
        return messageSuccesful;
    }


    public boolean isValidateMessage() {
        return false;
    }


    public MutableLiveData<Boolean> getProgressDialog() {
        return progressDialog;
    }


    public int getErrorUnauthorized() {
        return errorUnauhthorized;
    }


    public MutableLiveData<Boolean> getExpiredToken() {
        return expiredToken;
    }


    public MutableLiveData<String> getErrorMessage() {
        return errorMessage;
    }


    public MutableLiveData<ErrorMessage> getError() {
        return error;
    }


    public void showError() {

    }


}
