package com.epm.app.view.activities;

import android.Manifest;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import androidx.databinding.DataBindingUtil;
import android.graphics.drawable.BitmapDrawable;
import android.graphics.drawable.Drawable;
import android.location.Location;
import android.os.Bundle;
import android.provider.Settings;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.widget.Toolbar;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;

import android.widget.FrameLayout;
import android.widget.ImageView;
import android.widget.Toast;


import com.bumptech.glide.Glide;
import com.bumptech.glide.load.resource.drawable.GlideDrawable;
import com.bumptech.glide.request.animation.GlideAnimation;
import com.bumptech.glide.request.target.GlideDrawableImageViewTarget;
import com.epm.app.R;
import com.epm.app.databinding.DialogEasyPointOfficeBinding;
import com.epm.app.mvvm.util.CustomLocation;
import app.epm.com.utilities.helpers.InformationOffice;
import com.esri.arcgisruntime.ArcGISRuntimeEnvironment;
import com.esri.arcgisruntime.ArcGISRuntimeException;
import com.esri.arcgisruntime.concurrent.ListenableFuture;
import com.esri.arcgisruntime.geometry.Envelope;
import com.esri.arcgisruntime.geometry.Point;
import com.esri.arcgisruntime.geometry.SpatialReferences;
import com.esri.arcgisruntime.loadable.LoadStatus;
import com.esri.arcgisruntime.mapping.ArcGISMap;
import com.esri.arcgisruntime.mapping.Viewpoint;
import com.esri.arcgisruntime.mapping.popup.Popup;
import com.esri.arcgisruntime.mapping.view.Callout;
import com.esri.arcgisruntime.mapping.view.DefaultMapViewOnTouchListener;
import com.esri.arcgisruntime.mapping.view.Graphic;
import com.esri.arcgisruntime.mapping.view.GraphicsOverlay;
import com.esri.arcgisruntime.mapping.view.IdentifyLayerResult;
import com.esri.arcgisruntime.mapping.view.LocationDisplay;
import com.esri.arcgisruntime.mapping.view.MapView;
import com.esri.arcgisruntime.portal.Portal;
import com.esri.arcgisruntime.portal.PortalItem;
import com.esri.arcgisruntime.symbology.PictureMarkerSymbol;

import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.concurrent.ExecutionException;

import app.epm.com.utilities.controls.CustomTextViewBold;
import app.epm.com.utilities.entities.FeaturesPointMap;
import app.epm.com.utilities.helpers.Permissions;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;

/**
 * Created by leidycarolinazuluagabastidas on 5/04/17.
 */

public abstract class BaseOficinasDeAtencionOEstacionesDeGasActivity extends BaseActivityWithDI{

    private Toolbar toolbarApp;
    private MapView base_mapView;
    private Callout mCallout;
    private FrameLayout baseMap_frameLayoutArcgisMap;
    private ImageView base_imageView_location;
    private LocationDisplay locationDisplayManager;
    private boolean firstPointLoaded;
    private boolean openGpsSettings;
    private String modulo;
    private String urlBase;
    private String id;
    private ArcGISMap map;
    private Portal portal;
    private PortalItem portalItem;
    private CustomLocation tempCustomLocation;
    private InformationOffice informationOfficeTurn;
    private boolean callHandLocation = false;
    private CustomTextViewBold titlebar;
    private String titleActivity;
    private boolean controlCallOfficeDetailActivity;
    private final String TagException = "ExceptionBaseMapOffice";

    private DialogEasyPointOfficeBinding binding;
    private boolean controlDialogOfficeDetail = false;
    private AlertDialog officeDetailDialog;
    private final int PERMISION_GPS = 1;
    private boolean controlNeverAsk = false;
    private GraphicsOverlay mGraphicsOverlay;
    private boolean startGraphicsOverlay = false;
    private boolean callNewService;
    private boolean callActivity = false;
    private String TAG_EXCEPTION = "exceptionPermission";
    private boolean doubleClick =false;
    private boolean expiredToken;
    private LocationDisplay.LocationChangedEvent location;


