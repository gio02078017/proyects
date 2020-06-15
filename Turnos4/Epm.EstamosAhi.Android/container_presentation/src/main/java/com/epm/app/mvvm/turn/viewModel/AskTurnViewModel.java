package com.epm.app.mvvm.turn.viewModel;

import android.arch.lifecycle.MutableLiveData;

import com.epm.app.R;
import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.mvvm.comunidad.viewModel.BaseViewModel;
import com.epm.app.mvvm.turn.bussinesslogic.ITurnServicesBL;
import com.epm.app.mvvm.turn.network.request.AskTurnParameters;
import com.epm.app.mvvm.turn.network.response.askTurn.TurnResponse;
import com.epm.app.mvvm.turn.network.response.officeDetail.Oficina;
import com.epm.app.mvvm.turn.repository.TurnServicesRepository;
import com.epm.app.mvvm.turn.viewModel.iViewModel.IAskTurnViewModel;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.utils.Constants;
import io.reactivex.Observable;

public class AskTurnViewModel extends BaseViewModel implements IAskTurnViewModel {

    private MutableLiveData<Boolean> successAskTurn;
    private MutableLiveData<Boolean> problemsAskTurn;

    private ITurnServicesBL turnServicesBL;
    private ICustomSharedPreferences customSharedPreferences;
    private Oficina oficina;
    private String nameClient;
    private AskTurnParameters askTurnParameters;
    private TurnResponse turnResponse;
    private IValidateInternet validateInternet;


    @Inject
    public AskTurnViewModel(TurnServicesRepository turnServicesRepository,
                            CustomSharedPreferences customSharedPreferences, ValidateInternet validateInternet) {
        this.turnServicesBL = turnServicesRepository;
        this.customSharedPreferences = customSharedPreferences;
        progressDialog = new MutableLiveData<>();
        successAskTurn = new MutableLiveData<>();
        problemsAskTurn = new MutableLiveData<>();
        askTurnParameters = new AskTurnParameters();
        this.validateInternet = validateInternet;

    }

    @Override
    public void askTurn(String nameClient, Oficina office, Usuario user) {
        this.oficina = office;
        this.nameClient = nameClient;
        putAskTurnParameters(validateUserGuest(user.getNombres(), user.isInvitado()));
        Observable<TurnResponse> result = turnServicesBL.askTurnResponse(customSharedPreferences.getString(Constants.TOKEN), askTurnParameters);
        fetchService(result, validateInternet);
    }

    public void validateResponseTurn(TurnResponse responseAskTurn) {
        if (responseAskTurn != null) {
            setTurnResponse(responseAskTurn);
            validateMessageResponse();
        }
        progressDialog.setValue(false);
    }

    public void validateMessageResponse() {
        if (getTurnResponse().getMensaje() != null && getTurnResponse().getMensaje().getIdentificador() == 1) {
            successAskTurn.setValue(true);
        } else {
            problemsAskTurn.setValue(true);
        }
    }

    private void putAskTurnParameters(String name) {
        askTurnParameters.setIdDispositivo(IdDispositive.getIdDispositive());
        askTurnParameters.setIdOficinaSentry(oficina.getIdOficinaSentry());
        askTurnParameters.setNombreSolicitante(nameClient);
        askTurnParameters.setIdTramite(0);
        askTurnParameters.setIdOneSignal(customSharedPreferences.getString(Constants.ONE_SIGNAL_ID));
        askTurnParameters.setUsuarioAutenticado(name);
        askTurnParameters.setSistemaOperativo(Constants.SYSTEM_OPERATIVE);
    }

    public String validateUserGuest(String name, boolean guest) {
        if (guest) {
            return Constants.ID_USER;
        }
        return name;
    }

    @Override
    public MutableLiveData<Boolean> getSuccessAskTurn() {
        return successAskTurn;
    }

    @Override
    public MutableLiveData<Boolean> getProblemsAskTurn() {
        return problemsAskTurn;
    }

    @Override
    public TurnResponse getTurnResponse() {
        return turnResponse;
    }

    public void setTurnResponse(TurnResponse turnResponse) {
        this.turnResponse = turnResponse;
    }

    public void setAskTurnParameters(AskTurnParameters askTurnParameters) {
        this.askTurnParameters = askTurnParameters;
    }

    @Override
    protected void handleResponse(Object responseService) {
        validateResponseTurn((TurnResponse) responseService);
    }
}
