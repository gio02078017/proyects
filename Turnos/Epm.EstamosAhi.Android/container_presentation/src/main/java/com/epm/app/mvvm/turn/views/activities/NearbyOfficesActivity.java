package com.epm.app.mvvm.turn.views.activities;

import android.Manifest;
import android.annotation.SuppressLint;
import androidx.lifecycle.ViewModelProvider;
import androidx.lifecycle.ViewModelProviders;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import androidx.databinding.DataBindingUtil;
import android.location.Location;
import android.location.LocationManager;
import android.os.Bundle;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.appcompat.widget.Toolbar;
import android.util.Log;
import android.view.View;

import com.epm.app.R;
import com.epm.app.databinding.ActivityNearbyOfficesBinding;
import com.epm.app.mvvm.procedure.models.ProcedureInformation;
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
    private boolean controlDoubleClick = false;
    private String TAG_EXCEPTION = "exceptionPermission";
    private boolean successService;
    private Bundle datosBeforeActivity;
    private ProcedureInformation procedureInformation;

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
        getInformationBeforeActivity();
        binding.btnSeeOtherOffices.setOnClickListener(view -> callOficinasDeAtencionActivity(false, false));
        loadService();
    }

    @Override
    protected void onStart() {
        super.onStart();
        controlDoubleClick = false;
        if (!controlDialog && successService) {
            loadService();
            successService = false;
        }
    }

    private void loadService(){
        setControlNeverAsk(false);
        validatePermisionGPS();
    }

    @Override
    protected void onResume() {
        super.onResume();
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        try {
            if (requestCode == PERMISION_GPS) {
                if (grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                    callGPS();
                } else if (isControlNeverAsk() || !ActivityCompat.shouldShowRequestPermissionRationale(this, permissions[0])) {
                    controlDialog = true;
                    showMessageSettings();
                } else {
                    cancelPermisonGPS(true);
                }
            }
        } catch (ArrayIndexOutOfBoundsException e) {
            Log.e(TAG_EXCEPTION, e.getMessage());
            validateAlertDialog();
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
            successService = false;
            this.showAlertDialogTryAgain(errorMessage.getTitle(), errorMessage.getMessage(), R.string.text_intentar, R.string.text_cancelar);
        });
        nearbyOfficesViewModel.getExpiredToken().observe(this, errorMessage ->{
            successService = false;
            showAlertDialogUnauthorized(errorMessage.getTitle(),errorMessage.getMessage());
        } );
        nearbyOfficesViewModel.currentLocation.observe(this, resultCurrentLocation -> {
            currentLocation = resultCurrentLocation;
            getNearbyOffices();
        });
        nearbyOfficesViewModel.getSuccessNearbyOffices().observe(this, success -> {
            if (success != null && success) {
                DrawOffices();
                successService = true;
            }
        });
    }

    private void getInformationBeforeActivity(){
        datosBeforeActivity = this.getIntent().getExtras();
        if(datosBeforeActivity.getSerializable(Constants.PROCEDURE_INFORMATION) != null){
            procedureInformation = (ProcedureInformation)datosBeforeActivity.getSerializable(Constants.PROCEDURE_INFORMATION);
        }
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
            startActivityWithOutDoubleClick(intent);
            controlDoubleClick = true;
        }
    }

    private Intent validateTurnAssigned(String turnoAsignado) {
        Intent intent;
        if (turnoAsignado == null) {
            intent = new Intent(NearbyOfficesActivity.this, OfficeDetailActivity.class);
        } else {
            intent = new Intent(NearbyOfficesActivity.this, ShiftInformationActivity.class);
        }
        AddProcedureInformation(intent);
        return intent;
    }

    public void AddProcedureInformation(Intent intent){
        if(procedureInformation != null){
            intent.putExtra(Constants.PROCEDURE_INFORMATION, procedureInformation);
        }
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
            successService = false;
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
            if (finish)finish();
            AddProcedureInformation(intent);
            startActivityWithOutDoubleClick(intent);
            controlDoubleClick = true;
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == Constants.ACTION_TO_TURN_ON_GPS) {
            callGPS();
        } else if (requestCode == Constants.ACTION_TO_TURN_ON_PERMISSION) {
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.ACCESS_FINE_LOCATION}, PERMISION_GPS);
        }
    }

    private void showAlertDialogTryAgain(final int title, final int message, final int positive, final int negative) {
        runOnUiThread(() -> {
            controlDialog = true;
            getCustomAlertDialog().showAlertDialog((getUsuario()!= null && !getUsuario().isInvitado() && title == R.string.title_appreciated_user) ? getUsuario().getNombres() : getString(title), message, false, positive, (dialogInterface, i) -> {
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
