package com.epm.app.mvvm.turn.views.activities;

import android.Manifest;
import android.content.pm.PackageManager;
import android.databinding.DataBindingUtil;
import android.graphics.drawable.BitmapDrawable;
import android.graphics.drawable.Drawable;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AlertDialog;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.widget.ImageView;
import android.widget.ProgressBar;

import com.bumptech.glide.Glide;
import com.bumptech.glide.load.resource.drawable.GlideDrawable;
import com.bumptech.glide.request.animation.GlideAnimation;
import com.bumptech.glide.request.target.GlideDrawableImageViewTarget;
import com.epm.app.R;
import com.epm.app.databinding.DialogEasyPointOfficeBinding;
import com.epm.app.databinding.DialogOfficeDetailBinding;
import com.epm.app.mvvm.turn.views.dialogs.ChooseMapProviderDialogFragment;
import app.epm.com.utilities.helpers.InformationOffice;
import com.epm.app.mvvm.util.Utils;
import com.esri.arcgisruntime.ArcGISRuntimeEnvironment;
import com.esri.arcgisruntime.concurrent.ListenableFuture;
import com.esri.arcgisruntime.geometry.Envelope;
import com.esri.arcgisruntime.geometry.Point;
import com.esri.arcgisruntime.geometry.SpatialReferences;
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

