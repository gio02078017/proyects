package com.epm.app.mvvm.procedure.views.activities;

import androidx.lifecycle.ViewModelProviders;
import android.content.Intent;
import androidx.databinding.DataBindingUtil;
import android.os.Bundle;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.appcompat.widget.Toolbar;
import com.epm.app.R;
import com.epm.app.databinding.ActivityFrequentProceduresBinding;
import com.epm.app.mvvm.procedure.adapter.FrequentProceduresAdapter;
import com.epm.app.mvvm.procedure.models.ProcedureInformation;
import com.epm.app.mvvm.procedure.network.request.ProcedureRequest;
import com.epm.app.mvvm.procedure.network.response.Procedure;
import com.epm.app.mvvm.procedure.viewModel.FrequentProceduresViewModel;
import com.epm.app.mvvm.procedure.viewModel.iViewModel.IFrequentProceduresViewModel;
import java.util.List;
import java.util.Objects;

import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class FrequentProceduresActivity extends BaseActivityWithDI implements FrequentProceduresAdapter.OnFrequentProceduresRecyclerListener {

    private IFrequentProceduresViewModel frequentProceduresViewModel;
    private FrequentProceduresAdapter frequentProceduresAdapter;
    private ActivityFrequentProceduresBinding binding;
    private Toolbar toolbarApp;
    private ProcedureInformation procedureInformation;
    private Bundle datosBeforeActivity;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this,R.layout.activity_frequent_procedures);
        this.configureDagger();
        frequentProceduresViewModel = ViewModelProviders.of(this, this.viewModelFactory).get(FrequentProceduresViewModel.class);
        loadDrawerLayout(R.id.generalDrawerLayout);
        toolbarApp = (Toolbar) binding.toolbarFrequentProcedures;
        loadToolbar();
        listenLiveData();
        getInformationBeforeActivity();
    }

    private void getInformationBeforeActivity(){
        datosBeforeActivity = this.getIntent().getExtras();
        if(datosBeforeActivity.getSerializable(Constants.PROCEDURE_INFORMATION) != null){
            procedureInformation = (ProcedureInformation)datosBeforeActivity.getSerializable(Constants.PROCEDURE_INFORMATION);
            loadService();
        }
    }

    private void listenLiveData() {
        frequentProceduresAdapter =  new FrequentProceduresAdapter(this);
        frequentProceduresViewModel.getListProcedure().observe(this, this::drawDataOnScreen);
        frequentProceduresViewModel.getExpiredToken().observe(this, errorMessage -> showAlertDialogUnauthorized(errorMessage.getTitle(),errorMessage.getMessage()) );
        frequentProceduresViewModel.getProgressDialog().observe(this, this::handleProgress);
        frequentProceduresViewModel.getError().observe(this, errorMessage -> {
            this.showAlertDialogTryAgain(errorMessage.getTitle(), errorMessage.getMessage());
            dismissProgressDialog();
        });
    }

    private void handleProgress(Boolean value) {
        if (value) {
            showProgressDIalog(R.string.text_please_wait);
        } else {
            dismissProgressDialog();
        }
    }

    private void showAlertDialogTryAgain(final int title, final int message) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog((getUsuario()!= null &&  !getUsuario().isInvitado() && title == R.string.title_appreciated_user) ? getUsuario().getNombres() : getString(title), message, false, R.string.text_intentar, (dialogInterface, i) -> {
            loadService();
        }, R.string.text_cancelar, (dialogInterface, i) -> onBackPressed(), null));
    }

    private void loadService(){
        frequentProceduresViewModel.fetchFrequentProcedures(new ProcedureRequest(getCategoryId()));
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar( toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
    }

    private String getCategoryId() {
        return Objects.requireNonNull(procedureInformation.getIdService());
    }

    private void drawDataOnScreen(List<Procedure> frequentProcedures) {
        frequentProceduresAdapter.setFrequentProceduresList(frequentProcedures);
        LinearLayoutManager layoutManager = new LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false);
        binding.recyclerFrequentProcedures.setNestedScrollingEnabled(false);
        binding.recyclerFrequentProcedures.setHasFixedSize(true);
        binding.recyclerFrequentProcedures.setLayoutManager(layoutManager);
        binding.recyclerFrequentProcedures.setAdapter(frequentProceduresAdapter);
    }

    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }


    @Override
    public void onItemClick(int position) {
        Intent i = new Intent(this,TypePersonActivity.class);
       // i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        procedureInformation.setIdProcedure(frequentProceduresViewModel.getIdProcedure(position));
        procedureInformation.setNameProcedure(frequentProceduresViewModel.getNameProcedure(position));
        i.putExtra(Constants.PROCEDURE_INFORMATION, procedureInformation);
        startActivityWithOutDoubleClick(i);
    }
}
