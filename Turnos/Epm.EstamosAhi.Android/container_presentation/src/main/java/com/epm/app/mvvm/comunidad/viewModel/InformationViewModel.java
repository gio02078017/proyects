package com.epm.app.mvvm.comunidad.viewModel;

import androidx.lifecycle.MutableLiveData;
import android.view.View;

import com.epm.app.mvvm.comunidad.bussinesslogic.IWebViewBL;
import com.epm.app.mvvm.comunidad.repository.WebViewRepository;

import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.IInformationViewModel;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.DisposableManager;

import static io.fabric.sdk.android.services.common.CommonUtils.isNullOrEmpty;

public class InformationViewModel extends BaseViewModel implements IInformationViewModel{

    private ICustomSharedPreferences customSharedPreferences;
    private IWebViewBL informationInterestBL;
    private MutableLiveData<String> getUrl;
    private MutableLiveData<Boolean> expiredToken,internet;
    public final MutableLiveData<Integer> visibilityNotFound;
    public final MutableLiveData<Integer> progress;
    private int error;
    private int titleError;

    @Inject
    public InformationViewModel(CustomSharedPreferences customSharedPreferences, WebViewRepository webViewRepository) {
        this.customSharedPreferences = customSharedPreferences;
        this.informationInterestBL = webViewRepository;
        this.expiredToken = new MutableLiveData<>();
        this.visibilityNotFound = new MutableLiveData<>();
        this.progress = new MutableLiveData<>();
        this.getUrl = new MutableLiveData<>();
        this.internet = new MutableLiveData<>();
    }

    public void getUrlInformation() {
        visibilityNotFound.setValue(View.INVISIBLE);
        progress.setValue(View.VISIBLE);
        informationInterestBL.getUrlInformationInterest(customSharedPreferences.getString(Constants.TOKEN)).observeForever(informationInterest -> {
            if (informationInterest != null) {
                loadUrl(informationInterest.getUrlInformacionDeInteres());
            }
        });
    }

    public void tryAgain(){
        getUrlInformation();
    }

    public void loadUrl(String url) {
        if (!isNullOrEmpty(url)) {
            this.getUrl.setValue(url);
        }
    }

    @Override
    public void showError() {
        informationInterestBL.showError().observeForever(this::validateError);
        informationInterestBL.showErrorInternet().observeForever(error -> {
            this.titleError = informationInterestBL.showError().getValue().getTitle();
            this.error = error;
            internet.setValue(true);
        });
    }

    public void validateError(ErrorMessage errorMessage){
        if(ValidateServiceCode.getErrorCode()==Constants.UNAUTHORIZED_ERROR_CODE){
            this.titleError = errorMessage.getTitle();
            this.error = errorMessage.getMessage();
            expiredToken.setValue(true);
        }else{
            visibilityNotFound.setValue(View.VISIBLE);
            progress.setValue(View.GONE);
        }
    }

    public MutableLiveData<Boolean> getInternet() {
        return internet;
    }

    @Override
    public MutableLiveData<String> getUrl() {
        return getUrl;
    }

    @Override
    public int getIntTitleError() {
        return titleError;
    }



    public int getIntError() {
        return error;
    }

    @Override
    protected void onCleared() {
        super.onCleared();
        DisposableManager.dispose();
    }

}
