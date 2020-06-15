package com.epm.app.mvvm.comunidad.views.activities;

import android.content.Intent;

import com.epm.app.R;

import app.epm.com.utilities.utils.Constants;

public class RedAlertInformationActivity extends BaseMapRedAlertActivity {


    public RedAlertInformationActivity() {
        super(Constants.ALERTA_ROJA, Constants.URL_BASE_MAP, Constants.ID_MAP_ALERTA_ROJA) ;
    }

    @Override
    public String getTitleActivity() {
        return getResources().getString(R.string.title_toolbar_red_alert);
    }

    @Override
    protected String getTitleCustom() {
        return Constants.TITLE_ALERTA_ROJA;
    }

    @Override
    protected String getAdrresCustom() {
        return Constants.ADDRESS_ESTACIONES_DE_GAS;
    }

    @Override
    protected String getScheduleCustom() {
        return Constants.EMPTY_STRING;
    }

    @Override
    protected String getImageCustom() {
        return Constants.IMAGE_ESTACIONES_DE_GAS;
    }

    @Override
    protected void onCreate() {
        loadBinding();
        loadDatesViewModel();
    }


    private void loadDatesViewModel(){
        redAlertViewModel.showError();
        redAlertViewModel.validateSubscription();
    }

    private void loadBinding(){
        redAlertViewModel.getProgress().observe(this, aBoolean -> {
            if(aBoolean != null && aBoolean){
                showProgressDIalog(R.string.text_please_wait);
            }else{
                dismissProgressDialog();
            }
        });
        redAlertViewModel.getSubscription().observe(this, subscription -> {
            if(subscription != null && !subscription){
                startActivity();
            }
        });
        redAlertViewModel.getExpiredToken().observe(this, aBoolean -> {
            if(aBoolean!= null && aBoolean){
                showAlertDialogUnauthorized(redAlertViewModel.getIntTitleError(),redAlertViewModel.getErrorUnauhthorized());
            }
        });
        redAlertViewModel.getError().observe(this, errorMessage -> {
            if(errorMessage !=null) {
                showAlertDialogTryAgain(errorMessage.getTitle(),errorMessage.getMessage(), R.string.text_intentar, R.string.text_cancelar);
            }
        });
        redAlertViewModel.getNotFound().observe(this, notFound -> {
            if(notFound !=null && notFound) {
                startActivity();
            }
        });
        redAlertViewModel.getGetDetailRedAlertResponse().observe(this, getDetailRedAlertResponse -> {
            if(getDetailRedAlertResponse != null){
                location.setCurrentLocationLatitude(Double.valueOf(getDetailRedAlertResponse.getDetalleNotificacionPush().getLatitud()));
                location.setCurrentLocationLongitude(Double.valueOf(getDetailRedAlertResponse.getDetalleNotificacionPush().getLongitud()));
                startLocation();
                pointCenter();
                showPointInMap(location.getCurrentLocationLongitude(),location.getCurrentLocationLatitude());
            }
        });
    }

    private void showAlertDialogTryAgain(final int title, final int message, final int positive, final int negative) {
        runOnUiThread(() -> {
            if (!getUsuario().isInvitado() && title == R.string.title_appreciated_user) {
                getCustomAlertDialog().showAlertDialog(getUsuario().getNombres(), message, false, positive, (dialogInterface, i) -> {
                    redAlertViewModel.validateSubscription();
                }, negative, (dialogInterface, i) -> onBackPressed(), null);
            } else {
                getCustomAlertDialog().showAlertDialog(title, message, false, positive, (dialogInterface, i) -> {
                    redAlertViewModel.validateSubscription();
                }, negative, (dialogInterface, i) -> onBackPressed(), null);
            }
        });
    }

    private void startActivity(){
        Intent intent = new Intent(RedAlertInformationActivity.this,SplashActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }


}
