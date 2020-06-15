package com.epm.app.mvvm.util;

import android.arch.lifecycle.MutableLiveData;

import app.epm.com.utilities.helpers.ErrorMessage;

public interface IError {

    MutableLiveData<ErrorMessage> showError();

}
