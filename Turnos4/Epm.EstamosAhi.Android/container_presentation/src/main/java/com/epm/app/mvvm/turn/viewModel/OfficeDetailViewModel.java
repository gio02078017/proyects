package com.epm.app.mvvm.turn.viewModel;

import android.arch.lifecycle.MutableLiveData;
import android.view.View;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.viewModel.BaseViewModel;
import com.epm.app.mvvm.turn.bussinesslogic.ITurnServicesBL;
import com.epm.app.mvvm.turn.models.StateMessage;
import com.epm.app.mvvm.turn.network.request.OfficeDetailParameters;
import com.epm.app.mvvm.turn.network.response.officeDetail.OfficeDetailResponse;
import com.epm.app.mvvm.turn.network.response.officeDetail.Oficina;
import com.epm.app.mvvm.turn.network.response.officeDetail.Turno;
import com.epm.app.mvvm.turn.repository.TurnServicesRepository;
import com.epm.app.mvvm.turn.viewModel.iViewModel.IOfficeDetailViewModel;

import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.InformationOffice;

import java.io.Serializable;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.utils.Constants;
import io.reactivex.Observable;

public class OfficeDetailViewModel extends BaseViewModel implements IOfficeDetailViewModel {

    private MutableLiveData<Boolean> successOfficeDetail;
    private MutableLiveData<Boolean> problemsOfficeDetail;
    private ITurnServicesBL turnServicesBL;
    private ICustomSharedPreferences customSharedPreferences;
    private OfficeDetailParameters officeDetailParameters;
    private OfficeDetailResponse officeDetailResponse;
    public final MutableLiveData<String> textTurnWait;
    public final MutableLiveData<Integer> isOfficeClose;
    public final MutableLiveData<String> textAverageTimeWait;
    public final MutableLiveData<String> textAddress;
    public final MutableLiveData<String> textSchedule;
    public final MutableLiveData<Integer> textStateOfficeVisible;
    public final MutableLiveData<Boolean> changeButtonAskTurn;
    public final MutableLiveData<Boolean> shiftInSameoffice;
    public final MutableLiveData<String> textTurnAssigned;
    public final MutableLiveData<String> textTurnInOffice;
    private MutableLiveData<StateMessage<java.io.Serializable>> turnAbandonedOrAttended;
    private MutableLiveData<StateMessage<java.io.Serializable>> turnCanceled;
    private InformationOffice informationOffice;
    private boolean controlShiftInformation;
    private IValidateInternet validateInternet;
    private StateMessage<java.io.Serializable> stateMessage;

    @Inject
    public OfficeDetailViewModel(TurnServicesRepository turnServicesRepository,
                                 CustomSharedPreferences customSharedPreferences, ValidateInternet validateInternet) {
        this.turnServicesBL = turnServicesRepository;
        this.customSharedPreferences = customSharedPreferences;
        this.validateInternet = validateInternet;
        officeDetailParameters = new OfficeDetailParameters();
        this.successOfficeDetail = new MutableLiveData<>();
        this.isOfficeClose = new MutableLiveData<>();
        this.problemsOfficeDetail = new MutableLiveData<>();
        progressDialog = new MutableLiveData<>();
        textTurnWait = new MutableLiveData<>();
        textAverageTimeWait = new MutableLiveData<>();
        textAddress = new MutableLiveData<>();
        textSchedule = new MutableLiveData<>();
        textStateOfficeVisible = new MutableLiveData<>();
        changeButtonAskTurn = new MutableLiveData<>();
        shiftInSameoffice = new MutableLiveData<>();
        textTurnAssigned = new MutableLiveData<>();
        textTurnInOffice = new MutableLiveData<>();
        turnAbandonedOrAttended = new MutableLiveData<>();
        turnCanceled = new MutableLiveData<>();
        stateMessage = new StateMessage<>();
        this.controlShiftInformation = false;
        initComponents();
    }

    public void initComponents() {
        textStateOfficeVisible.setValue(View.GONE);
        changeButtonAskTurn.setValue(false);
    }

    @Override
    public void getOfficeDetail(InformationOffice informationOffice, boolean controlShiftInformation) {
        this.informationOffice = informationOffice;
        this.controlShiftInformation = controlShiftInformation;
        putOfficeDetailParameters();
        Observable<OfficeDetailResponse> result = turnServicesBL.getOfficeDetailResponse(customSharedPreferences.getString(Constants.TOKEN), officeDetailParameters);
        fetchService(result, validateInternet);
    }

    private static boolean isOfficeDetailResponseNull(OfficeDetailResponse officeDetailResponse) {
        return officeDetailResponse != null;
    }


    @Override
    protected void handleResponse(Object responseService) {
        OfficeDetailResponse myResponseService = (OfficeDetailResponse) responseService;
        this.setOfficeDetailResponse(myResponseService);
        if (myResponseService.getOficina() != null) {
            validateInformationOffice(myResponseService.getOficina());
        }

        hasControlShiftInformation(myResponseService);

        progressDialog.setValue(false);
    }

    private void hasControlShiftInformation(OfficeDetailResponse responseOfficesDetail) {
        if (!controlShiftInformation) {
            applyForRequest(responseOfficesDetail);

        } else if (responseOfficesDetail.getTurno() != null) {
            validateTurn(responseOfficesDetail.getTurno(), responseOfficesDetail.getOficina());
            successOfficeDetail.setValue(true);
        }
    }

