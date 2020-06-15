package com.epm.app.mvvm.procedure.views.activities;

import android.arch.lifecycle.ViewModelProviders;
import android.content.Intent;
import android.databinding.DataBindingUtil;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.Toolbar;

import com.epm.app.R;
import com.epm.app.databinding.ActivityTypePersonBinding;
import com.epm.app.mvvm.procedure.adapter.TypePersonRecyclerAdapter;
import com.epm.app.mvvm.procedure.models.ProcedureInformation;
import com.epm.app.mvvm.procedure.network.request.TypePersonRequest;
import com.epm.app.mvvm.procedure.network.response.TypePerson.TypePersonItem;
import com.epm.app.mvvm.procedure.viewModel.TypePersonViewModel;
import com.epm.app.mvvm.procedure.viewModel.iViewModel.ITypePersonViewModel;
import com.epm.app.mvvm.turn.views.activities.DetailsOfTheTransactionActivity;

import java.util.List;
import java.util.Objects;

import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class TypePersonActivity extends BaseActivityWithDI implements TypePersonRecyclerAdapter.OnTypePersonRecyclerListener {

    private ITypePersonViewModel typePersonViewModel;
    private TypePersonRecyclerAdapter typePersonRecyclerAdapter;
    private ActivityTypePersonBinding binding;
    private Toolbar toolbarApp;
    private Bundle datosBeforeActivity;
    private ProcedureInformation procedureInformation;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this,R.layout.activity_type_person);
        this.configureDagger();
        loadDrawerLayout(R.id.generalDrawerLayout);
        toolbarApp = (Toolbar) binding.toolbarTypePerson;
        loadToolbar();
        typePersonViewModel = ViewModelProviders.of(this, this.viewModelFactory).get(TypePersonViewModel.class);
        loadBinding();
        getInformationBeforeActivity();
    }

    @Override
    protected void onResume() {
        super.onResume();
        binding.recyclerTypePerson.setAdapter(typePersonRecyclerAdapter);

    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
    }
    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar( toolbarApp);
        Objects.requireNonNull(getSupportActionBar()).setDisplayShowTitleEnabled(false);
    }

    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }

    private void getInformationBeforeActivity(){
        datosBeforeActivity = this.getIntent().getExtras();
        if(datosBeforeActivity.getSerializable(Constants.PROCEDURE_INFORMATION) != null){
            procedureInformation = (ProcedureInformation)datosBeforeActivity.getSerializable(Constants.PROCEDURE_INFORMATION);
            binding.textTypeProcedure.setText(procedureInformation.getProcedure().getName());
            loadService();
        }
    }

    private void loadService(){
        typePersonViewModel.fetchTypePerson(new TypePersonRequest(procedureInformation.getGuideProceduresAndRequirementsCategoryItem().getCategoryId(), procedureInformation.getProcedure().getId()));
    }

    private void loadBinding() {
        typePersonViewModel.getExpiredToken().observe(this, aBoolean -> showAlertDialogUnauthorized(typePersonViewModel.getError().getValue().getTitle(), typePersonViewModel.getErrorUnauthorized()));
        typePersonViewModel.getProgressDialog().observe(this, this::handleProgress);
        typePersonViewModel.getError().observe(this, errorMessage -> {
            this.showAlertDialogTryAgain(errorMessage.getTitle(), errorMessage.getMessage());
            dismissProgressDialog();
        });
        typePersonViewModel.getListTypePerson().observe(this, this::drawDataOnScreen);
    }

    private void handleProgress(Boolean value) {
        if (value) {
            showProgressDIalog(R.string.text_please_wait);
        } else {
            dismissProgressDialog();
        }
    }

    private void showAlertDialogTryAgain(final int title, final int message) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog((!getUsuario().isInvitado() && title == R.string.title_appreciated_user) ? getUsuario().getNombres() : getString(title), message, false, R.string.text_intentar, (dialogInterface, i) -> {
            loadService();
        }, R.string.text_cancelar, (dialogInterface, i) -> onBackPressed(), null));
    }

    private void drawDataOnScreen(List<TypePersonItem> listTypePerson) {
        typePersonRecyclerAdapter = new TypePersonRecyclerAdapter(this,listTypePerson,this,getResources());
        LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false);
        binding.recyclerTypePerson.setNestedScrollingEnabled(false);
        binding.recyclerTypePerson.setHasFixedSize(true);
        binding.recyclerTypePerson.setAdapter(typePersonRecyclerAdapter);
        binding.recyclerTypePerson.setLayoutManager(linearLayoutManager);
        binding.recyclerTypePerson.getAdapter();
    }


    @Override
    public void onItemClick(int position) {
        Intent i = new Intent(this, DetailsOfTheTransactionActivity.class);
        i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        procedureInformation.setTypePersonItem(typePersonViewModel.getTypePersonItem(position));
        i.putExtra(Constants.PROCEDURE_INFORMATION, procedureInformation);
        startActivityForResult(i,Constants.DEFAUL_REQUEST_CODE);
    }
}
