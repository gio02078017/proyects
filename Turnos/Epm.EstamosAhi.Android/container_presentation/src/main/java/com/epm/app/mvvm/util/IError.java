package com.epm.app.mvvm.util;

import androidx.lifecycle.MutableLiveData;

import app.epm.com.utilities.helpers.ErrorMessage;

public interface IError {

    MutableLiveData<ErrorMessage> showError();

}
