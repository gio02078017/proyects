package com.epm.app.mvvm.procedure.views.activities;

import androidx.lifecycle.ViewModelProviders;
import android.content.Intent;
import androidx.databinding.DataBindingUtil;
import android.os.Bundle;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.appcompat.widget.Toolbar;

import com.epm.app.R;
import com.epm.app.databinding.ActivityDetailsOfTheTransactionBinding;
import com.epm.app.mvvm.procedure.adapter.DetailsOfFormalitiesGroupAdapter;
import com.epm.app.mvvm.procedure.models.ProcedureInformation;
import com.epm.app.mvvm.procedure.network.response.DetailOfTheTransactionResponse;

import com.epm.app.mvvm.procedure.views.fragment.FormalitiesCareChannelsFragment;
import com.epm.app.mvvm.turn.models.DetailOfFormalitiesGroup;
import com.epm.app.mvvm.procedure.viewModel.DetailsOfTheTransactionViewModel;
import com.epm.app.mvvm.turn.viewModel.iViewModel.IDetailsOfTheTransactionViewModel;
import com.epm.app.mvvm.turn.views.activities.NearbyOfficesActivity;
import com.epm.app.mvvm.utilAdapter.ExpandableRecyclerAdapter;
import com.epm.app.view.activities.LineasDeAtencionActivity;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class DetailsOfTheTransactionActivity extends BaseActivityWithDI implements FormalitiesCareChannelsFragment.IFormalitiesCareChannelsFragmentListener {

    ActivityDetailsOfTheTransactionBinding binding;
    IDetailsOfTheTransactionViewModel detailsOfTheTransactionViewModel;
    private Toolbar toolbarApp;
    private DetailsOfFormalitiesGroupAdapter formalitiesAdapter;
    private List<DetailOfFormalitiesGroup> listDetailOfFormalitiesGroups;
    private Bundle datosBeforeActivity;
    private ProcedureInformation procedureInformation;
    private int positionExpandable;
    private FormalitiesCareChannelsFragment formalitiesCareChannelsFragment;
    private Intent intent;
    private boolean redirect;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this,R.layout.activity_details_of_the_transaction);
        this.configureDagger();
        detailsOfTheTransactionViewModel = ViewModelProviders.of(this,viewModelFactory).get(DetailsOfTheTransactionViewModel.class);
        binding.setViewModel((DetailsOfTheTransactionViewModel) detailsOfTheTransactionViewModel);
        toolbarApp = (Toolbar) binding.toolbarDetails;
        formalitiesCareChannelsFragment = new FormalitiesCareChannelsFragment();
        loadToolbar();
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadRecycler();
        getInformationBeforeActivity();
        loadBinding();
        loadService();
    }

    private void loadBinding() {
        detailsOfTheTransactionViewModel.getProgressDialog().observe(this, aBoolean -> {
            if (aBoolean != null && aBoolean) {
                showProgressDIalog(R.string.text_please_wait);
            } else {
                dismissProgressDialog();
            }
        });
        detailsOfTheTransactionViewModel.getError().observe(this, errorMessage -> {
            dismissProgressDialog();
            this.showAlertDialogTryAgain(errorMessage.getTitle(), errorMessage.getMessage(), R.string.text_intentar, R.string.text_cancelar);
        });
        detailsOfTheTransactionViewModel.getExpiredToken().observe(this, errorMessage -> showAlertDialogUnauthorized(errorMessage.getTitle(),errorMessage.getMessage()) );
        detailsOfTheTransactionViewModel.getDetailOfFormalitiesGroups().observe(this, detailOfFormalitiesGroups -> {
            if(detailOfFormalitiesGroups != null){
                listDetailOfFormalitiesGroups = detailOfFormalitiesGroups;
                positionExpandable = validateItems();
                formalitiesAdapter.setItems(listDetailOfFormalitiesGroups,positionExpandable);
            }
        });
        detailsOfTheTransactionViewModel.getPushBottom().observe(this, idTransaction -> {
            if(idTransaction != null){
                validateRedirect(idTransaction);
            }
        });
    }

    private int validateItems(){
        return redirect ? Constants.ONE : Constants.ZERO;
    }

    private void validateRedirect(DetailOfTheTransactionResponse detail){
        if(redirect){
           onBackPressed();
        }else{
            goToFragmentCustomerService(detail);
        }
    }

    private void showAlertDialogTryAgain(final int title, final int message, final int positive, final int negative) {
        runOnUiThread(() -> {
            getCustomAlertDialog().showAlertDialog((getUsuario()!= null && !getUsuario().isInvitado() && title == R.string.title_appreciated_user) ? getUsuario().getNombres() : getString(title), message, false, positive, (dialogInterface, i) -> {
                loadService();
            }, negative, (dialogInterface, i) -> onBackPressed(), null);
        });
    }

    private void loadService(){
        detailsOfTheTransactionViewModel.loadDetailOfTheTransaction(procedureInformation,redirect);
    }

    private void loadRecycler(){
        listDetailOfFormalitiesGroups = new ArrayList<>();
        LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false){
            @Override
            public boolean canScrollVertically() {
                return false;
            }
        };
        formalitiesAdapter = new DetailsOfFormalitiesGroupAdapter(listDetailOfFormalitiesGroups);
        stateListView();
        binding.listView.setAdapter(formalitiesAdapter);
        binding.listView.setLayoutManager(linearLayoutManager);
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar(toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
    }


    private void stateListView(){
        formalitiesAdapter.setExpandCollapseListener(new ExpandableRecyclerAdapter.ExpandCollapseListener() {
            @Override
            public void onListItemExpanded(int position) {
                validateListExpandable(position);
            }

            @Override
            public void onListItemCollapsed(int position) {

            }
        });
    }

    private void validateListExpandable(int position){
        if(position != positionExpandable){
            formalitiesAdapter.collapseParent(positionExpandable);
        }
        positionExpandable = position;
    }

    private void getInformationBeforeActivity(){
        datosBeforeActivity = this.getIntent().getExtras();
        redirect = getIntent().getExtras().getBoolean(Constants.CALL_SINCE_SHIFT_INFORMATION);
        if(datosBeforeActivity.getSerializable(Constants.PROCEDURE_INFORMATION) != null){
            procedureInformation = (ProcedureInformation)datosBeforeActivity.getSerializable(Constants.PROCEDURE_INFORMATION);
        }
    }

    private void goToFragmentCustomerService(DetailOfTheTransactionResponse transactionResponse){
        if(!formalitiesCareChannelsFragment.isVisible()) {
            formalitiesCareChannelsFragment.loadService(transactionResponse);
            formalitiesCareChannelsFragment.show(getSupportFragmentManager(), "");
        }

    }


    private void goToLineAtentionActivity(){
        intent = new Intent(DetailsOfTheTransactionActivity.this, LineasDeAtencionActivity.class);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        intent.putExtra(Constants.INFORMATION_ATTENTION_LINES, true);
        startActivityWithOutDoubleClick(intent);
    }

    private void goToNearbyOfficesActivity(){
        intent = new Intent(DetailsOfTheTransactionActivity.this, NearbyOfficesActivity.class);
       // intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        intent.putExtra(Constants.PROCEDURE_INFORMATION, procedureInformation);
        startActivityWithOutDoubleClick(intent);
    }



    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }



    @Override
    public void onChoseLineAttentionClicked() {
        goToLineAtentionActivity();
    }



    @Override
    public void onChoseVisitClicked() {
        goToNearbyOfficesActivity();
    }


}