    public BaseOficinasDeAtencionOEstacionesDeGasActivity(String modulo, String urlBase, String id) {
        this.modulo = modulo;
        this.urlBase = urlBase;
        this.id = id;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_base_oficinas_de_atencion_o_estaciones_de_gas);
        loadDrawerLayout(R.id.generalDrawerLayout);
        titlebar = findViewById(R.id.titleBar);
        titlebar.setText(getTitleActivity());
        callNewService = true;
        loadViews();
        mGraphicsOverlay = new GraphicsOverlay();
    }

    @Override
    protected void onStart() {
        super.onStart();
        location = null;
        controlNeverAsk = false;
        controlCallOfficeDetailActivity = false;
        firstPointLoaded = false;
        doubleClick = false;
        location = null;
        if (!expiredToken){
            validateService();
        }
    }

    @Override
    protected void onResume() {
        super.onResume();
        if (base_mapView!= null){
            base_mapView.resume();
        }
    }

    @Override
    protected void onPause() {
        super.onPause();
        mGraphicsOverlay.getGraphics().clear();
        if (base_mapView!=null){
            base_mapView.pause();
        }
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
        if (base_mapView!= null){
            base_mapView.dispose();
        }
    }

    private void startMap(){
        loadMap();
        locationDisplayManager = base_mapView.getLocationDisplay();
        if(!startGraphicsOverlay){
            base_mapView.getGraphicsOverlays().add(mGraphicsOverlay);
            startGraphicsOverlay = true;
        }
        locationDisplayManager.stop();
    }

    private void validateService(){
        if (startGraphicsOverlay && base_mapView != null && base_mapView.getGraphicsOverlays() != null) {
            base_mapView.getGraphicsOverlays().clear();
            startGraphicsOverlay = false;
        }
        if(callNewService || callActivity) {
            validateInternetToLoadMap();
            callNewService = false;
            callActivity = false;
        }else{
            validateInternetWithoutServiceToLoadMap();
        }
    }

    protected void successValidateInternet(){
        startMap();
        loadService();
    }

    protected void loadService(){

    }

    private void successValidateWithoutServiceInternet(){
        startMap();
    }

    public String getTitleActivity() {
        return titleActivity;
    }

    private void loadMap() {
        ArcGISRuntimeEnvironment.setLicense(Constants.CUSTOMER_ID_ARCGIS);
        base_mapView = new MapView(this);
        baseMap_frameLayoutArcgisMap.addView(base_mapView);
        createPortalItem();
        mCallout = base_mapView.getCallout();
        startListeners();
    }

    private void createPortalItem(){
        if (base_mapView != null) {
            portal = new Portal(urlBase, false);
            portal.loadAsync();
            portal.addDoneLoadingListener(() -> {
                ArcGISRuntimeEnvironment.setLicense(Constants.ARCGIS_LICENSEINFO);
            });

            portalItem = new PortalItem(portal, id);
            map = new ArcGISMap(portalItem);
        }
        base_mapView.setMap(map);
    }

    private void startListeners(){
        map.addLoadStatusChangedListener(loadStatusChangedEvent -> {
            if (loadStatusChangedEvent.getNewLoadStatus() == LoadStatus.LOADED) {
                itDoGenerallyLoad();
            }
        });

        base_imageView_location.setOnClickListener(view -> {
            if(getValidateInternet().isConnected()){
                callHandLocation = true ;
                itDoGenerallyLoad();
            }else{
                showDialogNotInternetOnlyMessage();
            }
        });

        base_mapView.setOnTouchListener(new DefaultMapViewOnTouchListener(this, base_mapView) {
            @Override
            public boolean onSingleTapConfirmed(MotionEvent event) {

                if (getValidateInternet().isConnected()) {
                    prepareOfficeInformation(event);
                } else {
                    showDialogNotInternetOnlyMessage();
                }
                return super.onSingleTapConfirmed(event);
            }
        });
    }

    private void itDoGenerallyLoad(){
        if(!firstPointLoaded){
            loadDefaulLocation();
        }
        loadLocation();
    }


    protected void validateInternetToLoadMap() {
        validateAlertDialog();
        if (getValidateInternet().isConnected()) {
            successValidateInternet();
        } else {
            showDialogNotInternet();
        }
    }

    private void validateInternetWithoutServiceToLoadMap() {
        validateAlertDialog();
        if (getValidateInternet().isConnected()) {
            successValidateWithoutServiceInternet();
        } else {
            showDialogNotInternet();
        }
    }

    private void showDialogNotInternet(){
        getCustomAlertDialog().showAlertDialog(getName(), R.string.text_validate_internet, false, R.string.text_intentar, (dialogInterface, i) -> validateInternetToLoadMap(), R.string.text_cancelar, (dialogInterface, i) -> onBackPressed(), null);
    }

    private void loadViews() {
        toolbarApp = findViewById(R.id.toolbar);
        baseMap_frameLayoutArcgisMap =  findViewById(R.id.baseMap_frameLayoutArcgisMap);
        base_imageView_location =  findViewById(R.id.base_imageView_location);
        loadToolbar();
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar((Toolbar) toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        toolbarApp.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onBackPressed();
            }
        });
    }


    /**
     * @param event
     */
    private void prepareOfficeInformation(MotionEvent event) {
        if (!doubleClick){
            doubleClick = true;
            android.graphics.Point screenPoint = new android.graphics.Point((int) event.getX(),
                    (int) event.getY());
            final ListenableFuture<List<IdentifyLayerResult>> identifyFuture =
                    base_mapView.identifyLayersAsync(screenPoint, 10, false);
            identifyFuture.addDoneListener(() -> doneListener(identifyFuture));

        }
    }

    /**
     * @param identifyFuture
     */
    private void doneListener(ListenableFuture<List<IdentifyLayerResult>> identifyFuture) {
        try {
            List<IdentifyLayerResult> identifyLayersResults = identifyFuture.get();
            if (identifyLayersResults.size() >= 1) {
                IdentifyLayerResult identifyLayerResult = identifyLayersResults.get(0);
                validateEmptyIdentifyLayerResult(identifyLayerResult);
            }else{
                doubleClick = false;
            }
        } catch (InterruptedException e) {
            Log.e(TagException, e.getMessage());
            doubleClick = false;
        } catch (ExecutionException ex) {
            Log.e(TagException, ex.getMessage());
            doubleClick = false;
        }
    }

    private void validateEmptyIdentifyLayerResult(IdentifyLayerResult identifyLayerResult){
        if (identifyLayerResult.getPopups().size() >= 1) {
            Popup identifiedPopup = identifyLayerResult.getPopups().get(0);
            Map<String, Object> attr = identifiedPopup.getGeoElement().getAttributes();
            Set<String> keys = attr.keySet();
            setDataOfficePoint(keys, attr);
        }
    }


    protected boolean validateInternet() {
        return getValidateInternet().isConnected() ? true : false;
    }


    protected boolean dataIsValidated(Object data){
        return data == null ? false : true;
    }

    public void dialogEasyPointOffice(FeaturesPointMap featuresPointMap, boolean showingSchedule) {
        officeDetailDialog = new AlertDialog.Builder(this).create();
        controlDialogOfficeDetail = true;
        LayoutInflater factory = LayoutInflater.from(this);
        binding = DataBindingUtil.inflate(factory, R.layout.dialog_easy_point_office, null, false);
        officeDetailDialog.setView(binding.getRoot());
        officeDetailDialog.setCancelable(false);
        String name = getValueFeaturesPointMap(featuresPointMap, getTitleCustom()).replaceFirst(Constants.kEY_REPLACE_TITLE_EASY_POINT_START,Constants.kEY_REPLACE_TITLE_EASY_POINT_END);
        binding.textNameOffice.setText(name);
        binding.textAddress.setText(getValueFeaturesPointMap(featuresPointMap, getAdrresCustom()));
        binding.textSchedule.setText(getValueFeaturesPointMap(featuresPointMap, getScheduleCustom()));
        binding.textTitleSchedule.setVisibility(showingSchedule ? View.VISIBLE : View.GONE);
        binding.textSchedule.setVisibility(showingSchedule ? View.VISIBLE : View.GONE);
        binding.progressBarOffice.setVisibility(View.VISIBLE);
        downloadImage(featuresPointMap);
        binding.btnAcept.setOnClickListener(view -> {
            officeDetailDialog.dismiss();
            controlDialogOfficeDetail = false;
            doubleClick = false;
        });
        officeDetailDialog.show();
    }

    public void downloadImage(FeaturesPointMap featuresPointMap){
        Glide.with(getApplicationContext())
                .load(getValueFeaturesPointMap(featuresPointMap, getImageCustom()))
                .centerCrop()
                .error(R.drawable.ic_easy_point_office_default)
                .into(new GlideDrawableImageViewTarget(binding.imageOffice) {
                    @Override
                    public void onResourceReady(GlideDrawable resource, GlideAnimation<? super GlideDrawable> animation) {
                        binding.imageOffice.setImageDrawable(resource);
                        binding.progressBarOffice.setVisibility(View.GONE);
                    }

                    @Override
                    public void onLoadFailed(Exception e, Drawable errorDrawable) {
                        binding.progressBarOffice.setVisibility(View.GONE);
                        binding.imageOffice.setImageResource(R.drawable.ic_easy_point_office_default);
                    }
                });
    }

    protected String getValueFeaturesPointMap(FeaturesPointMap featuresPointMap, String itemSearch){
        String value = null;
        for(int i=0; i < featuresPointMap.getListFeaturesPointMapItem().size(); i++){
            if(featuresPointMap.getListFeaturesPointMapItem().get(i).getItemSearch().equalsIgnoreCase(itemSearch)){
                value = featuresPointMap.getListFeaturesPointMapItem().get(i).getValue();
            }
        }
        return value;
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        try{
            super.onRequestPermissionsResult(requestCode, permissions, grantResults);
            if(requestCode == PERMISION_GPS) {
                validatePermisionLocation(grantResults[0], permissions[0]);
            }
        }catch(ArrayIndexOutOfBoundsException e){
            Log.e(TAG_EXCEPTION, e.getMessage());
            validateAlertDialog();
        }
    }

    private void validatePermisionLocation(int grantResult, String permission ){
        if (grantResult == PackageManager.PERMISSION_GRANTED) {
            validateGpsIsOn();
        }else if(controlNeverAsk || !ActivityCompat.shouldShowRequestPermissionRationale(this, permission)){
            showMessageSettings();
        }else{
            loadDefaulLocation();
        }
    }

    private void loadLocation() {
        if (Permissions.isGrantedPermissions(this, Manifest.permission.ACCESS_FINE_LOCATION)) {
            validateGpsIsOn();
        } else {
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.ACCESS_FINE_LOCATION}, PERMISION_GPS);
        }
    }


    public void showMessageSettings()
    {
        try {
            getCustomAlertDialog().showAlertDialog(app.epm.com.utilities.R.string.title_recommend_giving_permissions, app.epm.com.utilities.R.string.text_recommend_giving_permissions, false, app.epm.com.utilities.R.string.text_go_to_settings, (dialog, which) -> {
                openGpsSettings = true;
                startActivityWithOutDoubleClick(new Intent(Settings.ACTION_APPLICATION_DETAILS_SETTINGS, Permissions.uriApp(getApplicationContext())),Constants.ACTION_TO_TURN_ON_PERMISSION);
            }, app.epm.com.utilities.R.string.text_cancelar, (dialog, which) -> {
                controlNeverAsk = true;
                loadDefaulLocation();
                dialog.dismiss();
            }, null);
        }catch(Exception e){

        }
    }

    private void validateGpsIsOn() {
        if (gpsIsOn()) {
            startLocation();
        } else {
            showAskPermissionGPS();
        }
    }


    protected boolean toDoZoom(){
        if(location != null && location.getLocation() != null) {
            pointCenter(location.getLocation().getPosition().getX(), location.getLocation().getPosition().getY());
            return true;
        }else{
            return false;
        }
    }



    private void showAskPermissionGPS(){
        getCustomAlertDialog().showAlertDialog(getName(), app.epm.com.reporte_danios_presentation.R.string.text_gps_disabled, false, app.epm.com.reporte_danios_presentation.R.string.text_aceptar, (dialog, which) -> {
            openGpsSettings = true;
            startActivity(new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS));
        }, app.epm.com.reporte_danios_presentation.R.string.text_cancelar, (dialog, which) -> {
            loadDefaulLocation();
            dialog.dismiss();
        }, null);
    }

    protected void itDoesLoadLocation(Point centerPoint, double scale){
        Viewpoint viewpoint = new Viewpoint(centerPoint, scale, 0);
        if(base_mapView != null){
            paintViewPoint(viewpoint);
        }
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

    @Override
    public void onBackPressed() {
        if (officeDetailDialog != null && officeDetailDialog.isShowing()) {
            officeDetailDialog.dismiss();
        } else {
            super.onBackPressed();
        }
    }

    protected void pointCenter(double latitud, double longitud) {
        mGraphicsOverlay.getGraphics().clear();
        if(informationOfficeTurn == null){
            drawWithOutTurn(latitud, longitud);
        }else{
            drawWithTurn(latitud, longitud);
        }
    }

    private void drawWithOutTurn(double latitud, double longitud){
        Point currentPoint = new Point(latitud, longitud, SpatialReferences.getWgs84());
        Viewpoint viewpoint = new Viewpoint(currentPoint, 20000, 0);
        showPointInMap(currentPoint,R.mipmap.ic_is_here,-25,30);
        paintViewPoint(viewpoint);
    }

    private void drawWithTurn(double latitud, double longitud){
        informationOfficeTurn.setCurrentLatitud(latitud);
        informationOfficeTurn.setCurrentLongitud(longitud);
        Point arrivePoint = new Point(informationOfficeTurn.getLongitud(), informationOfficeTurn.getLatitud(), SpatialReferences.getWgs84());
        Point currentPoint = new Point(informationOfficeTurn.getCurrentLatitud(), informationOfficeTurn.getCurrentLongitud(), SpatialReferences.getWgs84());
        Envelope initialEnvelope = new Envelope(currentPoint, arrivePoint);
        showPointInMap(currentPoint,R.mipmap.ic_is_here,-25,30);
        showPointInMap(arrivePoint,R.mipmap.ic_chosen_office,18,45);
        paintViewPointGeometryAsync(initialEnvelope);
    }

    public void showPointInMap(Point point,int idImage,float offsetX,float offsetY) {
        BitmapDrawable pinStarBlueDrawable = (BitmapDrawable) ContextCompat.getDrawable(this, idImage);
        final PictureMarkerSymbol pinStarBlueSymbol = new PictureMarkerSymbol(pinStarBlueDrawable);
        pinStarBlueSymbol.setOffsetY(offsetY);
        pinStarBlueSymbol.setOffsetX(offsetX);
        pinStarBlueSymbol.loadAsync();
        pinStarBlueSymbol.addDoneLoadingListener(() -> {
            Graphic pinStarBlueGraphic = new Graphic(point, pinStarBlueSymbol);
            mGraphicsOverlay.getGraphics().add(pinStarBlueGraphic);
        });
    }

    protected abstract String getTitleCustom();

    protected abstract String getAdrresCustom();

    protected abstract String getScheduleCustom();

    protected abstract String getImageCustom();

    protected abstract void setDataOfficePoint(Set<String> keys, Map<String, Object> attr);

    protected abstract void loadDefaulLocation();

    protected abstract void startLocation();

    protected void cancelDialogError(DialogInterface dialogInterface){}

    protected void callService(){}

    public LocationDisplay getLocationDisplayManager() {
        return locationDisplayManager;
    }

    public boolean isFirstPointLoaded() {
        return firstPointLoaded;
    }

    public void setFirstPointLoaded(boolean firstPointLoaded) {
        this.firstPointLoaded = firstPointLoaded;
    }

    public boolean isCallHandLocation() {
        return callHandLocation;
    }

    public void setCallHandLocation(boolean callHandLocation) {
        this.callHandLocation = callHandLocation;
    }

    public MapView getBase_mapView() {
        return base_mapView;
    }

    public void setBase_mapView(MapView base_mapView) {
        this.base_mapView = base_mapView;
    }

    public boolean isStartGraphicsOverlay() {
        return startGraphicsOverlay;
    }

    public void setStartGraphicsOverlay(boolean startGraphicsOverlay) {
        this.startGraphicsOverlay = startGraphicsOverlay;
    }

    public InformationOffice getInformationOfficeTurn() {
        return informationOfficeTurn;
    }

    public void setInformationOfficeTurn(InformationOffice informationOfficeTurn) {
        this.informationOfficeTurn = informationOfficeTurn;
    }

    public boolean isCallActivity() {
        return callActivity;
    }

    public void setCallActivity(boolean callActivity) {
        this.callActivity = callActivity;
    }

    public void setExpiredToken(boolean expiredToken) {
        this.expiredToken = expiredToken;
    }

    public void setLocation(LocationDisplay.LocationChangedEvent location) {
        this.location = location;
    }

    public void paintViewPoint(Viewpoint viewpoint){
        try {
            base_mapView.setViewpoint(viewpoint);
        }catch(ArcGISRuntimeException e){
            Log.e("ErrorPaintViewPoint",e.getMessage());
        }
    }

    public void paintViewPointGeometryAsync(Envelope initialEnvelope){
        try {
            base_mapView.setViewpointGeometryAsync(initialEnvelope, Constants.ZOOM_LEVEL_GENERAL_OFFICE);
        }catch(ArcGISRuntimeException e){
            Log.e("ErrorPaintViewGeometry",e.getMessage());
        }
    }
}