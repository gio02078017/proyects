package com.epm.app.di.module.transactions;

import androidx.lifecycle.ViewModel;
import androidx.lifecycle.ViewModelProvider;

import com.epm.app.di.util.ViewModelKey;
import com.epm.app.mvvm.comunidad.viewModel.FactoryViewModel;
import com.epm.app.mvvm.transactions.viewModel.EnterInvoiceContractViewModel;
import com.epm.app.mvvm.transactions.viewModel.TransactionListViewModel;

import dagger.Binds;
import dagger.Module;
import dagger.multibindings.IntoMap;

@Module
public abstract class ViewModelModuleTransactions {

    @Binds
    @IntoMap
    @ViewModelKey(TransactionListViewModel.class)
    abstract ViewModel bindTransactionListViewModel(TransactionListViewModel transactionListViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(EnterInvoiceContractViewModel.class)
    abstract ViewModel bindEnterInvoiceContractViewModel(EnterInvoiceContractViewModel enterInvoiceContractViewModel);



}
