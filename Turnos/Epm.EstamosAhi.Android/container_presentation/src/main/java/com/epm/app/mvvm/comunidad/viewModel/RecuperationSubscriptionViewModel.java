package com.epm.app.mvvm.comunidad.viewModel;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;
import com.epm.app.mvvm.comunidad.bussinesslogic.ISubscriptionBL;
import com.epm.app.mvvm.comunidad.network.request.UpdateSubscriptionByMailRequest;
import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.RecoverySubscriptionResponse;
import com.epm.app.mvvm.comunidad.repository.SubscriptionRepository;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.IRecuperationSubscriptionViewModel;

import javax.inject.Inject;
import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import app.epm.com.utilities.helpers.Validations;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.DisposableManager;

public class RecuperationSubscriptionViewModel extends BaseViewModel implements IRecuperationSubscriptionViewModel {

    private ISubscriptionBL subscriptionBL;
    private ICustomSharedPreferences customSharedPreferences;
    public final MutableLiveData<String> email;
    private MutableLiveData<String> messageEmptyParameters;
    private MutableLiveData<String> messageInvalidEmail;
    private MutableLiveData<Boolean> isUpdateSubscription;
    private MutableLiveData<Mensaje> messageNotFoundEmail;
    private MutableLiveData<RecoverySubscriptionResponse> messageSuccess;
    private UpdateSubscriptionByMailRequest updateSubscriptionByMailRequest;
    private MutableLiveData<ErrorMessage> error;
    private MutableLiveData<Boolean> progressDialog,expiredToken;
    private int errorUnauthorized;



    @Inject
    public RecuperationSubscriptionViewModel(SubscriptionRepository subscriptionRepository, CustomSharedPreferences customSharedPreferences,UpdateSubscriptionByMailRequest updateSubscriptionByMailRequest) {
        this.subscriptionBL = subscriptionRepository;
        this.customSharedPreferences = customSharedPreferences;
        this.email = new MutableLiveData<>();
        error = new MutableLiveData<>();
        this.messageEmptyParameters = new MutableLiveData<>();
        this.messageInvalidEmail = new MutableLiveData<>();
        this.messageNotFoundEmail = new MutableLiveData<>();
        this.isUpdateSubscription = new MutableLiveData<>();
        progressDialog = new MutableLiveData<>();
        expiredToken = new MutableLiveData<>();
        this.messageSuccess = new MutableLiveData<>();
        this.updateSubscriptionByMailRequest = updateSubscriptionByMailRequest;

    }

    public void onClickRecuperationSubscription() {
        progressDialog.setValue(true);
        if (validateEmailToGetSubscriptions(email.getValue())) {
            getSubscriptionDeviceAlertsByEmail(email.getValue(),customSharedPreferences.getString(Constants.TOKEN));
        }

    }


    public boolean validateEmailToGetSubscriptions(String email) {
        if (validateIFInputIsEmpty(email)) {
            showSimpleMessage(Constants.EMPTY_PARAMETERS, null);
            return false;
        } else {
            return validEmail(email);

        }

    }

    @Override
    public boolean validateIFInputIsEmpty(String email) {
        if (email != null) {
            return email.isEmpty();
        } else {
            return true;
        }

    }



    @Override
    public boolean validEmail(String email) {
        if (!Validations.validateEmail(email)) {
            showSimpleMessage(Constants.INVALID_EMAIL, null);
            return false;
        }
        return true;
    }

    @Override
    public void showSimpleMessage( String type,Mensaje messaje) {
        progressDialog.setValue(false);
        switch (type){
            case Constants.EMPTY_PARAMETERS:
                messageEmptyParameters.setValue("1");
                break;
            case Constants.INVALID_EMAIL:
                messageInvalidEmail.setValue("1");
                break;
            case Constants.NOT_FOUND_EMAIL:
                messageNotFoundEmail.setValue(messaje);
                break;
        }

    }

    @Override
    public void getSubscriptionDeviceAlertsByEmail(String email, String token) {
        subscriptionBL.getSubscriptionDeviceAlertsByEmail(email,token).observeForever(recoverySubscriptionResponse -> {
            progressDialog.setValue(false);
            validateIfExitSubscription(recoverySubscriptionResponse);
        });
    }

    @Override
    public void validateIfExitSubscription( RecoverySubscriptionResponse response) {
        if (response.isEstadoTransaccion()){
            messageSuccess.setValue(response);
        }else{
            showSimpleMessage(Constants.NOT_FOUND_EMAIL,response.getMensaje());
        }
    }

    @Override
    public void updateSubscriptionDeviceAlertsByEmail(String email) {
        progressDialog.setValue(true);
        updateSubscriptionByMailRequest.setCorreoElectronico(email);
        updateSubscriptionByMailRequest.setIdTipoSuscripcionNotificacion(1);
        updateSubscriptionByMailRequest.setIdSuscripcionOneSignal(customSharedPreferences.getString(Constants.ONE_SIGNAL_ID));
        updateSubscriptionByMailRequest.setIdDispositivo(IdDispositive.getIdDispositive());

        subscriptionBL.updateSubscriptionDeviceAlertsByEmail(updateSubscriptionByMailRequest,customSharedPreferences.getString(Constants.TOKEN)).observeForever(notificationResponse -> {
            validateResponseUpdateSubscription(notificationResponse);
            progressDialog.setValue(false);
        });
    }

    @Override
    public void validateResponseUpdateSubscription(NotificationResponse notificationResponse) {
        if (notificationResponse.getEstadoTransaccion()){
            isUpdateSubscription.setValue(true);
        }
    }


    public MutableLiveData<String> getMessageEmptyParameters() {
        return messageEmptyParameters;
    }

    public MutableLiveData<String> getMessageInvalidEmail() {
        return messageInvalidEmail;
    }

    public MutableLiveData<Mensaje> getMessageNotFoundEmail() {
        return messageNotFoundEmail;
    }

    public MutableLiveData<RecoverySubscriptionResponse> getMessageSuccess() {
        return messageSuccess;
    }

    public MutableLiveData<Boolean> getIsUpdateSubscription() {
        return isUpdateSubscription;
    }

    public MutableLiveData<String> getEmail() {
        return email;
    }





    @Override
    public void showError() {
        subscriptionBL.showError().observeForever(errorMessage -> {
            progressDialog.setValue(false);
            validateErrorCode(errorMessage);
        });

    }

    public void validateErrorCode(ErrorMessage errorMessage){
        if(ValidateServiceCode.getErrorCode() == Constants.UNAUTHORIZED_ERROR_CODE){
            errorUnauthorized=errorMessage.getMessage();
            expiredToken.setValue(true);
        }else{
            this.error.setValue(errorMessage);
        }
    }

    @Override
    protected void onCleared() {
        super.onCleared();
        DisposableManager.dispose();
    }

    @Override
    public int getErrorUnauthorized() {
        return errorUnauthorized;
    }

    public MutableLiveData<Boolean> getProgressDialog() {
        return progressDialog;
    }




}
