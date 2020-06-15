package com.epm.app.mvvm.turn.views.activities;

import androidx.lifecycle.ViewModelProviders;
import android.content.DialogInterface;
import android.content.Intent;
import androidx.databinding.DataBindingUtil;
import android.graphics.Typeface;
import android.os.Bundle;
import android.os.Handler;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.widget.Toolbar;
import android.text.Spannable;
import android.text.SpannableString;
import android.text.style.StyleSpan;
import android.text.style.UnderlineSpan;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.TextView;

import com.epm.app.R;
import com.epm.app.databinding.ActivityShiftInformationBinding;
import com.epm.app.mvvm.procedure.models.ProcedureInformation;
import com.epm.app.mvvm.procedure.views.activities.DetailsOfTheTransactionActivity;
import com.epm.app.mvvm.turn.network.response.askTurn.TurnResponse;
import com.epm.app.mvvm.turn.viewModel.OfficeDetailViewModel;
import com.epm.app.mvvm.turn.viewModel.ShiftInformationViewModel;
import com.epm.app.mvvm.turn.views.dialogs.ChooseMapProviderDialogFragment;
import app.epm.com.utilities.helpers.InformationOffice;

import com.epm.app.view.activities.LandingActivity;
import com.google.gson.Gson;

import java.util.Timer;
import java.util.TimerTask;

import app.epm.com.utilities.utils.ChangeStateTurnManager;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.ConvertUtilities;
import app.epm.com.utilities.utils.IChangeStateTurnObserver;
import app.epm.com.utilities.utils.INotificationObserver;
import app.epm.com.utilities.utils.IStateTurnObserver;
import app.epm.com.utilities.utils.StateTurnManager;

public class ShiftInformationActivity extends BaseOfficeDetailActivity implements IChangeStateTurnObserver {

    ActivityShiftInformationBinding binding;
    private Toolbar toolbarApp;

