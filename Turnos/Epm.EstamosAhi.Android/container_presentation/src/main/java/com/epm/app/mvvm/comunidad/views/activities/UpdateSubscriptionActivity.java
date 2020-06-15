package com.epm.app.mvvm.comunidad.views.activities;

import android.app.AlertDialog;
import androidx.lifecycle.ViewModelProviders;
import android.content.Intent;
import androidx.databinding.DataBindingUtil;
import android.os.Bundle;
import androidx.appcompat.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import com.epm.app.R;
import com.epm.app.databinding.ActivityUpdateSubscriptionBinding;
import com.epm.app.mvvm.comunidad.network.response.places.Municipio;
import com.epm.app.mvvm.comunidad.network.response.places.Sectores;
import com.epm.app.mvvm.comunidad.viewModel.NotificationCenterViewModel;
import com.epm.app.mvvm.comunidad.viewModel.UpdateSubscriptionViewModel;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.INotificationCenterViewModel;
import com.epm.app.mvvm.util.Utils;

import java.util.List;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class UpdateSubscriptionActivity extends BaseActivityWithDI {



    private Municipio itemMunicipioSelected;
    private Sectores itemSectorSelected;
    AlertDialog alertDialog, alertDialog2;
    UpdateSubscriptionViewModel updateSubscriptionViewModel;
    INotificationCenterViewModel notificationCenterViewModel;
    private Toolbar toolbarApp;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        ActivityUpdateSubscriptionBinding binding = DataBindingUtil.setContentView(this, R.layout.activity_update_subscription);
        this.configureDagger();
        updateSubscriptionViewModel = ViewModelProviders.of(this, viewModelFactory).get(UpdateSubscriptionViewModel.class);
        notificationCenterViewModel = ViewModelProviders.of(this,viewModelFactory).get(NotificationCenterViewModel.class);
        binding.setUpdateViewModel(updateSubscriptionViewModel);
        loadDrawerLayout(R.id.menuDrawer);
        createProgressDialog();
        updateSubscriptionViewModel.getSuscription();
        loadBinding();

    }

    private void loadBinding() {
        updateSubscriptionViewModel.showError();
        updateSubscriptionViewModel.getProgressDialog().observe(this, aBoolean -> {
            if (aBoolean != null && aBoolean) {
                showProgressDIalog(R.string.text_please_wait);
            } else {
                dismissProgressDialog();
            }
        });
        updateSubscriptionViewModel.getError().observe(this, errorMessage -> {
            if(!updateSubscriptionViewModel.isValidateMessage()){
                this.showAlertDialogTryAgain(errorMessage.getTitle(),errorMessage.getMessage(), R.string.text_intentar, R.string.text_cancelar);
            }else {
                this.showAlertDialog(errorMessage.getTitle(),errorMessage.getMessage());
            }
            dismissProgressDialog();
        });
         updateSubscriptionViewModel.pushMunicipio.observe(this, aBoolean -> {
            if (aBoolean != null && aBoolean) {
                listMunicipio(updateSubscriptionViewModel.getListMunicipio());
            }
        });
         updateSubscriptionViewModel.pushSector.observe(this, aBoolean -> {
            if ( updateSubscriptionViewModel.pushSector.getValue().booleanValue()) {
                listSectores(updateSubscriptionViewModel.getListSector());
            }
        });
        updateSubscriptionViewModel.getErrorMessage().observe(this, s -> {
            showAlertDialogGeneralInformationOnUiThread(updateSubscriptionViewModel.getTitleMessage().getValue(), s);
            dismissProgressDialog();
        });
        updateSubscriptionViewModel.getMessageSuccesful().observe(this, s -> {
            showAlertDialogSuccesful(updateSubscriptionViewModel.getTitleMessage().getValue(), updateSubscriptionViewModel.getMessageSuccesful().getValue());
            dismissProgressDialog();
        });
        updateSubscriptionViewModel.getExpiredToken().observe(this, errorMessage -> showAlertDialogUnauthorized(errorMessage.getTitle(),errorMessage.getMessage()) );
         updateSubscriptionViewModel.getActionMessageCancelSubscription().observe(this,mensaje ->{
            showAlertDialogCancelSubscritionSuccesful(mensaje.getTitulo(), mensaje.getContenido());
            notificationCenterViewModel.getNotificationsPush();
            dismissProgressDialog();
        });
        updateSubscriptionViewModel.getActionMessageRecoverySubscription().observe(this, mensaje ->{
            showAlertDialogRecoverySubscritionSuccesful(mensaje.getTitulo(), mensaje.getContenido());
            notificationCenterViewModel.getNotificationsPush();
            dismissProgressDialog();
        });


    }

    private void listMunicipio(List list) {
        final List<Municipio> listPlaces = list;
        if (listPlaces != null) {
            String[] municipios =  Utils.getMunicipiosArrayFromItemGeneralList(listPlaces);
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.setTitle("Municipio");
            builder.setItems(municipios, (dialog, which) -> {
                updateSubscriptionViewModel.sector.setValue(Constants.EMPTY_STRING);
                showProgressDIalog(R.string.text_please_wait);
                itemMunicipioSelected = (Municipio) list.get(which);
                 updateSubscriptionViewModel.municipality.setValue(itemMunicipioSelected.getDescripcion());
                updateSubscriptionViewModel.setIdmuncipio(itemMunicipioSelected.getIdMunicipio());
                 updateSubscriptionViewModel.enableSector.setValue(false);
                updateSubscriptionViewModel.loadSector(itemMunicipioSelected.getIdMunicipio());
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
                updateSubscriptionViewModel.setIdsector(itemSectorSelected.getIdSector());
                 updateSubscriptionViewModel.sector.postValue(itemSectorSelected.getSector());
            });
            alertDialog2 = builder.create();
            alertDialog2.show();
        }
    }

    public void close(View view){
        onBackPressed();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {

        return false;
    }



    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        return false;
    }

    private void showAlertDialogTryAgain(final int title, final int message, final int positive, final int negative) {
        runOnUiThread(() -> {
            if (!getUsuario().isInvitado() && title == R.string.title_appreciated_user) {
                getCustomAlertDialog().showAlertDialog(getUsuario().getNombres(), message, false, positive, (dialogInterface, i) -> {
                    loadDatesService();
                }, negative, (dialogInterface, i) -> onBackPressed(), null);
            } else {
                getCustomAlertDialog().showAlertDialog(title, message, false, positive, (dialogInterface, i) -> {
                    loadDatesService();
                }, negative, (dialogInterface, i) -> onBackPressed(), null);
            }
        });
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

    private void loadDatesService() {
        updateSubscriptionViewModel.getSuscription();
    }


    private void showAlertDialogSuccesful(String title, String message) {
        getCustomAlertDialog().showAlertDialog(title, message, false, R.string.btn_acept, (dialog, which) -> onBackPressed(), null);
    }

    private void showAlertDialogCancelSubscritionSuccesful(String title,String message) {
        getCustomAlertDialog().showAlertDialog(title, message, false, R.string.btn_acept, (dialog, which) -> startActivitySubscriptionAlertas(), null);
    }

    private void showAlertDialogRecoverySubscritionSuccesful(String title,String message) {
        getCustomAlertDialog().showAlertDialog(title, message, false, R.string.btn_acept, (dialog, which) -> startActivitySubscriptionAlertas(), null);
    }



    private void startActivityDashboardAlertas(){
        Intent intent = new Intent(UpdateSubscriptionActivity.this, DashboardComunityAlertActivity.class);
        startActivityWithOutDoubleClick(intent);
    }

    private void startActivitySubscriptionAlertas(){
        Intent intent = new Intent(UpdateSubscriptionActivity.this, SplashActivity.class);
        startActivityWithOutDoubleClick(intent);
    }


    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }

    @Override
    public void onBackPressed() {
        startActivityDashboardAlertas();
    }
}
