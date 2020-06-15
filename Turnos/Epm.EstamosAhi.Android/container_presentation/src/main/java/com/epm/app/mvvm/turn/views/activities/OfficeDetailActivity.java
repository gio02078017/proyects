package com.epm.app.mvvm.turn.views.activities;

import androidx.lifecycle.ViewModelProviders;
import android.content.DialogInterface;
import android.content.Intent;
import androidx.databinding.DataBindingUtil;
import android.os.Bundle;
import android.os.Handler;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.widget.Toolbar;
import android.text.SpannableString;
import android.text.Spanned;
import android.text.style.UnderlineSpan;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;
import com.epm.app.R;
import com.epm.app.databinding.ActivityOfficeDetailBinding;
import com.epm.app.mvvm.procedure.models.ProcedureInformation;
import com.epm.app.mvvm.turn.network.response.askTurn.TurnResponse;
import com.epm.app.mvvm.turn.viewModel.OfficeDetailViewModel;
import com.epm.app.mvvm.turn.viewModel.AskTurnViewModel;
import com.epm.app.view.activities.LandingActivity;

import app.epm.com.utilities.helpers.InformationOffice;

import java.util.Timer;
import java.util.TimerTask;

import app.epm.com.utilities.helpers.UtilitiesDate;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.ConvertUtilities;

public class OfficeDetailActivity extends BaseOfficeDetailActivity {

