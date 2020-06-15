package com.epm.app.mvvm.comunidad.views.activities;

import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProviders;
import android.content.Intent;
import androidx.databinding.DataBindingUtil;
import android.os.Bundle;
import androidx.annotation.Nullable;
import androidx.appcompat.widget.Toolbar;
import android.text.Html;
import android.util.Log;
import android.view.View;

import com.epm.app.R;
import com.epm.app.databinding.ActivityRecuperationSubscriptionActitityBinding;
import com.epm.app.mvvm.comunidad.viewModel.RecuperationSubscriptionViewModel;

import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class RecuperationSubscriptionActitity extends BaseActivityWithDI {

    ActivityRecuperationSubscriptionActitityBinding binding;
    RecuperationSubscriptionViewModel recuperationViewModel;
    private Toolbar toolbarApp;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this, R.layout.activity_recuperation_subscription_actitity);
        this.configureDagger();
        Log.e("recovery","recovery");
        recuperationViewModel =  ViewModelProviders.of(this,viewModelFactory).get(RecuperationSubscriptionViewModel.class);
        binding.setRecuperationViewModel((RecuperationSubscriptionViewModel) recuperationViewModel);
        createProgressDialog();
        showError();
        loadViewModel();
    }

    private void showError() {
        recuperationViewModel.showError();
        recuperationViewModel.getError().observe(this, errorMessage -> {
            showAlertDialog(errorMessage.getTitle(),errorMessage.getMessage());
            recuperationViewModel.getProgressDialog().setValue(false);
        });
       recuperationViewModel.getProgressDialog().observe(this, aBoolean -> {
           if(aBoolean != null && aBoolean){
               showProgressDIalog(R.string.text_please_wait);
           }else{
               dismissProgressDialog();
           }
       });
    }

    private void loadViewModel(){

        binding.descriptionSubscription.setText(Html.fromHtml(getResources().getString(R.string.description_recuperation_subscription)));
        binding.getRecuperationViewModel().getMessageEmptyParameters().observe(this,type->{
            alertError(getResources().getString(R.string.title_empty_fields),getResources().getString(R.string.text_empty_email));
        });
        binding.getRecuperationViewModel().getMessageInvalidEmail().observe(this,type->{
            alertError(getResources().getString(R.string.title_empty_fields),getResources().getString(R.string.text_valid_email));
        });

        binding.getRecuperationViewModel().getMessageNotFoundEmail().observe(this,message->{
            alertError(message.getTitulo(),message.getContenido());
        });

        binding.getRecuperationViewModel().getIsUpdateSubscription().observe(this,o->{
            Intent intent = new Intent(this ,UpdateSubscriptionActivity.class);
            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
            startActivity(intent);

        });


        binding.getRecuperationViewModel().getMessageSuccess().observe(this,recoverySubscriptionResponse->{
            alertSuccess(recoverySubscriptionResponse.getMensaje().getTitulo(),recoverySubscriptionResponse.getMensaje().getContenido(),recoverySubscriptionResponse.getSuscripcionAlertasItuango().getCorreoElectronico());
        });

        binding.getRecuperationViewModel().getExpiredToken().observe(this, errorMessage -> showAlertDialogUnauthorized(errorMessage.getTitle(),errorMessage.getMessage()) );

    }



    private void showAlertDialog(final int title, final int message) {
        runOnUiThread(() -> {
            if (!getUsuario().isInvitado() && title == R.string.title_appreciated_user) {
                showAlertDialogGeneralInformationOnUiThread(getUsuario().getNombres(), message);
            } else {
                showAlertDialogGeneralInformationOnUiThread(title, message);
            }
        });

    }

    public void alertError(String title, String message){

        runOnUiThread(() -> {
        getCustomAlertDialog().showAlertDialog(
                title,
                message,
                true,
                app.epm.com.contacto_transparente_presentation.R.string.text_aceptar,
                (dialog, which) -> dialog.dismiss(),
                null);
        });
    }

    public void alertSuccess(String title, String message, String correoElectronico){
        runOnUiThread(() -> {

            getCustomAlertDialog().showAlertDialog(
                    title,
                    message,
                    true,
                    app.epm.com.contacto_transparente_presentation.R.string.text_aceptar,
                    (dialog, which) -> recuperationViewModel.updateSubscriptionDeviceAlertsByEmail(correoElectronico),
                    com.epm.app.app_utilities_presentation.R.string.text_cancelar,
                    (dialog, which) -> dialog.dismiss(), null);
        });
    }

    public void closeRecoverySubscriptionActivity(View view){
        finish();
    }



}
