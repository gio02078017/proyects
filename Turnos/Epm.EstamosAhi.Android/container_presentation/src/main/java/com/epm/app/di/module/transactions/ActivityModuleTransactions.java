package com.epm.app.di.module.transactions;


import com.epm.app.mvvm.transactions.views.EnterInvoiceContractActivity;
import com.epm.app.mvvm.transactions.views.TransactionListActivity;

import dagger.Module;
import dagger.android.ContributesAndroidInjector;

@Module
public abstract class ActivityModuleTransactions {

    @ContributesAndroidInjector
    abstract TransactionListActivity contributeTransactionListActivity();
    @ContributesAndroidInjector
    abstract EnterInvoiceContractActivity contributeEnterInvoiceContractActivity();

}
