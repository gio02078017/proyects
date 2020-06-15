package com.epm.app.mvvm.turn.views.activities;

import android.Manifest;
import android.annotation.SuppressLint;
import android.arch.lifecycle.ViewModelProvider;
import android.arch.lifecycle.ViewModelProviders;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.databinding.DataBindingUtil;
import android.location.Location;
import android.location.LocationManager;
import android.os.Bundle;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.View;

import com.epm.app.R;
import com.epm.app.databinding.ActivityNearbyOfficesBinding;
import com.epm.app.mvvm.turn.adapter.NearbyOfficesRecyclerAdapter;
import com.epm.app.mvvm.turn.viewModel.NearbyOfficesViewModel;
import com.epm.app.mvvm.util.CustomLocation;

import app.epm.com.utilities.helpers.InformationOffice;

import com.epm.app.view.activities.OficinasDeAtencionActivity;

import javax.inject.Inject;

import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class NearbyOfficesActivity extends BaseActivityWithDI implements NearbyOfficesRecyclerAdapter.OnNearbyOfficesRecyclerListener {

    @Inject
    ViewModelProvider.Factory viewModelFactory;

    ActivityNearbyOfficesBinding binding;
    private Toolbar toolbarApp;

    private LocationManager locationManager;

    private final int PERMISION_GPS = 1;

    NearbyOfficesViewModel nearbyOfficesViewModel;

    private NearbyOfficesRecyclerAdapter adapter;

    private Location currentLocation;
    private InformationOffice informationOffice;
    private boolean controlDialog = false;
    private boolean controlNeverAsk = false;
    private boolean controlDoubleClick = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this, R.layout.activity_nearby_offices);
        this.configureDagger();

        nearbyOfficesViewModel = ViewModelProviders.of(this, viewModelFactory).get(NearbyOfficesViewModel.class);

        loadDrawerLayout(R.id.generalDrawerLayout);
        toolbarApp = (Toolbar) binding.toolbarNearbyOffices;
        loadToolbar();

        informationOffice = new InformationOffice();

        loadBinding();

        binding.btnSeeOtherOffices.setOnClickListener(view -> callOficinasDeAtencionActivity(false, false));
    }

    @Override
    protected void onStart() {
        super.onStart();
        controlDoubleClick = false;
        if (!controlDialog) {
            controlNeverAsk = false;
            validatePermisionGPS();
        }
    }

    @Override
    protected void onResume() {
        super.onResume();
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        if (requestCode == PERMISION_GPS) {
            if (grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                callGPS();
            } else if (!controlNeverAsk && !ActivityCompat.shouldShowRequestPermissionRationale(this, permissions[0])) {
                controlNeverAsk = true;
                controlDialog = true;
                showMessageSettings();
            } else {
                cancelPermisonGPS(true);
            }
        }
    }

    private void validatePermisionGPS() {
        locationManager = (LocationManager) getSystemService(Context.LOCATION_SERVICE);

        int permissionCheck = ContextCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION);

        if (permissionCheck == PackageManager.PERMISSION_GRANTED) {
            callGPS();
        } else {
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.ACCESS_FINE_LOCATION}, PERMISION_GPS);
        }
    }

    private void loadBinding() {
        nearbyOfficesViewModel.showError();
        nearbyOfficesViewModel.getProgressDialog().observe(this, aBoolean -> {
            if (aBoolean != null && aBoolean) {
                showProgressDIalog(R.string.text_please_wait);
            } else {
                dismissProgressDialog();
            }
        });
        nearbyOfficesViewModel.getError().observe(this, errorMessage -> {
            dismissProgressDialog();
            this.showAlertDialogTryAgain(errorMessage.getTitle(), errorMessage.getMessage(), R.string.text_intentar, R.string.text_cancelar);
        });
        nearbyOfficesViewModel.getExpiredToken().observe(this, aBoolean -> {
            controlDialog = true;
            showAlertDialogUnauthorized(nearbyOfficesViewModel.getError().getValue().getTitle(), nearbyOfficesViewModel.getErrorUnauthorized());
        });
        nearbyOfficesViewModel.currentLocation.observe(this, resultCurrentLocation -> {
            currentLocation = resultCurrentLocation;
            getNearbyOffices();
        });
        nearbyOfficesViewModel.getSuccessNearbyOffices().observe(this, success -> {
            if (success) {
                DrawOffices();
            }
        });
    }

    private void DrawOffices() {
        adapter = new NearbyOfficesRecyclerAdapter(this, nearbyOfficesViewModel.getListNearbyOffices(), this, getResources());
        LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false);
        binding.nearbyOfficesRecyclerView.setNestedScrollingEnabled(false);
        binding.nearbyOfficesRecyclerView.setHasFixedSize(true);
        binding.nearbyOfficesRecyclerView.setAdapter(adapter);
        binding.nearbyOfficesRecyclerView.setLayoutManager(linearLayoutManager);
        binding.nearbyOfficesRecyclerView.getAdapter();
        binding.btnSeeOtherOffices.setVisibility(View.VISIBLE);
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        nearbyOfficesViewModel.getError().removeObservers(this);
        finish();
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar((Toolbar) toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
    }


    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }

    private void startActivityDetail(int position) {

        if (!controlDoubleClick) {
            informationOffice.setIdOficina(nearbyOfficesViewModel.getListNearbyOffices().get(position).getIdOficina());
            informationOffice.setIdOficinaSentry(nearbyOfficesViewModel.getListNearbyOffices().get(position).getIdOficinaSentry());
            informationOffice.setNombreOficina(nearbyOfficesViewModel.getListNearbyOffices().get(position).getNombreOficina());
            informationOffice.setLatitud(Double.parseDouble(nearbyOfficesViewModel.getListNearbyOffices().get(position).getLatitud()));
            informationOffice.setLongitud(Double.parseDouble(nearbyOfficesViewModel.getListNearbyOffices().get(position).getLongitud()));
            informationOffice.setPuntoFacil(false);
            Intent intent = validateTurnAssigned(nearbyOfficesViewModel.getListNearbyOffices().get(position).getTurnoAsignado());
            intent.putExtra(Constants.INFORMATION_OFFICE, informationOffice);
            startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
            controlDoubleClick = true;
        }
    }

    private Intent validateTurnAssigned(String turnoAsignado){
        Intent intent;
        if (turnoAsignado == null) {
            intent = new Intent(NearbyOfficesActivity.this, OfficeDetailActivity.class);
        } else {
            intent = new Intent(NearbyOfficesActivity.this, ShiftInformationActivity.class);
        }
        return intent;
    }

    public boolean validateGpsAvailability() {
        if (gpsIsOn()) {
            return true;
        } else {
            showGpsDialog();
            return false;
        }
    }

    @SuppressLint("MissingPermission")
    private void callGPS() {
        if (validateGpsAvailability()) {
            nearbyOfficesViewModel.getLocation();
        }
    }

    private void getNearbyOffices() {
        if (currentLocation != null) {
            validateAlertDialog();
            nearbyOfficesViewModel.getNearbyOffices(currentLocation);
        }
    }

    private void callOficinasDeAtencionActivity(boolean finish, boolean defaultLocation) {
        if (!controlDoubleClick) {
            Intent intent = new Intent(NearbyOfficesActivity.this, OficinasDeAtencionActivity.class);
            if (finish && defaultLocation) {
                CustomLocation customLocation = new CustomLocation();
                customLocation.setLatitud(6.24488905);
                customLocation.setLongitud(-75.57745177);
                customLocation.setScale(7000);
                intent.putExtra(Constants.EXTRAS_LOCATION, customLocation);
            }
            startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
            if (finish) finish();
            controlDoubleClick = true;
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == Constants.ACTION_TO_TURN_ON_GPS) {
            //callGPS();
        } else if (requestCode == Constants.ACTION_TO_TURN_ON_PERMISSION) {
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.ACCESS_FINE_LOCATION}, PERMISION_GPS);
        }
    }

    private void showAlertDialogTryAgain(final int title, final int message, final int positive, final int negative) {
        runOnUiThread(() -> {
            controlDialog = true;
            getCustomAlertDialog().showAlertDialog((!getUsuario().isInvitado() && title == R.string.title_appreciated_user) ? getUsuario().getNombres() : getString(title), message, false, positive, (dialogInterface, i) -> {
                callService();
                controlDialog = false;
            }, negative, (dialogInterface, i) -> onBackPressed(), null);
        });
    }

    private void callService() {
        if (currentLocation != null) {
            showProgressDIalog(R.string.text_please_wait);
            getNearbyOffices();
        } else {
            callGPS();
        }
    }

    @Override
    public void onItemClick(int position, int idOffice) {
        startActivityDetail(position);
    }

    public void cancelPermisonGPS(boolean defaultLocation) {
        callOficinasDeAtencionActivity(true, defaultLocation);
    }
}