    private void applyForRequest(OfficeDetailResponse responseOfficesDetail) {
        if (responseOfficesDetail.getAplicaSolicitudTurno() != null && responseOfficesDetail.getAplicaSolicitudTurno()) {
            successOfficeDetail.setValue(true);
            changeButtonAskTurn.setValue(true);
        } else {
            validateIfHasAssignedTurn(responseOfficesDetail);
        }
    }

    private void validateIfHasAssignedTurn(OfficeDetailResponse officeDetailResponse) {
        if (officeDetailResponse.getDispositivoTieneTurnoAsignado() != null && officeDetailResponse.getDispositivoTieneTurnoAsignado()) {
            validateIfIdentificationEmpty(officeDetailResponse.getMensaje().getIdentificador());
        } else {
            CannotRequestShift();
        }
    }

    public void validateStateTurn(String stateOffice) {
        switch (stateOffice) {
            case Constants.STATE_SHIFT_ABANDONED:
                deletePreferences();
                setStateMessages(R.string.text_title_shift_abandoned, R.string.text_description_shift_abandoned);
                turnAbandonedOrAttended.setValue(stateMessage);
                break;
            case Constants.STATE_SHIFT_ATTENDED:
                deletePreferences();
                setStateMessages(R.string.text_title_shift_attended, R.string.text_description_shift_attended);
                turnAbandonedOrAttended.setValue(stateMessage);
                break;
            case Constants.STATE_SHIFT_CANCELED:
                deletePreferences();
                setStateMessages(officeDetailResponse.getTurno().getNombreSolicitante(), R.string.text_message_success_cancel_turn);
                turnCanceled.setValue(stateMessage);
                break;
            default:
                break;
        }
    }

    public void setStateMessages(int title, int message) {
        stateMessage.setTitle(title);
        stateMessage.setMessage(message);
    }

    public void setStateMessages(String title, int message) {
        stateMessage.setTitle(title);
        stateMessage.setMessage(message);
    }

    private void validateIfIdentificationEmpty(Integer identificador) {
        if (identificador == 1) {
            shiftInSameoffice.setValue(true);
        } else {
            CannotRequestShift();
        }
    }

    public void CannotRequestShift() {
        changeButtonAskTurn.setValue(false);
        problemsOfficeDetail.setValue(true);
    }

    private void validateInformationOffice(Oficina oficina) {
        if (oficina != null) {
            drawSuccessInformation(oficina);
        }
    }

    private void drawSuccessInformation(Oficina oficina) {
        textTurnWait.setValue(oficina.getTurnosEnEspera().toString());
        textAverageTimeWait.setValue(oficina.getTiempoPromedio());
        textAddress.setValue(oficina.getDireccion());
        textSchedule.setValue(oficina.getHorario());
        textStateOfficeVisible.setValue(oficina.getEstadoOficina().equalsIgnoreCase(Constants.OFFICE_STATED_CERRADA) ? View.VISIBLE : View.GONE);
    }

    private void validateTurn(Turno turno, Oficina oficina) {
        if (turno != null && controlShiftInformation) {
            drawSuccessTurn(turno, oficina);
            validateStateTurn(officeDetailResponse.getTurno().getEstadoDeTurno());
        }
    }

    private void drawSuccessTurn(Turno turno, Oficina oficina) {
        textTurnAssigned.setValue(turno.getTurnoAsignado());
        textTurnInOffice.setValue(oficina.getUltimoTurnoLlamado() != null ? oficina.getUltimoTurnoLlamado() : "");
        if (oficina.getEstadoOficina().equalsIgnoreCase(Constants.OFFICE_STATED_CERRADA)) {
            isOfficeClose.setValue(0);
        }
    }

    private void putOfficeDetailParameters() {
        officeDetailParameters.setIdDispositivo(IdDispositive.getIdDispositive());
        officeDetailParameters.setIdOficinaSentry(informationOffice.getIdOficinaSentry());
        officeDetailParameters.setSistemaOperativo(Constants.SYSTEM_OPERATIVE);
    }

    private void deletePreferences() {
        customSharedPreferences.deleteValue(Constants.ASSIGNED_TRUN);
        customSharedPreferences.deleteValue(Constants.INFORMATION_OFFICE_JSON);
    }

    @Override
    public MutableLiveData<Boolean> getSuccessOfficeDetail() {
        return successOfficeDetail;
    }

    @Override
    public MutableLiveData<Boolean> getProblemsOfficeDetail() {
        return problemsOfficeDetail;
    }

    @Override
    public MutableLiveData<Boolean> getChangeButtonAskTurn() {
        return changeButtonAskTurn;
    }

    public OfficeDetailParameters getOfficeDetailParameters() {
        return officeDetailParameters;
    }

    public void setOfficeDetailParameters(OfficeDetailParameters officeDetailParameters) {
        this.officeDetailParameters = officeDetailParameters;
    }

    @Override
    public MutableLiveData<StateMessage<Serializable>> getTurnAbandonedOrAttended() {
        return turnAbandonedOrAttended;
    }

    @Override
    public OfficeDetailResponse getOfficeDetailResponse() {
        return officeDetailResponse;
    }

    public void setOfficeDetailResponse(OfficeDetailResponse officeDetailResponse) {
        this.officeDetailResponse = officeDetailResponse;
    }

    public MutableLiveData<StateMessage<Serializable>> getTurnCanceled() {
        return turnCanceled;
    }

    public MutableLiveData<Integer> getIsOfficeClose() {
        return isOfficeClose;
    }

    @Override
    public MutableLiveData<Boolean> getShiftInSameoffice() {
        return shiftInSameoffice;
    }

}