import app.epm.com.utilities.entities.FeaturesPointMap;
import app.epm.com.utilities.entities.FeaturesPointMapItem;
import app.epm.com.utilities.helpers.Permissions;
import app.epm.com.utilities.helpers.ProcessXMLData;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class BaseOfficeDetailActivity extends BaseActivityWithDI implements ChooseMapProviderDialogFragment.BottomSheetListener {

    protected LocationDisplay locationDisplayManager;
    protected ArcGISMap map;
    protected Portal portal;
    protected PortalItem portalItem;
    private MapView mapView;
    private Callout mCallout;
    private boolean controlDialogOfficeDetail = false;
    private AlertDialog officeDetailDialog;
    private final String TagException = "ExceptionOfficeDetail";
    private GraphicsOverlay mGraphicsOverlay;
    private InformationOffice informationOffice;
    private ChooseMapProviderDialogFragment chooseMapProviderDialogFragment;
    private final int PERMISION_GPS = 1;
    private int paddingMap;
    private boolean itDoZoom;
    private DialogEasyPointOfficeBinding binding;
    private FeaturesPointMap featuresPointMap;

    /**
     * @param mapView
     * @param paddingMap
     */
    protected void startMap(MapView mapView, int paddingMap, boolean itDoZoom) {
        this.mapView = mapView;
        this.paddingMap = paddingMap;
        mGraphicsOverlay = new GraphicsOverlay();
        chooseMapProviderDialogFragment = new ChooseMapProviderDialogFragment();
        this.itDoZoom = itDoZoom;
    }

    /**
     *
     */
    @Override
    public void onChoseGoogleMapsClicked() {
        callGoogleMaps(informationOffice.getLatitud(), informationOffice.getLongitud());
    }

    /**
     *
     */
    @Override
    public void onChoseWaseClicked() {
        callWaze(informationOffice.getLatitud(), informationOffice.getLongitud());
    }

    /**
     *
     */
    protected void validateInternetToLoadMap() {
        if (getValidateInternet().isConnected()) {
            successValidateInternet();
        } else {
            getCustomAlertDialog().showAlertDialog(getName(), R.string.text_validate_internet, false, R.string.text_intentar, (dialogInterface, i) -> validateInternetToLoadMap()
                    , R.string.text_cancelar, (dialogInterface, i) -> finish(), null);
        }
    }

    /**
     *
     */
    protected void successValidateInternet() {
        loadMap();
        locationDisplayManager = mapView.getLocationDisplay();
        mapView.getGraphicsOverlays().add(mGraphicsOverlay);
        locationDisplayManager.stop();
        initInformation();
    }

    /**
     * @param requestCode
     * @param permissions
     * @param grantResults
     */
    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        if (requestCode == PERMISION_GPS) {
            if (grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                validateGpsIsOn();
            } else {
                cancelPermisonGPSBaseOffice(true);
            }
        }
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    /**
     * @param informationOffice
     */
    protected void loadLocation(InformationOffice informationOffice) {
        this.informationOffice = informationOffice;
        validatePermissionUbicacion(true);
    }

    private void validatePermissionUbicacion(boolean goValidateGpsIsOn){
        if (Permissions.isGrantedPermissions(this, Manifest.permission.ACCESS_FINE_LOCATION)) {
            actionGpsIsOn(goValidateGpsIsOn);
        } else {
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.ACCESS_FINE_LOCATION}, PERMISION_GPS);
        }
     }

     private void actionGpsIsOn(boolean goValidateGpsIsOn){
         if(goValidateGpsIsOn){
             validateGpsIsOn();
         }else{
             validateGpsIsOnOnlyLocation();
         }
     }

    /**
     *
     */
    protected void validateGpsIsOn() {
        if (gpsIsOn()) {
            startLocation();
            successValidateGPS();
        } else {
            showGpsDialog();
        }
    }

    /**
     *
     */
    protected void startLocation() {
        if (locationDisplayManager.isStarted()) locationDisplayManager.stop();
        locationDisplayManager.startAsync();
        locationDisplayManager.addLocationChangedListener(locationChangedEvent -> {
            setDataLocation(locationChangedEvent);
        });
    }

    protected void itDoHandLocation(boolean itDoZoom){
        if (getValidateInternet().isConnected()) {
            this.itDoZoom = itDoZoom;
            validatePermissionUbicacion(false);
        } else {
            showDialogNotInternetOnlyMessage();
        }
    }

    private void validateGpsIsOnOnlyLocation(){
        if(gpsIsOn()){
            startLocation();
        }else {
            showGpsDialog();
        }
    }

    /**
     * @param locationChangedEvent
     */
    private void setDataLocation(LocationDisplay.LocationChangedEvent locationChangedEvent) {
        if (locationChangedEvent != null && locationChangedEvent.getLocation().getPosition() != null) {
            informationOffice.setCurrentLatitud(locationChangedEvent.getLocation().getPosition().getY());
            informationOffice.setCurrentLongitud(locationChangedEvent.getLocation().getPosition().getX());
            pointCenter(informationOffice);
        }
    }

    /**
     *
     */
    protected void loadMap() {
        ArcGISRuntimeEnvironment.setLicense(Constants.CUSTOMER_ID_ARCGIS);
        portal = new Portal(Constants.URL_BASE_MAP, false);
        portal.loadAsync();
        portal.addDoneLoadingListener(() -> ArcGISRuntimeEnvironment.setLicense(Constants.ARCGIS_LICENSEINFO));
        portalItem = new PortalItem(portal, Constants.ID_MAP_OFICINAS_DE_ATENCION);
        map = new ArcGISMap(portalItem);
        mapView.setMap(map);
        mCallout = mapView.getCallout();
        loadListener();
    }

    /**
     *
     */
    private void loadListener() {
        mapView.setOnTouchListener(new DefaultMapViewOnTouchListener(this, mapView) {
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

    /**
     * @param event
     */
    private void prepareOfficeInformation(MotionEvent event) {
        callOutIsShowing();
        android.graphics.Point screenPoint = new android.graphics.Point((int) event.getX(),
                (int) event.getY());
        final ListenableFuture<List<IdentifyLayerResult>> identifyFuture =
                mapView.identifyLayersAsync(screenPoint, 10, false);
        identifyFuture.addDoneListener(() -> doneListener(identifyFuture));

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
            }
        } catch (InterruptedException e) {
            Log.e(TagException, e.getMessage());
        } catch (ExecutionException ex) {
            Log.e(TagException, ex.getMessage());
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

    /**
     * @param keys
     * @param attr
     */
    private void setDataOfficePoint(Set<String> keys, Map<String, Object> attr) {
        featuresPointMap = ProcessXMLData.convertXMLToObject(this, Constants.NAME_FEATURE_POINT_MAP);
        featuresPointMap.setVerified(true);
        for (String key : keys) {
            Object value = attr.get(key);
            validateListPointMap(value,key);
        }
        evaluateFeaturesPointMap(attr);
    }

    private void validateListPointMap(Object value,String key){
        for (int i = 0; i < featuresPointMap.getListFeaturesPointMapItem().size(); i++){
            if(key.equalsIgnoreCase(featuresPointMap.getListFeaturesPointMapItem().get(i).getItemSearch())){
                validateDataModal(value,featuresPointMap.getListFeaturesPointMapItem().get(i));
            }
        }
    }

    private void validateDataModal(Object value, FeaturesPointMapItem featuresPointMapItem){
        if(dataIsValidated(value)){
            featuresPointMapItem.setValue(String.valueOf(value));
        }else if(featuresPointMapItem.isRequire()){
            featuresPointMap.setVerified(false);
        }else if(featuresPointMapItem.isEmpty()){
            featuresPointMapItem.setValue("");
        }
    }

    public void evaluateFeaturesPointMap(Map<String, Object> attr){
        if(featuresPointMap.isVerified()){
            validateModalType(attr.get(Constants.APPLY_EASY_POINT));
        }else{
            showAlertDialog(getString(R.string.text_title_it_is_not_information), getString(R.string.text_it_is_not_information));
        }
    }

    /**
      * @param value
     */
    private void validateModalType(Object value) {
        if (value.equals(Constants.CONFIRM)) {
                dialogEasyPointOffice();
        } else {
            renderPointOffice();
        }

    }


    private void renderPointOffice() {
        if (!controlDialogOfficeDetail) {
            dialogOfficeDatail();
        }
    }

    /**
     *
     */
    private void callOutIsShowing() {
        if (mCallout.isShowing()) {
            mCallout.dismiss();
        }
    }

    public void dialogOfficeDatail() {
        controlDialogOfficeDetail = true;
        LayoutInflater factory = LayoutInflater.from(this);
        DialogOfficeDetailBinding binding = DataBindingUtil.inflate(factory, R.layout.dialog_office_detail, null, false);
        officeDetailDialog = new AlertDialog.Builder(this).create();
        officeDetailDialog.setView(binding.getRoot());
        officeDetailDialog.setCancelable(false);
        binding.textNameOffice.setText(getValueFeaturesPointMap(Constants.TITLE_OFICINAS_DE_ATENCION));
        binding.progressBarOffice.setVisibility(View.VISIBLE);
        downloadImageOfficeDatail(binding.imageOffice, binding.progressBarOffice);
        binding.btnAcept.setOnClickListener(view -> {
            officeDetailDialog.dismiss();
            controlDialogOfficeDetail = false;
        });

        officeDetailDialog.show();
    }

    public void downloadImageOfficeDatail(ImageView imageOffice, ProgressBar progressBar){
        Glide.with(getApplicationContext())
                .load(getValueFeaturesPointMap(Constants.IMAGE_OFICINAS_DE_ATENCION))
                .centerCrop()
                .error(R.mipmap.icon_oficina_por_defecto)
                .into(new GlideDrawableImageViewTarget(imageOffice) {
                    @Override
                    public void onResourceReady(GlideDrawable resource, GlideAnimation<? super GlideDrawable> animation) {
                        imageOffice.setImageDrawable(resource);
                        progressBar.setVisibility(View.VISIBLE);
                    }

                    @Override
                    public void onLoadFailed(Exception e, Drawable errorDrawable) {
                        progressBar.setVisibility(View.VISIBLE);
                        imageOffice.setImageResource(R.mipmap.icon_oficina_por_defecto);
                    }
                });
    }

    public void dialogEasyPointOffice() {
        controlDialogOfficeDetail = true;
        LayoutInflater factory = LayoutInflater.from(this);
        binding = DataBindingUtil.inflate(factory, R.layout.dialog_easy_point_office, null, false);
        officeDetailDialog = new AlertDialog.Builder(this).create();
        officeDetailDialog.setView(binding.getRoot());
        officeDetailDialog.setCancelable(false);
        String name = getValueFeaturesPointMap(Constants.TITLE_OFICINAS_DE_ATENCION).replaceFirst(Constants.kEY_REPLACE_TITLE_EASY_POINT_START,Constants.kEY_REPLACE_TITLE_EASY_POINT_END);
        binding.textNameOffice.setText(name);
        binding.textAddress.setText(getValueFeaturesPointMap(Constants.ADDRESS_OFICINAS_DE_ATENCION));
        binding.textSchedule.setText(getValueFeaturesPointMap(Constants.SCHEDULE_OFICINAS_DE_ATENCION));
        binding.progressBarOffice.setVisibility(View.VISIBLE);
        downloadImageEasyPointOffice(binding.imageOffice, binding.progressBarOffice);
        binding.btnAcept.setOnClickListener(view -> {
            officeDetailDialog.dismiss();
            controlDialogOfficeDetail = false;
        });
        officeDetailDialog.show();
    }

    public void downloadImageEasyPointOffice(ImageView imageOffice, ProgressBar progressBar){
        Glide.with(getApplicationContext())
                .load(getValueFeaturesPointMap(Constants.IMAGE_OFICINAS_DE_ATENCION))
                .centerCrop()
                .error(R.mipmap.icon_oficina_por_defecto)
                .into(new GlideDrawableImageViewTarget(imageOffice) {
                    @Override
                    public void onResourceReady(GlideDrawable resource, GlideAnimation<? super GlideDrawable> animation) {
                        imageOffice.setImageDrawable(resource);
                        progressBar.setVisibility(View.GONE);
                    }

                    @Override
                    public void onLoadFailed(Exception e, Drawable errorDrawable) {
                        progressBar.setVisibility(View.GONE);
                        imageOffice.setImageResource(R.mipmap.icon_oficina_por_defecto);
                    }
                });
    }

    private String getValueFeaturesPointMap(String itemSearch){
      String value = null;
      for(int i=0; i < featuresPointMap.getListFeaturesPointMapItem().size(); i++){
          if(featuresPointMap.getListFeaturesPointMapItem().get(i).getItemSearch().equalsIgnoreCase(itemSearch)){
             value = featuresPointMap.getListFeaturesPointMapItem().get(i).getValue();
          }
      }
      return value;
    }

    /**
     * @param informationOffice
     */
    protected void pointCenter(InformationOffice informationOffice) {
        mGraphicsOverlay.getGraphics().clear();
        if (informationOffice.getCurrentLatitud() == null && informationOffice.getCurrentLongitud() == null) {
            drawWithOutCurrentLocation(informationOffice);
        } else {
            drawWithCurrentLocation(informationOffice);
        }
    }

    private void drawWithOutCurrentLocation(InformationOffice informationOffice){
        Point centerPoint = new Point(informationOffice.getLongitud(),informationOffice.getLatitud(), SpatialReferences.getWgs84());
        Viewpoint viewpoint = new Viewpoint(centerPoint, 20000, 0);
        showPointInMap(centerPoint,R.mipmap.ic_is_here,-25,30);
        if(itDoZoom) {mapView.setViewpoint(viewpoint);itDoZoom = false;}
    }

    private void drawWithCurrentLocation(InformationOffice informationOffice){
        Point arrivePoint = new Point(informationOffice.getLongitud(), informationOffice.getLatitud(), SpatialReferences.getWgs84());
        Point currentPoint = new Point(informationOffice.getCurrentLongitud(), informationOffice.getCurrentLatitud(), SpatialReferences.getWgs84());
        Envelope initialEnvelope = new Envelope(currentPoint, arrivePoint);
        showPointInMap(currentPoint,R.mipmap.ic_is_here,-25,30);
        showPointInMap(arrivePoint,R.mipmap.icon_go_here,18,45);
        if(itDoZoom) {mapView.setViewpointGeometryAsync(initialEnvelope, paddingMap); itDoZoom = false;}
    }


    public void showPointInMap(Point point,int idImage,float offsetX,float offsetY) {
        BitmapDrawable pinStarBlueDrawable = (BitmapDrawable) ContextCompat.getDrawable(this, idImage);
        final PictureMarkerSymbol pinStarBlueSymbol = new PictureMarkerSymbol(pinStarBlueDrawable);
        pinStarBlueSymbol.setOffsetY(offsetY);
        pinStarBlueSymbol.setOffsetX(offsetX);
        pinStarBlueSymbol.loadAsync();
        pinStarBlueSymbol.addDoneLoadingListener(() -> addNewPoint(point, pinStarBlueSymbol));
    }

    /**
     * @param pintStarBlueSymbol
     */
    private void addNewPoint(Point pinStarBluePoint, PictureMarkerSymbol pintStarBlueSymbol) {
        Graphic pinStarBlueGraphic = new Graphic(pinStarBluePoint, pintStarBlueSymbol);
        mGraphicsOverlay.getGraphics().add(pinStarBlueGraphic);
    }

    /**
     *
     */
    protected void eventGoToOffice() {
        if (validateIfWazeIsInstalled()) {
            showProviderMap();
        } else {
            onChoseGoogleMapsClicked();
        }
    }

    /**
     *
     */
    private void showProviderMap() {
        if (validateIfFragmentIsNotNull()) {
            chooseMapProviderDialogFragment.show(getSupportFragmentManager(), "");
        }
    }

    /**
     * @return
     */
    private boolean validateIfFragmentIsNotNull() {
        return chooseMapProviderDialogFragment != null && !chooseMapProviderDialogFragment.isAdded();
    }

    /**
     * @return
     */
    private boolean validateIfWazeIsInstalled(){
        return Utils.isPackageInstalled(Constants.PACKAGE_WAZE, getApplicationContext().getPackageManager());
    }

    private boolean dataIsValidated(Object data){
        return data == null ? false : true;
    }


    protected void initInformation() {

    }

    protected void successValidateGPS() {

    }

    protected void cancelPermisonGPSBaseOffice(boolean defaultLocation) {

    }
}
