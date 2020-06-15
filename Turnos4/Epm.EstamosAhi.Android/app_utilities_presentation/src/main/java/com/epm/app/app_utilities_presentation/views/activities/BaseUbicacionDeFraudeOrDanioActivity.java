package com.epm.app.app_utilities_presentation.views.activities;

import android.Manifest;
import android.content.Intent;
import android.graphics.drawable.BitmapDrawable;
import android.location.LocationManager;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.v4.content.ContextCompat;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.MotionEvent;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TextView;


import com.epm.app.app_utilities_presentation.R;
import com.epm.app.app_utilities_presentation.presenters.BaseUbicacionDeFraudeOrDanioPresenter;
import com.epm.app.app_utilities_presentation.utils.LoadIconServices;
import com.epm.app.app_utilities_presentation.views.views_activities.IBaseUbicacionDeFraudeOrDanioView;
import com.epm.app.business_models.business_models.ETipoServicio;
import com.epm.app.business_models.business_models.InformacionDeUbicacion;
import com.epm.app.business_models.business_models.Mapa;
import com.esri.arcgisruntime.geometry.GeographicTransformation;
import com.esri.arcgisruntime.geometry.GeographicTransformationStep;
import com.esri.arcgisruntime.geometry.GeometryEngine;
import com.esri.arcgisruntime.geometry.Point;
import com.esri.arcgisruntime.geometry.SpatialReference;
import com.esri.arcgisruntime.geometry.SpatialReferences;
import com.esri.arcgisruntime.loadable.LoadStatus;
import com.esri.arcgisruntime.mapping.ArcGISMap;
import com.esri.arcgisruntime.mapping.Basemap;
import com.esri.arcgisruntime.mapping.view.Graphic;
import com.esri.arcgisruntime.mapping.view.GraphicsOverlay;
import com.esri.arcgisruntime.mapping.view.LocationDisplay;
import com.esri.arcgisruntime.mapping.view.MapView;
import com.esri.arcgisruntime.symbology.PictureMarkerSymbol;

import app.epm.com.utilities.helpers.CustomMapOnTouchListener;
import app.epm.com.utilities.helpers.GpsLocationReceiver;
import app.epm.com.utilities.helpers.ICustomMapOnTouchListener;
import app.epm.com.utilities.helpers.Permissions;
import app.epm.com.utilities.helpers.UbicationHelper;
import app.epm.com.utilities.services.ServicesArcGIS;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

