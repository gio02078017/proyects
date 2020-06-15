package com.epm.app.di.module.transactions;

import com.epm.app.mvvm.transactions.network.TransactionServices;
import com.epm.app.mvvm.transactions.repository.TransactionRepository;

import app.epm.com.utilities.helpers.ValidateInternet;
import dagger.Module;
import dagger.Provides;

@Module(includes = ViewModelModuleTransactions.class)
public class RespositoryModuleTransactions {

    @Provides
    public TransactionRepository providerNotificationsRepository(TransactionServices notificationsServices){
        return new TransactionRepository(notificationsServices);
    }

}
