package com.epm.app.mvvm.turn.viewModel;

import android.arch.lifecycle.MutableLiveData;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.viewModel.BaseViewModel;
import com.epm.app.mvvm.turn.bussinesslogic.ITurnServicesBL;
import com.epm.app.mvvm.turn.network.response.AssignedTurn;
import com.epm.app.mvvm.turn.network.response.ShiftDevice;
import com.epm.app.mvvm.turn.repository.TurnServicesRepository;
import com.epm.app.mvvm.turn.viewModel.iViewModel.IOficinasDeAtencionViewModel;

import app.epm.com.utilities.helpers.ErrorMessage;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.utils.Constants;
import io.reactivex.Observable;

/**
 * Created by root on 28/03/17.
 */

public class OficinasDeAtencionViewModel extends BaseViewModel implements IOficinasDeAtencionViewModel {

    private ITurnServicesBL turnServicesRepository;
    private ICustomSharedPreferences customSharedPreferences;
    private IValidateInternet validateInternet;
    private MutableLiveData<AssignedTurn> assignedTurn;
    private MutableLiveData<Boolean> withOutAssignedTurn;


    @Inject
    public OficinasDeAtencionViewModel(TurnServicesRepository turnServicesRepository, CustomSharedPreferences customSharedPreferences, ValidateInternet validateInternet) {
        this.turnServicesRepository = turnServicesRepository;
        this.customSharedPreferences = customSharedPreferences;
        this.validateInternet = validateInternet;
        this.assignedTurn = new MutableLiveData<>();
        this.withOutAssignedTurn = new MutableLiveData<>();
    }

    @Override
    public void getAssignedTurn() {
        Observable<AssignedTurn> result = turnServicesRepository.getAssignedTurn(customSharedPreferences.getString(Constants.TOKEN), IdDispositive.getIdDispositive());
        fetchService(result, validateInternet);
    }

    public void validateTurn(AssignedTurn assignedTurn) {
        progressDialog.setValue(false);
        if (assignedTurnOrTurnDeviceIsNotNull(assignedTurn)) {
            this.assignedTurn.setValue(assignedTurn);
        } else {
            withOutAssignedTurn.setValue(true);
        }
    }

    public boolean getAssignedTurnIsNullOrEmpty(ShiftDevice shiftDevice) {
        return shiftDevice.getTurnoAsignado() == null || shiftDevice.getTurnoAsignado().isEmpty();
    }


    @Override
    public MutableLiveData<AssignedTurn> getResponseAssignedTurn() {
        return assignedTurn;
    }

    @Override
    public MutableLiveData<Boolean> getWithOutAssignedTurn() {
        return withOutAssignedTurn;
    }


    @Override
    protected void handleResponse(Object responseService) {
        validateTurn((AssignedTurn) responseService);
    }

    public boolean assignedTurnOrTurnDeviceIsNotNull(AssignedTurn assignedTurn) {
        if (assignedTurn != null && assignedTurn.getShiftDevice() != null) {
            return !getAssignedTurnIsNullOrEmpty(assignedTurn.getShiftDevice());
        } else {
            customSharedPreferences.deleteValue(Constants.ASSIGNED_TRUN);
        }
        return false;
    }


}
