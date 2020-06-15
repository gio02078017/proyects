package com.epm.app.mvvm.comunidad.viewModel.iViewModel;

import androidx.lifecycle.MutableLiveData;

import app.epm.com.utilities.helpers.ErrorMessage;

public interface IViewModel {

    interface IError{
        MutableLiveData<String> getErrorMessage();
        MutableLiveData<ErrorMessage> getError();
        void showError();
        int getErrorUnauthorized();
        MutableLiveData<ErrorMessage> getExpiredToken();

    }

    interface IMessage
    {
        MutableLiveData<Boolean> getProgressDialog();
    }


}
