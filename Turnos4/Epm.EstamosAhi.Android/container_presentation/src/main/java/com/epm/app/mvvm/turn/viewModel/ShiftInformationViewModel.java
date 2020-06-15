package com.epm.app.mvvm.turn.viewModel;

import android.arch.lifecycle.MutableLiveData;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.viewModel.BaseViewModel;
import com.epm.app.mvvm.turn.bussinesslogic.ITurnServicesBL;
import com.epm.app.mvvm.turn.network.request.CancelTurnParameters;
import com.epm.app.mvvm.turn.network.response.cancelTurn.CancelTurnResponse;
import com.epm.app.mvvm.turn.repository.TurnServicesRepository;
import com.epm.app.mvvm.turn.viewModel.iViewModel.IShiftInformationViewModel;
import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.utils.Constants;
import io.reactivex.Observable;

public class ShiftInformationViewModel extends BaseViewModel implements IShiftInformationViewModel {

    private MutableLiveData<Boolean> successCancelTurn;
    private MutableLiveData<Boolean> failedCancelTurn;
    private MutableLiveData<Boolean> failedWithActionCancelTurn;
    private ITurnServicesBL turnServicesBL;
    private IValidateInternet validateInternet;
    private ICustomSharedPreferences customSharedPreferences;
    private CancelTurnParameters cancelTurnParameters;
    private CancelTurnResponse cancelTurnResponse;
    private Integer numberTurn;

    @Inject
    public ShiftInformationViewModel(TurnServicesRepository turnServicesRepository,
                                     CustomSharedPreferences customSharedPreferences, ValidateInternet validateInternet) {
        this.turnServicesBL = turnServicesRepository;
        this.customSharedPreferences = customSharedPreferences;
        cancelTurnParameters = new CancelTurnParameters();
        this.successCancelTurn = new MutableLiveData<>();
        this.failedCancelTurn = new MutableLiveData<>();
        this.validateInternet = validateInternet;
        this.failedWithActionCancelTurn = new MutableLiveData<>();
    }

    @Override
    public void cancelTurn(Integer numberTurn) {
            this.numberTurn = numberTurn;
            progressDialog.setValue(true);
            putCancelTurnParameters();
            Observable<CancelTurnResponse> result = turnServicesBL.cancelTurnResponse(customSharedPreferences.getString(Constants.TOKEN), cancelTurnParameters);
            fetchService(result,validateInternet);
    }

    private void putCancelTurnParameters(){
        cancelTurnParameters.setIdDispositivo(IdDispositive.getIdDispositive());
        cancelTurnParameters.setIdTurno(numberTurn);
        cancelTurnParameters.setSistemaOperativo(Constants.SYSTEM_OPERATIVE);
    }


    @Override
    public MutableLiveData<Boolean> getSuccessCancelTurn() {
        return successCancelTurn;
    }

    @Override
    public MutableLiveData<Boolean> getFailedCancelTurn() {
        return failedCancelTurn;
    }

    @Override
    public MutableLiveData<Boolean> getFailedWithActionCancelTurn() {
        return failedWithActionCancelTurn;
    }

    @Override
    public CancelTurnResponse getCancelTurnResponse() {
        return cancelTurnResponse;
    }

    public void setCancelTurnResponse(CancelTurnResponse cancelTurnResponse) {
        this.cancelTurnResponse = cancelTurnResponse;
    }

    public CancelTurnParameters getCancelTurnParameters() {
        return cancelTurnParameters;
    }

    public void setCancelTurnParameters(CancelTurnParameters cancelTurnParameters) {
        this.cancelTurnParameters = cancelTurnParameters;
    }

    private void onChanged(CancelTurnResponse responseCancelTurn) {
        if (responseCancelTurn != null) {
            setCancelTurnResponse(responseCancelTurn);
            evaluateResponse(responseCancelTurn.getEstadoTransaccion());
        }
        progressDialog.setValue(false);
    }

    public void evaluateResponse(boolean stateTransaction){
        if(stateTransaction){
            successCancelTurn.setValue(stateTransaction);
            deletePreferences();
        }else{
            identificateWrongCause();
        }
    }

    public void identificateWrongCause(){
        if(cancelTurnResponse.getMensaje().getIdentificador() == 8){
            failedWithActionCancelTurn.setValue(true);
            deletePreferences();
        }else{
            failedCancelTurn.setValue(true);
        }
    }

    private void deletePreferences(){
        customSharedPreferences.deleteValue(Constants.ASSIGNED_TRUN);
        customSharedPreferences.deleteValue(Constants.INFORMATION_OFFICE_JSON);
    }


    @Override
    protected void handleResponse(Object responseService) {
        onChanged((CancelTurnResponse) responseService);
    }
}