    ActivityOfficeDetailBinding binding;
    private Toolbar toolbarApp;
    OfficeDetailViewModel officeDetailViewModel;
    AskTurnViewModel askTurnViewModel;
    private TimerTask scanTask;
    private final Handler handler = new Handler();
    private Timer t = new Timer();
    private InformationOffice informationOffice;
    private boolean messageProblem = false;
    private boolean validateNameClient = false;
    private String nameClient = "";
    private int viewModelError;
    private AlertDialog askForATurnDialog;
    private EditText edTextName;
    private int currentTiempoDeEsperaDetalleOficina;
    private boolean maxTiempoDeEsperaDetalleOficina = false;
    private Bundle datosBeforeActivity;
    private ProcedureInformation procedureInformation;
    private boolean oneClick = true;
    private boolean successService;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this, R.layout.activity_office_detail);
        this.configureDagger();
        officeDetailViewModel = ViewModelProviders.of(this, viewModelFactory).get(OfficeDetailViewModel.class);
        askTurnViewModel = ViewModelProviders.of(this, viewModelFactory).get(AskTurnViewModel.class);
        binding.setOfficeDetailViewModel((OfficeDetailViewModel) officeDetailViewModel);
        loadDrawerLayout(R.id.generalDrawerLayout);
        toolbarApp = (Toolbar) binding.toolbarCustomerServiceMenuOption;
        setUnderlineSpan();
        loadToolbar();
        loadBinding();
        loadBindingAskTurn();
        datosBeforeActivity = this.getIntent().getExtras();
        startMap(binding.ubicacionMapView, 75, true);
        validateInternetToLoadMap();
    }


    protected void initInformation() {
        if (datosBeforeActivity.getSerializable(Constants.INFORMATION_OFFICE) != null) {
            informationOffice = (InformationOffice) datosBeforeActivity.getSerializable(Constants.INFORMATION_OFFICE);
            binding.textNameOffice.setText(informationOffice.getNombreOficina());
            loadLocation(informationOffice);
        }
        if(datosBeforeActivity.getSerializable(Constants.PROCEDURE_INFORMATION) != null){
            procedureInformation = (ProcedureInformation)datosBeforeActivity.getSerializable(Constants.PROCEDURE_INFORMATION);
        }
    }

    protected void successValidateGPS() {
        callDetailViewModel();
    }

    private void loadBinding() {
        officeDetailViewModel.showError();
        officeDetailViewModel.getProgressDialog().observe(this, aBoolean -> {
            if (aBoolean != null && aBoolean) {
                showProgressDIalog(R.string.text_please_wait);
            } else {
                dismissProgressDialog();
            }
        });
        officeDetailViewModel.getError().observe(this, errorMessage -> {
            dismissProgressDialog();
            stopScan();
            viewModelError = Constants.DETAIL_OFFICE_VIEW_MODEL;
            this.showAlertDialogTryAgain(getString(errorMessage.getTitle()), getString(errorMessage.getMessage()), R.string.text_intentar, R.string.text_cancelar);

        });
        officeDetailViewModel.getExpiredToken().observe(this, errorMessage ->{
            stopScan();
            showAlertDialogUnauthorized(errorMessage.getTitle(),errorMessage.getMessage(),R.string.accept_button_text);
        });

        officeDetailViewModel.getProblemsOfficeDetail().observe(this, problems -> {
            if (problems && !messageProblem) {
                showAlertDialog(officeDetailViewModel.getOfficeDetailResponse().getMensaje().getTitulo(), officeDetailViewModel.getOfficeDetailResponse().getMensaje().getContenido());
                messageProblem = true;
            }
            doRefresh();

        });

        officeDetailViewModel.getShiftInSameoffice().observe(this, result -> {
            callActivityShiftinformation(null);
        });

        officeDetailViewModel.getSuccessOfficeDetail().observe(this, success -> {
            if (success != null && success) {
                doRefresh();
                startLocation();
            }
        });

        officeDetailViewModel.getChangeButtonAskTurn().observe(this, changeStateButton -> {
            changeStateButton(changeStateButton);
        });


    }

    private void loadBindingAskTurn() {
        askTurnViewModel.getProgressDialog().observe(this, aBoolean -> {
            if (aBoolean != null && aBoolean) {
                showProgressDIalog(R.string.text_please_wait);
            } else {
                dismissProgressDialog();
            }
        });
        askTurnViewModel.getError().observe(this, errorMessage -> {
            dismissProgressDialog();
            viewModelError = Constants.ASK_TURN_VIEW_MODEL;
            this.showAlertDialogTryAgain(getString(errorMessage.getTitle()), getString(errorMessage.getMessage()), R.string.text_intentar, R.string.text_cancelar);
        });
        askTurnViewModel.getExpiredToken().observe(this, errorMessage ->{
            showAlertDialogUnauthorized(errorMessage.getTitle(),errorMessage.getMessage(),R.string.accept_button_text);
        } );

        askTurnViewModel.getSuccessAskTurn().observe(this, success -> {
            callActivityShiftinformation(askTurnViewModel.getTurnResponse());
        });

        askTurnViewModel.getProblemsAskTurn().observe(this, problems -> {
            if (problems) {
                showAlertDialog(askTurnViewModel.getTurnResponse().getMensaje().getTitulo(), askTurnViewModel.getTurnResponse().getMensaje().getContenido());
            }
        });
    }

    protected void callDetailViewModel() {
        if (informationOffice != null) {
            officeDetailViewModel.getOfficeDetail(informationOffice, false);
        }
    }

    @Override
    protected void onResume() {
        super.onResume();
        binding.ubicacionMapView.resume();
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

    @Override
    public void onBackPressed() {
        super.onBackPressed();
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

    private void showAlertDialogTryAgain(final String title, final String message, final int positive, final int negative) {
        runOnUiThread(() -> {
            getCustomAlertDialog().showAlertDialog((getUsuario()!= null &&  !getUsuario().isInvitado() && title.equalsIgnoreCase(getString(R.string.title_appreciated_user))) ? getUsuario().getNombres(): title, message, false, positive, (dialogInterface, i) -> {
                callService();
            }, negative, (dialogInterface, i) -> cancelDialogError(dialogInterface) , null);
        });
    }

    private void cancelDialogError(DialogInterface dialogInterface) {
        dialogInterface.dismiss();
        if (viewModelError == Constants.DETAIL_OFFICE_VIEW_MODEL) {
            onBackPressed();
        }
    }

    private void callService() {
        if (viewModelError == Constants.DETAIL_OFFICE_VIEW_MODEL) {
            callDetailViewModel();
        } else if (viewModelError == Constants.ASK_TURN_VIEW_MODEL) {
            callAskTurn();
        }
    }

    private void changeStateButton(boolean stated) {
        if (stated) {
            statedEnabled();
        } else {
            statedDisabled();
        }
    }

    private void statedDisabled() {
        binding.btnAskTurn.setBackgroundResource(R.color.gray_line_separator);
        binding.btnAskTurn.setEnabled(false);
    }

    private void statedEnabled() {
        binding.btnAskTurn.setBackgroundResource(R.color.pageIndicator);
        binding.btnAskTurn.setEnabled(true);
    }

    public void doRefresh() {

        if (informationOffice != null && !informationOffice.getPuntoFacil()) {
            scanTask = new TimerTask() {
                public void run() {
                    handler.post(() -> {
                        Log.d("TIMER", "Timer set off");
                        currentTiempoDeEsperaDetalleOficina += officeDetailViewModel.getOfficeDetailResponse().getTiempoParaRefrescarDetalleOficina();
                        validateMaxTiempoDeEsperaDetalleOficina();
                    });
                }
            };

            t.schedule(scanTask, ConvertUtilities.convertFromMinutesToMilliseconds(officeDetailViewModel.getOfficeDetailResponse().getTiempoParaRefrescarDetalleOficina()));
        }
    }

    public void validateMaxTiempoDeEsperaDetalleOficina() {
        Log.d("MaxTiempoDeEspera", "" + currentTiempoDeEsperaDetalleOficina);
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
            showAlertDialogTryAgain(getName(), getString(R.string.text_validate_internet), R.string.text_intentar, R.string.text_cancelar);
        }
    }

    public void canDoZoom(){
        setItDoZoom(true);
        startLocation();
    }

    public void stopScan() {
        if (scanTask != null) {
            Log.d("TIMER", "timer canceled");
            scanTask.cancel();
        }
    }

    protected void cancelPermisonGPSBaseOffice(boolean defaultLocation) {
        cancelPermisonGPS(defaultLocation);
    }

    public void cancelPermisonGPS(boolean defaultLocation) {
        pointCenter(informationOffice);
        callDetailViewModel();
    }

    public void eventAskTurn(View view) {
        callAskTurn();
    }

    public void eventRefreshLocation(View view) {

        itDoHandLocation(true);
    }

    private void callAskTurn() {
        if (askForATurnDialog != null) {
            askForATurnDialog.dismiss();
        }
        if (!validateNameClient) {
            dialogAskForATurn();
        } else {
            askTurnViewModel.askTurn(nameClient, officeDetailViewModel.getOfficeDetailResponse().getOficina(), getUsuario(), procedureInformation);
        }
    }

    private void callOnlyAskTurn() {
        if (askForATurnDialog != null) {
            askForATurnDialog.dismiss();
        }

        askTurnViewModel.askTurn(nameClient, officeDetailViewModel.getOfficeDetailResponse().getOficina(),getUsuario(), procedureInformation);

    }

    public void dialogAskForATurn() {
        LayoutInflater factory = LayoutInflater.from(this);
        final View askForATurnDialogView = factory.inflate(R.layout.dialog_ask_for_a_turn, null);
        askForATurnDialog = new AlertDialog.Builder(this).create();
        askForATurnDialog.setView(askForATurnDialogView);
        edTextName = askForATurnDialogView.findViewById(R.id.edTextName);
        edTextName.setVisibility(getUsuario().isInvitado() ? View.VISIBLE : View.GONE);
        edTextName.setText(getUsuario().isInvitado() ? "" : getUsuario().getNombres());
        TextView textMessage = askForATurnDialogView.findViewById(R.id.textMessage);
        TextView textAskName = askForATurnDialogView.findViewById(R.id.textAskName);
        textAskName.setText(getUsuario().isInvitado() ? getString(R.string.text_do_you_ask_your_name) : String.format(getString(R.string.text_hello_name), getUsuario().getNombres()));
        textMessage.setText(String.format(getString(R.string.text_message_dialog_ask_for_a_turn), officeDetailViewModel.getOfficeDetailResponse().getOficina().getHoraCierre()));

        askForATurnDialogView.findViewById(R.id.btnAskTurn).setOnClickListener(view -> {
            askForATurnDialog.dismiss();
            if (!edTextName.getText().toString().trim().equalsIgnoreCase("")) {
                nameClient = edTextName.getText().toString();
                validateNameClient = true;
            }
            callOnlyAskTurn();
        });

        askForATurnDialogView.findViewById(R.id.btnCancel).setOnClickListener(view -> askForATurnDialog.dismiss());

        askForATurnDialog.show();

    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == Constants.ACTION_TO_TURN_ON_GPS) {
            loadLocation(informationOffice);
        }
        super.onActivityResult(requestCode, resultCode, data);
    }

    public void callActivityShiftinformation(TurnResponse turnResponse) {
        Intent intent = new Intent(OfficeDetailActivity.this, ShiftInformationActivity.class);
        intent.putExtra(Constants.INFORMATION_OFFICE, informationOffice);
        if (turnResponse != null) {
            turnResponse.setShowTurn(false);
            saveShiftInformationInPreferences(turnResponse);
            intent.putExtra(Constants.SHIFT_INFORMATION, turnResponse);
            informationOffice.setTurnDate(UtilitiesDate.getDate());
            intent.putExtra(Constants.INFORMATION_OFFICE, informationOffice);
            AddProcedureInformation(intent);
        }
        finish();
        startActivityWithOutDoubleClick(intent);
    }

    public void AddProcedureInformation(Intent intent){
        if(procedureInformation != null){
            intent.putExtra(Constants.PROCEDURE_INFORMATION, procedureInformation);
        }
    }

    private void saveShiftInformationInPreferences(TurnResponse turnResponse) {
        customSharedPreferences.addString(Constants.ASSIGNED_TRUN, turnResponse.getTurnoAsignado());
    }


    public void generalPositiveAction() {
        if (maxTiempoDeEsperaDetalleOficina) {
            onBackPressed();
            maxTiempoDeEsperaDetalleOficina = false;
        }
    }

    private void setUnderlineSpan() {
        SpannableString spannableStr = new SpannableString(getString(R.string.text_see_processing_requirements));
        UnderlineSpan underlineSpan = new UnderlineSpan();
        spannableStr.setSpan(underlineSpan, 0, spannableStr.length(), Spanned.SPAN_INCLUSIVE_EXCLUSIVE);
        binding.textSeeProcessingRequirements.setText(spannableStr);
    }

    public void eventGoToOffice(View view) {
        if (isOneClick()) {
            eventGoToOffice();
        }
    }

    public boolean isOneClick() {
        return oneClick;
    }

    public void setOneClick(boolean oneClick) {
        this.oneClick = oneClick;
    }

    private void closeDalogsOpened(){
        validateAlertDialog();
        validateCustomAlertDialog();
    }
}