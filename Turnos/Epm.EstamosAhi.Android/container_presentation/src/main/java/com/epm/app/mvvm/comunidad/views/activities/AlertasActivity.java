package com.epm.app.mvvm.comunidad.views.activities;

import android.app.AlertDialog;
import androidx.lifecycle.ViewModelProviders;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import androidx.databinding.DataBindingUtil;
import android.os.Bundle;
import android.text.Html;

import com.epm.app.R;
import com.epm.app.databinding.ActivityAlertasBinding;
import com.epm.app.mvvm.comunidad.network.response.places.Municipio;
import com.epm.app.mvvm.comunidad.network.response.places.Sectores;
import com.epm.app.mvvm.util.Utils;
import com.epm.app.mvvm.utilAdapter.BindingAdapaters;
import com.epm.app.mvvm.comunidad.viewModel.AlertasViewModel;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.IAlertasViewModel;
import com.epm.app.view.activities.LandingActivity;

import java.util.List;

import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class AlertasActivity extends BaseActivityWithDI {



    IAlertasViewModel alertasViewModel;
    ActivityAlertasBinding binding;
    private Municipio itemMunicipioSelected;
    private Sectores itemSectorSelected;
    AlertDialog alertDialog, alertDialog2;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this, R.layout.activity_alertas);
        this.configureDagger();
        alertasViewModel = ViewModelProviders.of(this, this.viewModelFactory).get(AlertasViewModel.class);
        binding.setAlertasviewmodel((AlertasViewModel) alertasViewModel);
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_PORTRAIT);
        itemMunicipioSelected = new Municipio();
        itemSectorSelected = new Sectores();
        alertasViewModel.loadInfoProfile(getUsuario());
        createProgressDialog();
        loadDates();
        loadBinding();
        alertasViewModel.showError();
        alertasViewModel.loadMunicipio(getIntent().getParcelableArrayListExtra(Constants.MUNICIPALITIES));

    }

    private void loadDates() {
        binding.descriptionSuscription.setText(Html.fromHtml(getResources().getString(R.string.description_suscription)));
        binding.recuperationSubscription.setText(Html.fromHtml(getResources().getString(R.string.text_recuperation_subscription)));
        binding.conditions.setOnClickListener(v -> {
            binding.conditions.setEnabled(false);
            goToPrivacyPolicyActivity();
        });
        binding.recuperationSubscription.setOnClickListener(view ->
            goToRecuperationActivity());
    }

    @Override
    protected void onResume() {
        super.onResume();
        binding.conditions.setEnabled(true);
    }

    @Override
    public void onBackPressed() {
        Intent intent = new Intent(this, LandingActivity.class);
        startActivity(intent);
    }


    private void loadBinding() {
        BindingAdapaters.TextViewEnable(binding.Sector, ((AlertasViewModel) alertasViewModel).enableSector);
        BindingAdapaters.TextViewEnable(binding.Municipio, ((AlertasViewModel) alertasViewModel).enableMunicipio);
        ((AlertasViewModel) alertasViewModel).backPressed.observe(this, aBoolean -> {
            if (aBoolean != null && aBoolean) {
                onBackPressed();
            }
        });

        alertasViewModel.getProgressDialog().observe(this, aBoolean -> {
            if (alertasViewModel.getProgressDialog().getValue()) {
                showProgressDIalog(R.string.text_please_wait);
            } else {
                dismissProgressDialog();
            }
        });

        alertasViewModel.getError().observe(this, errorMessage -> {
            showAlertDialog(errorMessage.getTitle(),errorMessage.getMessage());
            alertasViewModel.getProgressDialog().setValue(false);
        });

        alertasViewModel.getResponseService().observe(this, s -> {
            if (alertasViewModel.isStatusSubscription()) {
                showAlertDialogSuccesful(alertasViewModel.getTitleResponseService().getValue(), alertasViewModel.getResponseService().getValue());
            } else {
                showAlertDialogGeneralInformationOnUiThread(alertasViewModel.getTitleResponseService().getValue(), alertasViewModel.getResponseService().getValue());
            }
            alertasViewModel.getProgressDialog().setValue(false);
        });

        ((AlertasViewModel) alertasViewModel).pushMunicipio.observe(this, aBoolean -> {
            if (aBoolean != null && aBoolean) {

                binding.Sector.setText(Constants.EMPTY_STRING);
                listMunicipio(alertasViewModel.getListMunicipio());
            }
        });

        ((AlertasViewModel) alertasViewModel).pushSector.observe(this, aBoolean -> {
            if (((AlertasViewModel) alertasViewModel).pushSector.getValue().booleanValue()) {
                listSectores(alertasViewModel.getListSector());
            }
        });
        alertasViewModel.getExpiredToken().observe(this, errorMessage -> showAlertDialogUnauthorized(errorMessage.getTitle(),errorMessage.getMessage()) );

    }

    private void listMunicipio(List list) {
        final List<Municipio> listPlaces = list;
        if (listPlaces != null) {
            String[] municipios = Utils.getMunicipiosArrayFromItemGeneralList(listPlaces);
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.setTitle("Municipio");
            builder.setItems(municipios, (dialog, which) -> {
                itemMunicipioSelected = (Municipio) list.get(which);
                binding.Municipio.setText(itemMunicipioSelected.getDescripcion());
                ((AlertasViewModel) alertasViewModel).municipio.setValue(itemMunicipioSelected.getDescripcion());
                alertasViewModel.loadSector(itemMunicipioSelected.getIdMunicipio());
                binding.Sector.setEnabled(false);
            });
            alertDialog = builder.create();
            alertDialog.show();
        }
    }

    private void listSectores(List list) {
        final List<Sectores> listPlaces = list;
        if (listPlaces != null) {
            String[] sectores = Utils.getSectoresArrayFromItemGeneralList(listPlaces);
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.setTitle("Sector");
            builder.setItems(sectores, (dialog, which) -> {
                itemSectorSelected = (Sectores) list.get(which);
                ((AlertasViewModel) alertasViewModel).sector.setValue(itemSectorSelected.getSector());
                ((AlertasViewModel) alertasViewModel).setIdSector(itemSectorSelected.getIdSector());
                binding.Sector.setText(itemSectorSelected.getSector());
            });
            alertDialog2 = builder.create();
            alertDialog2.show();
        }
    }


    private void showAlertDialogSuccesful(final String title, final String message) {
        getCustomAlertDialog().showAlertDialog(title, message, false, R.string.btn_acept, (dialog, which) -> goToDashboardAlertas(), null);
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




    private void goToDashboardAlertas() {
        Intent intent = new Intent(AlertasActivity.this, DashboardComunityAlertActivity.class);
        startActivityWithOutDoubleClick(intent);
        finishActivity();
    }

    private void goToPrivacyPolicyActivity() {
        Intent intent = new Intent(AlertasActivity.this, PrivacyPolicyActivity.class);
        startActivityWithOutDoubleClick(intent);
    }

    private void goToRecuperationActivity() {
        Intent intent = new Intent(AlertasActivity.this, RecuperationSubscriptionActitity.class);
        startActivityWithOutDoubleClick(intent);
    }


}
