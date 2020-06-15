package com.epm.app.mvvm.procedure.views.activities;

import android.arch.lifecycle.ViewModelProviders;
import android.content.Intent;
import android.databinding.DataBindingUtil;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.Toolbar;

import com.epm.app.R;
import com.epm.app.databinding.ActivityGuideProceduresAndRequirementsBinding;
import com.epm.app.mvvm.procedure.adapter.GuideProceduresAndRequirementsCategoryRecyclerAdapter;
import com.epm.app.mvvm.procedure.models.ProcedureInformation;
import com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory.GuideProceduresAndRequirementsCategoryItem;
import com.epm.app.mvvm.procedure.viewModel.GuideProceduresAndRequirementsViewModel;

import java.util.List;
import java.util.Objects;

import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class GuideProceduresAndRequirementsActivity extends BaseActivityWithDI implements GuideProceduresAndRequirementsCategoryRecyclerAdapter.OnGuideProceduresAndRequirementsCategoryRecyclerListener {

    ActivityGuideProceduresAndRequirementsBinding binding;
    private Toolbar toolbarApp;
    GuideProceduresAndRequirementsViewModel guideProceduresAndRequirementsViewModel;
    private GuideProceduresAndRequirementsCategoryRecyclerAdapter adapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this, R.layout.activity_guide_procedures_and_requirements);
        this.configureDagger();
        guideProceduresAndRequirementsViewModel = ViewModelProviders.of(this, viewModelFactory).get(GuideProceduresAndRequirementsViewModel.class);
        loadDrawerLayout(R.id.generalDrawerLayout);
        toolbarApp = (Toolbar) binding.toolbarGuideProceduresAndRequirements;
        loadToolbar();
        loadBinding();

    }

    private void loadBinding() {
        guideProceduresAndRequirementsViewModel.showError();
        guideProceduresAndRequirementsViewModel.getProgressDialog().observe(this, aBoolean -> {
            if (aBoolean != null && aBoolean) {
                showProgressDIalog(R.string.text_please_wait);
            } else {
                dismissProgressDialog();
            }
        });
        guideProceduresAndRequirementsViewModel.getError().observe(this, errorMessage -> {
            this.showAlertDialogTryAgain(errorMessage.getTitle(), errorMessage.getMessage());
            dismissProgressDialog();
        });
        guideProceduresAndRequirementsViewModel.getExpiredToken().observe(this, aBoolean -> showAlertDialogUnauthorized(guideProceduresAndRequirementsViewModel.getError().getValue().getTitle(), guideProceduresAndRequirementsViewModel.getErrorUnauthorized()));

        loadService();
        guideProceduresAndRequirementsViewModel.getListGuideProceduresAndRequirementsCategory().observe(this, this::DrawCategory);

    }

    private void loadService() {
        guideProceduresAndRequirementsViewModel.getGuideProceduresAndRequirementsCategories();
    }

    private void showAlertDialogTryAgain(final int title, final int message) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog((!getUsuario().isInvitado() && title == R.string.title_appreciated_user) ? getUsuario().getNombres() : getString(title), message, false, R.string.text_intentar, (dialogInterface, i) -> {
            loadService();
        }, R.string.text_cancelar, (dialogInterface, i) -> onBackPressed(), null));
    }


    private void DrawCategory(List<GuideProceduresAndRequirementsCategoryItem> listGuideProceduresAndRequirementsCategory) {
        adapter = new GuideProceduresAndRequirementsCategoryRecyclerAdapter( listGuideProceduresAndRequirementsCategory, this, getResources());
        LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false);
        binding.guideProceduresAndRequirementsRecyclerView.setNestedScrollingEnabled(false);
        binding.guideProceduresAndRequirementsRecyclerView.setHasFixedSize(true);
        binding.guideProceduresAndRequirementsRecyclerView.setAdapter(adapter);
        binding.guideProceduresAndRequirementsRecyclerView.setLayoutManager(linearLayoutManager);
        binding.guideProceduresAndRequirementsRecyclerView.getAdapter();
    }

    @Override
    protected void onResume() {
        super.onResume();
        binding.guideProceduresAndRequirementsRecyclerView.setAdapter(adapter);

    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar(toolbarApp);
        Objects.requireNonNull(getSupportActionBar()).setDisplayShowTitleEnabled(false);
    }


    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }

    @Override
    public void onItemClick(int position) {
        Intent intent = new Intent(this, FrequentProceduresActivity.class);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        ProcedureInformation procedureInformation = new ProcedureInformation();
        procedureInformation.setGuideProceduresAndRequirementsCategoryItem(guideProceduresAndRequirementsViewModel.getItemGuideProceduresAndRequirementsCategory(position));
        intent.putExtra(Constants.PROCEDURE_INFORMATION, procedureInformation);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }
}