public abstract class BaseUbicacionDeFraudeOrDanioActivity<T extends BaseUbicacionDeFraudeOrDanioPresenter> extends BaseActivity<T> implements  ICustomMapOnTouchListener, TextWatcher, IBaseUbicacionDeFraudeOrDanioView {
    private MapView ubicacion_mapView;
    private GraphicsOverlay graphicsLayer;
    private Button ubicacion_btnContinuar;
    protected EditText ubicacion_etAddress;
    private ImageView ubicacion_ivServicio;
    private TextView ubicacion_tvAddress;
    protected ImageView ubicacion_ivProgress;
    protected RelativeLayout ubicacion_rlAnonymous;
    protected CheckBox ubicacion_cbAnonymous;
    protected TextView ubicacion_tvTitle;
    private LocationDisplay locationDisplayManager;
    private boolean firstPointLoaded;
    private PictureMarkerSymbol pictureMarkerSymbol;
    private boolean openGpsSettings;
    protected Point pointSelected;
    protected ETipoServicio tipoServicio;
    protected InformacionDeUbicacion informacionDeUbicacion;
    protected Toolbar toolbarApp;
    protected ServicesArcGIS servicesArcGIS;
    private ImageView ubicacion_ivLocation;
    protected Mapa mapa;
    private ArcGISMap map;
    private boolean showPointInMapSelected;
    private boolean validatePointSelected;
    protected abstract String getTitleCustom();
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ubicacion);
        loadDrawerLayout(R.id.generalDrawerLayout);
        tipoServicio = (ETipoServicio) getIntent().getSerializableExtra(Constants.TIPO_SERVICIO);
        createProgressDialog();
        loadViews();
        loadIconServices();
        try {
            locationDisplayManager = ubicacion_mapView.getLocationDisplay();
            locationDisplayManager.stop();
        } catch (Exception ex) {
            Log.e("Exception", ex.toString());
        }


        mapa = (Mapa) getIntent().getSerializableExtra(Constants.SERVICES);
        map = new ArcGISMap(Basemap.Type.OPEN_STREET_MAP, 6.234703, -75.5514745, 12);
        loadServicesArcGIS(mapa.getUrlServicio());
        ubicacion_mapView.setMap(map);
        validateInternetToLoadMap();
    }

    private void loadIconServices() {
        LoadIconServices loadIconServices = new LoadIconServices();
        ubicacion_ivServicio.setImageResource(loadIconServices.setIdIconoServicio(this.tipoServicio.getName()));
        ubicacion_tvAddress.setText(getTitleCustom());
    }

    private void loadServicesArcGIS(String url) {
        servicesArcGIS = new ServicesArcGIS(url,map, this);
    }

    @Override
    protected void onPause() {
        super.onPause();
        if(ubicacion_mapView != null) {
            ubicacion_mapView.pause();
        }
    }

    protected void onResume() {
        super.onResume();
        if(ubicacion_mapView != null) {
            ubicacion_mapView.resume();
        }
    }

    @Override
    protected void onDestroy() {
        if(ubicacion_mapView != null) {
            ubicacion_mapView.dispose();
        }
        super.onDestroy();
    }

    @Override
    public Intent getDefaultIntent(Intent intent) {
        if (openGpsSettings) {
            openGpsSettings = false;
            return intent;
        } else {
            return super.getDefaultIntent(intent);
        }
    }

    private void validateInternetToLoadMap() {
        if (getValidateInternet().isConnected()) {
            loadAllControlsToTheMap();
        } else {
            getCustomAlertDialog().showAlertDialog(getName(), R.string.text_validate_internet, false, R.string.text_intentar, (dialog, which) -> validateInternetToLoadMap(), R.string.text_cancelar, (dialog, which) -> BaseUbicacionDeFraudeOrDanioActivity.this.finish(), null);
        }
    }

    private void loadAllControlsToTheMap() {
        laodPictureMarkerSymbol();
        loadMap();
    }


    private void laodPictureMarkerSymbol() {
        BitmapDrawable pinStarBlueDrawable = (BitmapDrawable) ContextCompat.getDrawable(this, R.mipmap.icon_location_epm);
        pictureMarkerSymbol = new PictureMarkerSymbol(pinStarBlueDrawable);
    }

    private void loadChangeStateGpsListener() {
        GpsLocationReceiver.setGpsLocationListener(() -> {
            if (gpsIsOn()) {
                startLocation();
            }
        });
    }

    private void loadLocation() {
        if (Permissions.isGrantedPermissions(this, Manifest.permission.ACCESS_FINE_LOCATION)) {
            valdiateGpsIsOn();
        } else {
            String[] permissions = {Manifest.permission.ACCESS_FINE_LOCATION, Manifest.permission.ACCESS_COARSE_LOCATION};
            Permissions.verifyPermissions(this, permissions);
        }
    }

    private void startLocation() {
        startLocationDisplay();
    }

    private void beforeLoadPoint(Point point){
        validatePointSelected = true;
        loadPoint(point);
    }

    private void beforeLoadPoint(Point point, boolean validatePoint){
        validatePointSelected = validatePoint;
        loadPoint(point);
    }

    private void loadPoint(Point point) {

        showPointInMapSelected = false;
        this.pointSelected = point;
        Point beforePoint = point;
        point = UbicationHelper.getWGS84PointBuilder(beforePoint);

        if(point == null){
           point =  beforePoint;
        }

        if(point != null){
            getAdrress(point);
        }
    }

    private void getAdrress(final Point point) {
        if (point == null) {
            showAlertDialogGeneralInformationOnUiThread(getName(), R.string.text_ubicacion_no_encontrada);
        } else {
            getPresenter().validateInternetToExecuteAnAction(() -> getPresenter().getInformacionDeUbicacion(String.valueOf(point.getX()), String.valueOf(point.getY())));
        }
    }

    private void loadMap() {
        map.addDoneLoadingListener(() -> {
            if(map.getLoadStatus() == LoadStatus.LOADED){
                loadChangeStateGpsListener();
                loadLocation();
            }else if(map.getLoadError() != null){
            }
        });
        graphicsLayer = new GraphicsOverlay();
        ubicacion_mapView.getGraphicsOverlays().add(graphicsLayer);
        ubicacion_mapView.setOnTouchListener(new CustomMapOnTouchListener(this, ubicacion_mapView, this));

    }

    private void loadViews() {
        toolbarApp =  findViewById(R.id.toolbarApp);
        ubicacion_mapView =  findViewById(R.id.ubicacion_mapView);
        ubicacion_btnContinuar =  findViewById(R.id.ubicacion_btnContinuar);
        ubicacion_btnContinuar.addTextChangedListener(this);
        ubicacion_btnContinuar.setEnabled(false);
        ubicacion_etAddress =  findViewById(R.id.ubicacion_etAddress);
        ubicacion_etAddress.addTextChangedListener(this);
        ubicacion_ivLocation =  findViewById(R.id.ubicacion_ivLocation);
        ubicacion_ivServicio =  findViewById(R.id.ubicacion_ivServicio);
        ubicacion_ivProgress =  findViewById(R.id.ubicacion_ivProgress);
        ubicacion_rlAnonymous =  findViewById(R.id.ubicacion_rlAnonymous);
        ubicacion_cbAnonymous =  findViewById(R.id.ubicacion_cbAnonymous);
        ubicacion_tvTitle =  findViewById(R.id.ubicacion_tvTitle);
        ubicacion_tvAddress =  findViewById(R.id.ubicacion_tvAddress);
        loadOnClickListener();
    }

    private void loadOnClickListener() {
        ubicacion_ivLocation.setOnClickListener(view -> {
            firstPointLoaded = false;
            loadChangeStateGpsListener();
            loadLocation();
        });

        ubicacion_btnContinuar.setOnClickListener(v -> onClickBtnContinuar());
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        if (requestCode == Constants.REQUEST_CODE_PERMISSION && grantResults[0] == 0) {
            valdiateGpsIsOn();
        }
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    private void valdiateGpsIsOn() {
        if (gpsIsOn()) {
            goCurrentPosition();
        } else {
            getCustomAlertDialog().showAlertDialog(getName(), R.string.text_gps_disabled, false, R.string.text_aceptar, (dialog, which) -> {
                openGpsSettings = true;
                startActivity(new Intent(android.provider.Settings.ACTION_LOCATION_SOURCE_SETTINGS));
            }, R.string.text_cancelar, (dialog, which) -> dialog.dismiss(), null);
        }
    }

    private void loadUbication(InformacionDeUbicacion informacionDeUbicacion) {
        this.informacionDeUbicacion = informacionDeUbicacion;
        showPointInMap();
    }

    public abstract void onClickBtnContinuar();

    private void showPointInMap() {
        if(validatePointSelected) {
            graphicsLayer.getGraphics().clear();
            Graphic graphic = new Graphic(this.pointSelected, pictureMarkerSymbol);
            graphicsLayer.getGraphics().add(graphic);

            if (!graphicsLayer.getGraphics().isEmpty()) {
                showPointInMapSelected = true;
            }
            enableButtonContinue();
        }
    }

    @Override
    public void loadUbicationOnUiThread(final InformacionDeUbicacion informacionDeUbicacion) {
        runOnUiThread(() -> loadUbication(informacionDeUbicacion));
    }


    @Override
    public void showAlertWithOutAddress(final String name, final int title) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(name, title, false, R.string.text_aceptar, (dialog, which) -> {
            createProgressDialog();
            pointSelected = null;
            enableButtonContinue();
            if (gpsIsOn()) {
                firstPointLoaded = false;
                startLocation();
            } else {
                graphicsLayer.getGraphics().clear();
            }

            dismissProgressDialog();
        }, null));
    }

    private boolean gpsIsOn() {
        LocationManager locationManager;
        locationManager = (LocationManager) getSystemService(LOCATION_SERVICE);
        return locationManager.isProviderEnabled(LocationManager.GPS_PROVIDER);
    }


    @Override
    public void onSingleTap(MotionEvent motionEvent) {
        android.graphics.Point screenPoint = new android.graphics.Point((int) motionEvent.getX(), (int) motionEvent.getY());
        Point  point = ubicacion_mapView.screenToLocation(screenPoint);
        beforeLoadPoint(point, true);
    }

    private void enableButtonContinue() {

        if (ubicacion_etAddress.getText().toString().trim().isEmpty() || pointSelected == null || !showPointInMapSelected) {
            ubicacion_btnContinuar.setEnabled(false);
            ubicacion_btnContinuar.setBackgroundResource(R.color.status_bar);
        } else {
            ubicacion_btnContinuar.setEnabled(true);
            ubicacion_btnContinuar.setBackgroundResource(R.color.button_green_factura);
        }
    }

    private void startLocationDisplay(){
        if(!locationDisplayManager.isStarted()) {
            locationDisplayManager.startAsync();
        }
        locationDisplayManager.addLocationChangedListener(locationChangedEvent -> {
            if (!firstPointLoaded) {

                Point position = locationChangedEvent.getSource().getLocation().getPosition();
                if(position != null) {
                    firstPointLoaded = true;
                    Point wgs84Pt = new Point(position.getX(), position.getY(), SpatialReference.create(4326));
                    GeographicTransformation transform = GeographicTransformation.create(GeographicTransformationStep.create(1149).getInverse());
                    Point webMercator = (Point) GeometryEngine.project(wgs84Pt, SpatialReferences.getWebMercator(), transform);
                    beforeLoadPoint(webMercator);

                }
            }
        });
    }

    private void goCurrentPosition(){
        locationDisplayManager.setAutoPanMode(LocationDisplay.AutoPanMode.RECENTER);
        startLocationDisplay();
    }

    @Override
    public void beforeTextChanged(CharSequence charSequence, int i, int i1, int i2) {

    }

    @Override
    public void onTextChanged(CharSequence charSequence, int i, int i1, int i2) {
        enableButtonContinue();
    }

    @Override
    public void afterTextChanged(Editable editable) {
        ubicacion_etAddress.setSelection(ubicacion_etAddress.length());
    }
}