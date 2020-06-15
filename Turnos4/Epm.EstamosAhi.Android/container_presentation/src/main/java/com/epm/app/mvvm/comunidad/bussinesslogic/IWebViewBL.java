package com.epm.app.mvvm.comunidad.bussinesslogic;

import android.arch.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.network.response.webViews.InformationInterest;
import com.epm.app.mvvm.comunidad.network.response.webViews.PrivacyPolicy;
import com.epm.app.mvvm.util.IError;

public interface IWebViewBL extends IError {

    MutableLiveData<InformationInterest> getUrlInformationInterest(String token);
    MutableLiveData<PrivacyPolicy> getUrlPolicyPrivacy(String token);
    MutableLiveData<Integer> showErrorInternet();
}
