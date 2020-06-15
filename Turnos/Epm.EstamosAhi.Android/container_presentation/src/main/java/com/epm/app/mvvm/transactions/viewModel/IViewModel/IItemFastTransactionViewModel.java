package com.epm.app.mvvm.transactions.viewModel.IViewModel;

import android.content.res.Resources;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.transactions.models.Transaction;

public interface IItemFastTransactionViewModel {

    void setResources(Resources resources);
    void setTransation(Transaction transation);
    void drawInformation(int position );
    MutableLiveData<Transaction> getTransation();
}
