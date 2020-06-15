package com.epm.app.mvvm.transactions.views;

import androidx.appcompat.widget.Toolbar;
import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.ViewModelProviders;
import androidx.recyclerview.widget.LinearLayoutManager;

import android.content.Intent;
import android.os.Bundle;

import com.epm.app.R;
import com.epm.app.databinding.ActivityTransactionListBinding;
import com.epm.app.mvvm.transactions.adapter.TransactionsRecyclerAdapter;
import com.epm.app.mvvm.transactions.models.Transaction;
import com.epm.app.mvvm.transactions.viewModel.IViewModel.ITransactionListViewModel;
import com.epm.app.mvvm.transactions.viewModel.TransactionListViewModel;

import java.util.List;

import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class TransactionListActivity extends BaseActivityWithDI implements TransactionsRecyclerAdapter.OnFastTransitionRecyclerListener {

    private Toolbar toolbarApp;
    private ActivityTransactionListBinding binding;
    private ITransactionListViewModel transactionListViewModel;
    private TransactionsRecyclerAdapter transactionsRecyclerAdapter;
    private List<Transaction> fastTransitions;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this,R.layout.activity_transaction_list);
        this.configureDagger();
        transactionListViewModel =  ViewModelProviders.of(this, this.viewModelFactory).get(TransactionListViewModel.class);
        loadDrawerLayout(R.id.generalDrawerLayout);
        toolbarApp = (Toolbar) binding.toolbarTransition;
        loadToolbar();
        listenLiveData();
        loadService();
    }


    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar(toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
    }

    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }

    private void listenLiveData() {
        transactionsRecyclerAdapter =  new TransactionsRecyclerAdapter(this,getResources());
        transactionListViewModel.getListTransaction().observe(this, this::drawDataOnScreen);
        transactionListViewModel.getExpiredToken().observe(this, errorMessage -> showAlertDialogUnauthorized(errorMessage.getTitle(),errorMessage.getMessage()) );
        transactionListViewModel.getProgressDialog().observe(this, this::handleProgress);
        transactionListViewModel.getError().observe(this, errorMessage -> {
            this.showAlertDialogTryAgain(errorMessage.getTitle(), errorMessage.getMessage());
            dismissProgressDialog();
        });
    }

    private void showAlertDialogTryAgain(final int title, final int message) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog((getUsuario()!= null &&  !getUsuario().isInvitado() && title == R.string.title_appreciated_user) ? getUsuario().getNombres() : getString(title), message, false, R.string.text_intentar, (dialogInterface, i) -> {
            loadService();
        }, R.string.text_cancelar, (dialogInterface, i) -> onBackPressed(), null));
    }


    private void loadService(){
        transactionListViewModel.fetchFastTransactions();
    }

    private void drawDataOnScreen(List<Transaction> fastTransitions) {
        this.fastTransitions = fastTransitions;
        transactionsRecyclerAdapter.setFastTransactions(fastTransitions);
        LinearLayoutManager layoutManager = new LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false);
        binding.transactionRecycler.setNestedScrollingEnabled(false);
        binding.transactionRecycler.setHasFixedSize(true);
        binding.transactionRecycler.setLayoutManager(layoutManager);
        binding.transactionRecycler.setAdapter(transactionsRecyclerAdapter);
    }

    private void handleProgress(Boolean value) {
        if (value) {
            showProgressDIalog(R.string.text_please_wait);
        } else {
            dismissProgressDialog();
        }
    }

    @Override
    public void onItemClick(int position) {
        Intent i = new Intent(this,EnterInvoiceContractActivity.class);
        i.putExtra(Constants.FAST_TRANSACTION, fastTransitions.get(position).getId());
        startActivityWithOutDoubleClick(i);
    }
}