    ShiftInformationViewModel shiftInformationViewModel;
    OfficeDetailViewModel officeDetailViewModel;
    private TimerTask scanTask;
    private final Handler handler = new Handler();
    private Timer t = new Timer();
    private InformationOffice informationOffice;
    private TurnResponse turnResponse;
    private int idTurn = 0;
    private int currentTiempoDeEsperaDetalleOficina;
    private boolean maxTiempoDeEsperaDetalleOficina = false;
    private Bundle datosBeforeActivity;
    DialogInterface dialogInterfaceCancel;
    private ProcedureInformation procedureInformation;
    private boolean doubleClick;
    private boolean successService;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this, R.layout.activity_shift_information);
        new ChooseMapProviderDialogFragment();
        this.configureDagger();
        shiftInformationViewModel = ViewModelProviders.of(this, viewModelFactory).get(ShiftInformationViewModel.class);
        officeDetailViewModel = ViewModelProviders.of(this, viewModelFactory).get(OfficeDetailViewModel.class);
        binding.setShiftInformationViewModel(shiftInformationViewModel);
        binding.setOfficeDetailViewModel(officeDetailViewModel);
        loadDrawerLayout(R.id.generalDrawerLayout);
        toolbarApp = (Toolbar) binding.toolbarShiftInformationMenuOption;
        ChangeStateTurnManager.getInstance().getNotificationSubject().attach(this);
        loadToolbar();
        loadBinding();
        loadBindingOfficeDetailViewModel();
        datosBeforeActivity = this.getIntent().getExtras();
        startMap(binding.ubicacionMapView, 90, true);
        validateInternetToLoadMap();
        putUnderlineSpan();
    }


    @Override
    protected void onResume() {
        super.onResume();
        binding.ubicacionMapView.resume();
        doubleClick = false;
    }

    @Override
    protected void onStart() {
        super.onStart();
        canDoZoom();
        setControlNeverAsk(false);
        currentTiempoDeEsperaDetalleOficina = 0;
    }

    @Override
    protected void onPause() {
        super.onPause();
        binding.ubicacionMapView.pause();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        binding.ubicacionMapView.dispose();
        stopScan();
    }


    protected void initInformation() {
        if (datosBeforeActivity.getSerializable(Constants.INFORMATION_OFFICE) != null) {
            informationOffice = (InformationOffice) datosBeforeActivity.getSerializable(Constants.INFORMATION_OFFICE);
            loadLocation(informationOffice);
            binding.textNameOffice.setText(informationOffice.getNombreOficina());
        }
    }

    protected void successValidateGPS() {
        if (datosBeforeActivity.getSerializable(Constants.SHIFT_INFORMATION) != null) {
            turnResponse = (TurnResponse) datosBeforeActivity.getSerializable(Constants.SHIFT_INFORMATION);
            validateIsShowTurn();
            idTurn = turnResponse.getIdTurnoSentry();
        } else {
            callDetailViewModel();
        }
    }

    private void validateIsShowTurn() {
        if (!turnResponse.isShowTurn()) {
            dialogSuccessTurn();
            turnResponse.setShowTurn(true);
        }
    }

    protected void callDetailViewModel() {
        controlStateDialog();
        if (informationOffice != null) {
            closeDalogsOpened();
            officeDetailViewModel.getOfficeDetail(informationOffice, true);
        }
    }

    private void loadBinding() {
        shiftInformationViewModel.showError();
        shiftInformationViewModel.getProgressDialog().observe(this, this::showOrDimissProgressBar);
        shiftInformationViewModel.getError().observe(this, errorMessage -> {
            dismissProgressDialog();
            showAlertDialog(errorMessage.getTitle(), errorMessage.getMessage());
        });

        shiftInformationViewModel.getExpiredToken().observe(this, errorMessage -> {
            stopScan();
            showAlertDialogUnauthorized(errorMessage.getTitle(),errorMessage.getMessage(),R.string.accept_button_text);
        });

        shiftInformationViewModel.getSuccessCancelTurn().observe(this, success -> {
            showActionCancelDialog(getString(R.string.text_message_success_cancel_turn));
            if(procedureInformation != null){
                procedureInformation = null;
            }
        });

        shiftInformationViewModel.getFailedCancelTurn().observe(this, failed ->
                showAlertDialog(shiftInformationViewModel.getCancelTurnResponse().getMensaje().getTitulo(), shiftInformationViewModel.getCancelTurnResponse().getMensaje().getContenido()));

        shiftInformationViewModel.getFailedWithActionCancelTurn().observe(this, failed -> {
            showActionCancelDialog(shiftInformationViewModel.getCancelTurnResponse().getMensaje().getContenido());
        });
    }

    private void loadBindingOfficeDetailViewModel() {
        officeDetailViewModel.showError();
        officeDetailViewModel.getProgressDialog().observe(this, isProgressShown -> {
            showOrDimissProgressBar(isProgressShown);
        });
        officeDetailViewModel.getError().observe(this, errorMessage -> {
            dismissProgressDialog();
            stopScan();
            this.showAlertDialogTryAgain(errorMessage.getTitle(), errorMessage.getMessage(), R.string.text_intentar, R.string.text_cancelar);

        });
        officeDetailViewModel.getExpiredToken().observe(this, errorMessage -> {
            stopScan();
            showAlertDialogUnauthorized(errorMessage.getTitle(),errorMessage.getMessage(),R.string.accept_button_text);
        } );

        officeDetailViewModel.getProblemsOfficeDetail().observe(this, problems -> {
            showAlertDialog(officeDetailViewModel.getOfficeDetailResponse().getMensaje().getTitulo(), officeDetailViewModel.getOfficeDetailResponse().getMensaje().getContenido());
            doRefresh();
        });


        officeDetailViewModel.getSuccessOfficeDetail().observe(this, success -> {
            doRefresh();
            idTurn = officeDetailViewModel.getOfficeDetailResponse().getTurno().getIdTurnoSentry();
            canDoZoom();
        });

        officeDetailViewModel.getIsOfficeClose().observe(this, i->{
            showAlertDialog(R.string.title_clese_office,R.string.message_clese_office);
        });

        officeDetailViewModel.getTurnCanceled().observe(this, stateMessage -> {
            if(stateMessage != null){
                showTurnNotAssigned((String)stateMessage.getTitle(),getString ((int) stateMessage.getMessage()));
            }
        });
        officeDetailViewModel.getTurnAbandonedOrAttended().observe(this, stateMessage -> {
            if(stateMessage != null){
                closeDalogsOpened();
                showTurnNotAssigned(getString((int)stateMessage.getTitle()),getString((int)stateMessage.getMessage()));
            }
        });

        officeDetailViewModel.getTurnOpenedState().observe(this, stateMessage -> {
            if(stateMessage != null){
                saveShiftInformationInPreferences(informationOffice);
            }
        });

        officeDetailViewModel.getLiveDataProcedureInformation().observe(this, result ->{
            if(result != null){
                procedureInformation = result;
            }
        });
    }

    private void showTurnNotAssigned(String title, String message){
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(title, message, false, R.string.text_aceptar_uppercase, (dialogInterface, i) -> {
            goToOfficeDetailActivity();
        }, null));
    }

    private void goToOfficeDetailActivity(){
        Intent intent = new Intent(ShiftInformationActivity.this, OfficeDetailActivity.class);
        intent.putExtra(Constants.INFORMATION_OFFICE, informationOffice);
        AddProcedureInformation(intent);
        startActivityWithOutDoubleClick(intent);
        finish();
    }

    public void AddProcedureInformation(Intent intent){
        if(procedureInformation == null){
            if(datosBeforeActivity.getSerializable(Constants.PROCEDURE_INFORMATION) != null){
                procedureInformation = (ProcedureInformation)datosBeforeActivity.getSerializable(Constants.PROCEDURE_INFORMATION);
                intent.putExtra(Constants.PROCEDURE_INFORMATION, procedureInformation);
            }
        }
    }

    private void saveShiftInformationInPreferences(InformationOffice informationOffice) {
        getCustomSharedPreferences().addString(Constants.INFORMATION_OFFICE_JSON, new Gson().toJson(informationOffice));
    }


    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == Constants.ACTION_TO_TURN_ON_GPS) {
            loadLocation(informationOffice);
        }
        super.onActivityResult(requestCode, resultCode, data);
    }

    private void callService(){
        if (getValidateInternet().isConnected() && successService) {
            itDoHandLocationWithNotDissmissModal(true);
        }
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar((Toolbar) toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
    }

    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }

    private void showAlertDialogTryAgain(final int title, final int message, final int positive, final int negative) {
        runOnUiThread(() -> {
            getCustomAlertDialog().showAlertDialog((getUsuario()!= null &&  !getUsuario().isInvitado() && title == R.string.title_appreciated_user) ? getUsuario().getNombres(): getString(title), message, false, positive, (dialogInterface, i) -> {
                callDetailViewModel();
            }, negative, (dialogInterface, i) -> onBackPressed() , null);
        });
    }

    protected void showAlertDialog(final String title, final String message) {
        runOnUiThread(() -> showAlertDialogGeneralInformationOnUiThread(title, message));
    }


    private void showAlertDialog(final int title, final int message) {
        runOnUiThread(() -> showAlertDialogGeneralInformationOnUiThread(title, message));
    }

    private void showCancelDialog() {
        runOnUiThread(() ->
                getCustomAlertDialog().showAlertDialog(
                        officeDetailViewModel.getOfficeDetailResponse().getTurno().getNombreSolicitante(),
                        String.format(
                                this.getString(R.string.text_message_cancel_turn),
                                informationOffice.getNombreOficina(),
                                officeDetailViewModel.getOfficeDetailResponse().getTurno().getTurnoAsignado()),
                        false,
                        R.string.text_cancel_turn_uppercase,
                        (dialogInterface, i) ->{
                            dialogInterfaceCancel = dialogInterface;
                            shiftInformationViewModel.cancelTurn(idTurn); },
                        R.string.text_return,
                        (dialogInterface, i) ->{
                            dialogInterfaceCancel = dialogInterface;
                            dialogInterface.dismiss();}, null));

    }

    private void showActionCancelDialog(String message) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(!officeDetailViewModel.getOfficeDetailResponse().getTurno().getNombreSolicitante().equalsIgnoreCase("") ? officeDetailViewModel.getOfficeDetailResponse().getTurno().getNombreSolicitante() : getString(R.string.title_appreciated_user), message, false, R.string.text_aceptar_uppercase, (dialogInterface, i) -> {
            goToOfficeDetailActivity();
        }, null));
    }

    private void doRefresh() {

        scanTask = new TimerTask() {
            public void run() {
                handler.post(() -> {
                    currentTiempoDeEsperaDetalleOficina += officeDetailViewModel.getOfficeDetailResponse().getTiempoParaRefrescarDetalleOficina();
                    validateMaxTiempoDeEsperaDetalleOficina();
                });
            }
        };

        t.schedule(scanTask, ConvertUtilities.convertFromMinutesToMilliseconds(officeDetailViewModel.getOfficeDetailResponse().getTiempoParaRefrescarDetalleOficina()));
    }

    private void validateMaxTiempoDeEsperaDetalleOficina() {
        if (currentTiempoDeEsperaDetalleOficina >= officeDetailViewModel.getOfficeDetailResponse().getTiempoDeEsperaDetalleOficina()) {
            maxTiempoDeEsperaDetalleOficina = true;
            closeDalogsOpened();
            showAlertDialogGeneralInformationOnUiThread(R.string.text_title_expired_session, R.string.text_message_expired_session);
        } else {
            allowDoingRefresh();
        }
    }

    private void allowDoingRefresh(){
        closeDalogsOpened();
        if (getValidateInternet().isConnected()) {
            callDetailViewModel();
            canDoZoom();
        }else{
            showAlertDialogTryAgain(R.string.title_appreciated_user, R.string.text_validate_internet, R.string.text_intentar, R.string.text_cancelar);
        }
    }

    public void canDoZoom(){
        setItDoZoom(true);
        startLocation();
    }

    public void generalPositiveAction() {
        if (maxTiempoDeEsperaDetalleOficina) {
            onBackPressed();
            maxTiempoDeEsperaDetalleOficina = false;
        }
    }

    public void stopScan() {
        if (scanTask != null) {
            scanTask.cancel();
        }
    }

    public void dialogSuccessTurn() {

        LayoutInflater factory = LayoutInflater.from(this);
        final View successTurnDialogView = factory.inflate(R.layout.dialog_success_turn, null);
        final AlertDialog successTurnDialog = new AlertDialog.Builder(this).create();
        successTurnDialog.setCancelable(false);
        successTurnDialog.setView(successTurnDialogView);
        TextView textNameClient = successTurnDialogView.findViewById(R.id.textNameClient);
        TextView textAssignedShift = successTurnDialogView.findViewById(R.id.textAssignedShift);
        TextView textMessage = successTurnDialogView.findViewById(R.id.textMessage);
        textNameClient.setText(turnResponse.getNombreSolicitante());
        textAssignedShift.setText(turnResponse.getTurnoAsignado());
        String message = String.format(getString(R.string.text_message_dialog_success_turn), turnResponse.getNombreOficina());
        String office = turnResponse.getNombreOficina();
        int start = 32;
        int fin = start + office.length();
        Spannable strMessage = new SpannableString(message);
        strMessage.setSpan(new StyleSpan(Typeface.BOLD), start, fin, Spannable.SPAN_EXCLUSIVE_EXCLUSIVE);
        textMessage.setText(strMessage, TextView.BufferType.SPANNABLE);
        successTurnDialogView.findViewById(R.id.btnAcept).setOnClickListener(view -> {
            callDetailViewModel();
            successTurnDialog.dismiss();
        });
        successTurnDialog.show();

    }

    protected void cancelPermisonGPSBaseOffice(boolean defaultLocation) {
        cancelPermisonGPS(defaultLocation);
    }

    public void cancelPermisonGPS(boolean defaultLocation) {
        pointCenter(informationOffice);
        callDetailViewModel();
    }

    public void eventCancelTurn(View view) {
        controlStateDialog();
        if (idTurn != 0) {
            showCancelDialog();
        }
    }

    public void eventRefreshLocation(View view) {
        itDoHandLocation(true);
    }

    public void eventGoToOffice(View view) {
        eventGoToOffice();
    }

    public void controlStateDialog(){
        if(getStateDialog()){
            dismissProgressDialog();
        }
    }

    public void eventGoDetailProcedure(View view) {
        if(!doubleClick) {
            doubleClick = true;
            Intent intent = new Intent(ShiftInformationActivity.this, DetailsOfTheTransactionActivity.class);
            intent.putExtra(Constants.PROCEDURE_INFORMATION, procedureInformation);
            intent.putExtra(Constants.CALL_SINCE_SHIFT_INFORMATION, true);
            startActivityWithOutDoubleClick(intent);
        }
    }

    private void closeDalogsOpened(){
        validateAlertDialog();
        validateCustomAlertDialog();
    }

    private void putUnderlineSpan(){
        SpannableString mitextoU = new SpannableString(binding.textSeeProcessingRequirements.getText());
        mitextoU.setSpan(new UnderlineSpan(), 0, mitextoU.length(), 0);
        binding.textSeeProcessingRequirements.setText(mitextoU);
    }


    @Override
    public void updateTurn() {
        validateAlertDialog();
        officeDetailViewModel.getOfficeDetail(informationOffice, true);
    }
}
