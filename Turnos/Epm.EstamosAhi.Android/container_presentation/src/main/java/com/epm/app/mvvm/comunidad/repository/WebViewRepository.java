package com.epm.app.mvvm.comunidad.repository;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.bussinesslogic.IWebViewBL;
import com.epm.app.mvvm.comunidad.network.WebViewService;
import com.epm.app.mvvm.comunidad.network.response.webViews.InformationInterest;
import com.epm.app.mvvm.comunidad.network.response.webViews.PrivacyPolicy;

import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.utils.DisposableManager;
import app.epm.com.utilities.helpers.ValidateServiceCode;

import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.ValidateInternet;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.Disposable;
import io.reactivex.schedulers.Schedulers;

public class WebViewRepository implements IWebViewBL {

    private MutableLiveData<Integer>  errorInternet;
    private MutableLiveData<ErrorMessage>  error;
    private WebViewService webService;
    private IValidateInternet validateInternet;

    public WebViewRepository(WebViewService webViewService, ValidateInternet validateInternet) {
        this.error = new MutableLiveData<>();
        this.errorInternet = new MutableLiveData<>();
        this.webService = webViewService;
        this.validateInternet = validateInternet;
    }

    @Override
    public MutableLiveData<InformationInterest> getUrlInformationInterest(String token) {
        MutableLiveData<InformationInterest> informationInterestMutableLiveData = new MutableLiveData<>();
        if (validateInternet.isConnected()) {
            InformationInterest informationInterest = new InformationInterest();
            Disposable disposable = webService.getUrlInformationInterest(token)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribe(response -> {
                        if (ValidateServiceCode.captureServiceErrorCode(response.code())) {
                            if (response.isSuccessful()) {
                                if (response.body() != null) {
                                    informationInterest.setUrlInformacionDeInteres(response.body().getUrlInformacionDeInteres());
                                    informationInterest.setMensaje(response.body().getMensaje());
                                }
                                informationInterestMutableLiveData.setValue(informationInterest);
                            }
                        }else{
                            setError(ValidateServiceCode.getTitleError(),ValidateServiceCode.getError());
                        }
                    }, throwable -> {
                        setError(R.string.title_error,R.string.text_error_500);
                    });
            DisposableManager.add(disposable);
        } else {
            errorInternet();
        }

        return informationInterestMutableLiveData;
    }

    private void setError(int title, int message) {
        error.setValue(new ErrorMessage(title,message));
    }

    @Override
    public MutableLiveData<PrivacyPolicy> getUrlPolicyPrivacy(String token){
        MutableLiveData<PrivacyPolicy> policyPrivacyMutableLiveData = new MutableLiveData<>();
        if(validateInternet.isConnected()){
            PrivacyPolicy privacyPolicy = new PrivacyPolicy();
            Disposable disposable = webService.getUrlPolicyPrivacy(token)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribe(response -> {
                        if (ValidateServiceCode.captureServiceErrorCode(response.code())) {
                            if (response.isSuccessful()) {
                                if (response.body() != null) {
                                    privacyPolicy.setUrlPoliticaDePrivacidad(response.body().getUrlPoliticaDePrivacidad());
                                    privacyPolicy.setMensaje(response.body().getMensaje());
                                }
                                policyPrivacyMutableLiveData.setValue(privacyPolicy);
                            }
                        } else {
                            setError(ValidateServiceCode.getTitleError(),ValidateServiceCode.getError());
                        }
                    }, throwable -> {
                        setError(R.string.title_error,R.string.text_error_500);
                    });
            DisposableManager.add(disposable);
        }else {
            errorInternet();
        }
        return policyPrivacyMutableLiveData;
    }

    private void errorInternet() {
        setError(R.string.title_appreciated_user,R.string.text_validate_internet);
    }


    @Override
    public MutableLiveData<ErrorMessage> showError() {
        return error;
    }


    @Override
    public MutableLiveData<Integer> showErrorInternet(){
        return errorInternet;
    }


}
