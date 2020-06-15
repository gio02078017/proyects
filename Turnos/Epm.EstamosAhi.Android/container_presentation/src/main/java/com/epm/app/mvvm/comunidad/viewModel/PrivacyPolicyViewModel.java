package com.epm.app.mvvm.comunidad.viewModel;

import androidx.lifecycle.MutableLiveData;
import android.view.View;

import com.epm.app.mvvm.comunidad.bussinesslogic.IWebViewBL;
import com.epm.app.mvvm.comunidad.repository.WebViewRepository;

import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.IPrivacyPolicyViewModel;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.DisposableManager;

public class PrivacyPolicyViewModel extends BaseViewModel implements IPrivacyPolicyViewModel {

    private IWebViewBL webViewBL;
    private ICustomSharedPreferences customSharedPreferences;
    private String url;
    private MutableLiveData<Boolean> successful;
    public final MutableLiveData<Integer> visibilityNotFound;
    public final MutableLiveData<Integer> progress;

    @Inject
    public PrivacyPolicyViewModel(WebViewRepository webViewRepository, CustomSharedPreferences customSharedPreferences) {
        this.webViewBL = webViewRepository;
        this.customSharedPreferences = customSharedPreferences;
        this.successful = new MutableLiveData<>();
        this.expiredToken = new MutableLiveData<>();
        this.visibilityNotFound = new MutableLiveData<>();
        this.progress = new MutableLiveData<>();
    }

    @Override
    public void loadUrl(){
        visibilityNotFound.setValue(View.INVISIBLE);
        progress.setValue(View.VISIBLE);
        webViewBL.getUrlPolicyPrivacy(customSharedPreferences.getString(Constants.TOKEN)).observeForever( policyPrivacy -> {
            if(policyPrivacy != null && policyPrivacy.getUrlPoliticaDePrivacidad() != null) {
                url = policyPrivacy.getUrlPoliticaDePrivacidad();
                successful.setValue(true);
            }
        });
    }


    public void validateShowError(ErrorMessage errorMessage) {
        if (ValidateServiceCode.getErrorCode() == Constants.UNAUTHORIZED_ERROR_CODE) {
            this.errorUnauhthorized = errorMessage.getMessage();
            expiredToken.setValue(errorMessage);
        } else {
            visibilityNotFound.setValue(View.VISIBLE);
            progress.setValue(View.GONE);
        }
    }

    @Override
    public String getUrl(){
      return url;
    }

    @Override
    public MutableLiveData<Boolean> getSuccessful() {
        return successful;
    }

    @Override
    protected void onCleared() {
        super.onCleared();
        DisposableManager.dispose();
    }



    @Override
    public void showError() {
        webViewBL.showError().observeForever(errorMessage -> {
            if(error != null ) {
                validateShowError(errorMessage);
            }
        });
        webViewBL.showErrorInternet().observeForever(errorInternet -> {
            if(errorInternet != null ) {
                this.error.setValue(new ErrorMessage(webViewBL.showError().getValue().getTitle(),errorInternet));
            }
        });
    }


}
