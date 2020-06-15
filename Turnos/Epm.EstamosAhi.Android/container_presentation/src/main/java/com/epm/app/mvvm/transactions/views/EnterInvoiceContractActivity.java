package com.epm.app.mvvm.transactions.views;

import androidx.appcompat.widget.Toolbar;
import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.ViewModelProviders;

import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;

import com.epm.app.R;
import com.epm.app.databinding.ActivityEnterInvoiceContractBinding;
import com.epm.app.mvvm.transactions.viewModel.EnterInvoiceContractViewModel;
import com.epm.app.mvvm.transactions.viewModel.IViewModel.IEnterInvoiceContractViewModel;

import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class EnterInvoiceContractActivity extends BaseActivityWithDI implements TextWatcher {


    ActivityEnterInvoiceContractBinding binding;
    IEnterInvoiceContractViewModel enterInvoiceContractViewModel;
    private Toolbar toolbarApp;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this,R.layout.activity_enter_invoice_contract);
        this.configureDagger();
        enterInvoiceContractViewModel = ViewModelProviders.of(this,viewModelFactory).get(EnterInvoiceContractViewModel.class);
        binding.setViewModel((EnterInvoiceContractViewModel) enterInvoiceContractViewModel);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        loadToolbar();
        binding.inputContract.addTextChangedListener(this);
    }

    private void loadToolbar() {
        toolbarApp = (Toolbar) binding.toolbarContract;
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar((Toolbar) toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
    }

    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }

    @Override
    public void beforeTextChanged(CharSequence charSequence, int i, int i1, int i2) {

    }

    @Override
    public void onTextChanged(CharSequence charSequence, int i, int i1, int i2) {
        validateTextChanged(charSequence);
    }

    @Override
    public void afterTextChanged(Editable editable) {

    }

    private void validateTextChanged(CharSequence text){
        if(text.length() >= 2 ){
            binding.downloadPDF.setEnabled(true);
        }else {
            binding.downloadPDF.setEnabled(false);
        }
    }

}
