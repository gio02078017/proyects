package com.epm.app.mvvm.transactions.viewModel;

import android.content.res.Resources;
import android.graphics.drawable.Drawable;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.epm.app.R;
import com.epm.app.mvvm.transactions.models.Transaction;
import com.epm.app.mvvm.transactions.viewModel.IViewModel.IItemFastTransactionViewModel;

public class ItemFastTransitionViewModel extends ViewModel implements IItemFastTransactionViewModel {

    private MutableLiveData<Transaction> transationMutable;
    public final MutableLiveData<Drawable> image;
    private Resources resources;

    public ItemFastTransitionViewModel() {
        this.transationMutable =new  MutableLiveData();
        this.image = new MutableLiveData<>();
    }

    @Override
    public void setResources(Resources resources) {
        this.resources = resources;
    }

    @Override
    public MutableLiveData<Transaction> getTransation() {
        return transationMutable;
    }

    @Override
    public void setTransation(Transaction transation) {
        this.transationMutable.setValue(transation);
    }

    @Override
    public void drawInformation(int position ) {
        if (position == 0){
            this.image.setValue(resources.getDrawable(R.mipmap.icon_duplicado_factura));
        }else if (position == 1){
            this.image.setValue(resources.getDrawable(R.mipmap.icon_saldo_factura));

        }
    }
}
