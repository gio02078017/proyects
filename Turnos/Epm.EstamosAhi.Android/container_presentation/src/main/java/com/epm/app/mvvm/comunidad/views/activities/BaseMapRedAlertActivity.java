package com.epm.app.mvvm.comunidad.views.activities;

import android.Manifest;

import androidx.lifecycle.ViewModelProviders;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import androidx.databinding.DataBindingUtil;
import android.graphics.drawable.BitmapDrawable;
import android.os.Bundle;
import androidx.core.content.ContextCompat;
import androidx.appcompat.widget.Toolbar;
import android.view.MotionEvent;

import com.epm.app.R;
import com.epm.app.databinding.ActivityRedAlertInformationBinding;
import com.epm.app.mvvm.comunidad.models.Location;
import com.epm.app.mvvm.comunidad.viewModel.RedAlertViewModel;
import com.esri.arcgisruntime.ArcGISRuntimeEnvironment;
import com.esri.arcgisruntime.concurrent.ListenableFuture;
import com.esri.arcgisruntime.geometry.Envelope;
import com.esri.arcgisruntime.geometry.Point;
import com.esri.arcgisruntime.geometry.SpatialReference;
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
import com.esri.arcgisruntime.portal.Portal;
import com.esri.arcgisruntime.portal.PortalItem;
import com.esri.arcgisruntime.symbology.PictureMarkerSymbol;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.concurrent.ExecutionException;

import app.epm.com.utilities.controls.CustomTextViewBold;
import app.epm.com.utilities.controls.CustomTextViewNormal;
import app.epm.com.utilities.helpers.Permissions;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;

/**
 * Created by leidycarolinazuluagabastidas on 7/04/17.
 */

public abstract class BaseMapRedAlertActivity extends BaseActivityWithDI {


    private Toolbar toolbarApp;
    private Callout mCallout;
    private LocationDisplay locationDisplayManager;
    private boolean firstPointLoaded;
    private boolean openGpsSettings;
    private String modulo;
    private String urlBase;
    private String id;
    private ArcGISMap map;
    private Portal portal;
    private PortalItem portalItem;
    GraphicsOverlay mGraphicsOverlay;
    private CustomTextViewBold titlebar;
    private String titleActivity;
    private ActivityRedAlertInformationBinding binding;
    private final int PERMISION_GPS = 2;
    public Location location;


    public RedAlertViewModel redAlertViewModel;

    public BaseMapRedAlertActivity(String modulo, String urlBase, String id) {
        this.modulo = modulo;
        this.urlBase = urlBase;
        this.id = id;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this,R.layout.activity_red_alert_information);
        this.configureDagger();
        redAlertViewModel = ViewModelProviders.of(this,viewModelFactory).get(RedAlertViewModel.class);
        binding.setRedAlertViewModel((RedAlertViewModel) redAlertViewModel);
        createProgressDialog();
        mGraphicsOverlay = new GraphicsOverlay();
        binding.baseMapFrameLayoutArcgisMap.getGraphicsOverlays().add(mGraphicsOverlay);
        location = new Location();
        toolbarApp = (Toolbar) binding.toolbarRedAlert;
        loadToolbar();
        validateInternetToLoadMap();

    }

    public String getTitleActivity() {
        return titleActivity;
    }


    protected abstract String getTitleCustom();

    protected abstract String getAdrresCustom();

    protected abstract String getScheduleCustom();

    protected abstract String getImageCustom();

    protected abstract void onCreate();

    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        if(requestCode == PERMISION_GPS){
            if(grantResults[0] == PackageManager.PERMISSION_GRANTED){
                validateGpsIsOn();
            }else{
                cancelPermisonGPS(true);
            }
        }

        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    @Override
    public void cancelPermisonGPS(boolean defaultLocation) {
        if(defaultLocation){
          onCreate();
        }
    }

    private void loadMap() {
        ArcGISRuntimeEnvironment.setLicense(Constants.CUSTOMER_ID_ARCGIS);

        portal = new Portal(urlBase, false);
        portal.loadAsync();
        portal.addDoneLoadingListener(() -> {
            ArcGISRuntimeEnvironment.setLicense(Constants.ARCGIS_LICENSEINFO);
        });
        portalItem = new PortalItem(portal, id);
        map = new ArcGISMap(portalItem);

        binding.baseMapFrameLayoutArcgisMap.setMap(map);

        mCallout = binding.baseMapFrameLayoutArcgisMap.getCallout();


    }

    private void successValidateInternet(){
        loadMap();
        locationDisplayManager = binding.baseMapFrameLayoutArcgisMap.getLocationDisplay();
        locationDisplayManager.stop();
        loadLocation();
    }

    private void validateInternetToLoadMap() {
        if (getValidateInternet().isConnected()) {
            successValidateInternet();
            loadListener();
        } else {
            getCustomAlertDialog().showAlertDialog(getName(), R.string.text_validate_internet, false, R.string.text_intentar, new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialogInterface, int i) {
                    validateInternetToLoadMap();
                }
            }, R.string.text_cancelar, (dialogInterface, i) -> finish(), null);
        }
    }

    private void loadListener() {
         binding.baseMapFrameLayoutArcgisMap.setOnTouchListener(new DefaultMapViewOnTouchListener(this, binding.baseMapFrameLayoutArcgisMap) {
            @Override public boolean onSingleTapConfirmed(MotionEvent event) {
                if (mCallout.isShowing()) {
                    mCallout.dismiss();
                }

                android.graphics.Point screenPoint = new android.graphics.Point((int) event.getX(),
                        (int) event.getY());

                final ListenableFuture<List<IdentifyLayerResult>> identifyFuture =
                        binding.baseMapFrameLayoutArcgisMap.identifyLayersAsync(screenPoint, 10, false);

                identifyFuture.addDoneListener(() -> {
                    try {
                        List<IdentifyLayerResult> identifyLayersResults = identifyFuture.get();
                        if (identifyLayersResults.size() >= 1) {
                            IdentifyLayerResult identifyLayerResult = identifyLayersResults.get(0);
                            if (identifyLayerResult.getPopups().size() >= 1) {
                                Popup identifiedPopup = identifyLayerResult.getPopups().get(0);
                                Map<String, Object> attr = identifiedPopup.getGeoElement().getAttributes();
                                Set<String> keys = attr.keySet();
                                String description="";

                                for (String key : keys) {

                                    Object value = attr.get(key);
                                    if(key.equalsIgnoreCase(getTitleCustom())){
                                        description = (String) value.toString();
                                        showAlertDialogGeneralInformationOnUiThread(R.string.text_title_pop_red_alert,description);
                                    }

                                }
                            }
                        }
                    } catch (InterruptedException e) {

                    } catch (ExecutionException e) {

                    }
                });

                return super.onSingleTapConfirmed(event);
            }
        });


    }


    private void loadToolbar() {
        CustomTextViewNormal customTextViewNormal = toolbarApp.findViewById(R.id.toolbar_title);
        customTextViewNormal.setText(R.string.title_toolbar_red_alert);
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_white);
        this.toolbarApp.setNavigationOnClickListener(view -> onBackPressed());
    }

/*    private void hideTemplatePoint() {
        Animation animation = AnimationUtils.loadAnimation(BaseMapRedAlertActivity.this, R.anim.zoom_back_out);
        template_relativeLayout_generalView.startAnimation(animation);
        template_relativeLayout_generalView.setVisibility(View.GONE);
    }*/

    private void loadLocation() {
        if (Permissions.isGrantedPermissions(this, Manifest.permission.ACCESS_FINE_LOCATION)) {
            validateGpsIsOn();
        } else {
            String[] permissions = {Manifest.permission.ACCESS_FINE_LOCATION, Manifest.permission.ACCESS_COARSE_LOCATION};
            Permissions.verifyPermissions(this, permissions);
        }
    }

    private void validateGpsIsOn() {
        if (gpsIsOn()) {
            onCreate();
        } else {
            showGpsDialog();
        }
    }

    private void loadAntioquiaLocation() {
        Point centerPoint = new Point(-75.566667, 6.6516667, SpatialReferences.getWgs84());
        Viewpoint viewpoint = new Viewpoint(centerPoint, 2000000, 0);
        binding.baseMapFrameLayoutArcgisMap.setViewpoint(viewpoint);
    }

    public void startLocation() {
        locationDisplayManager.startAsync();
        locationDisplayManager.addLocationChangedListener(locationChangedEvent -> {
            if(locationChangedEvent.getLocation().getPosition() != null) {
                if (!firstPointLoaded) {
                    location.setArriveLocationLatitude(locationChangedEvent.getLocation().getPosition().getY());
                    location.setArriveLocationLongitude(locationChangedEvent.getLocation().getPosition().getX());
                    pointCenter();
                }
            }
        });
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

    public void pointCenter(){
        if(location.getArriveLocationLatitude() != 0 && location.getArriveLocationLongitude() != 0) {
            Point currentPoint = new Point(location.getCurrentLocationLongitude(), location.getCurrentLocationLatitude(), SpatialReferences.getWgs84());
            Point arrivePoint = new Point(location.getArriveLocationLongitude(), location.getArriveLocationLatitude(), SpatialReferences.getWgs84());
            Envelope initialEnvelope = new Envelope(currentPoint, arrivePoint);
            binding.baseMapFrameLayoutArcgisMap.setViewpointGeometryAsync(initialEnvelope, 125);
            showPointIsHereMap(arrivePoint);
            firstPointLoaded = true;
        }
        else {
            Point centerPoint = new Point(location.getCurrentLocationLongitude() ,location.getCurrentLocationLatitude(), SpatialReferences.getWgs84());
            Viewpoint viewpoint = new Viewpoint(centerPoint, 1000, 0);
            binding.baseMapFrameLayoutArcgisMap.setViewpoint(viewpoint);
        }

    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if(requestCode == Constants.ACTION_TO_TURN_ON_GPS){
            loadLocation();
        }
        super.onActivityResult(requestCode, resultCode, data);
    }

    //prueba de concepto


    public void showPointInMap(Double longitude, Double latitude) {
        BitmapDrawable pinStarBlueDrawable = (BitmapDrawable) ContextCompat.getDrawable(this, R.mipmap.icon_market_alert_red);
        final PictureMarkerSymbol pintStarBlueSymbol = new PictureMarkerSymbol(pinStarBlueDrawable);
        pintStarBlueSymbol.setOffsetY(38);
        pintStarBlueSymbol.setOffsetX(10);
        pintStarBlueSymbol.loadAsync();
        pintStarBlueSymbol.addDoneLoadingListener(() -> {
            Point pinStarBluePoint = new Point(longitude, latitude, SpatialReference.create(4326));
            Graphic pinStarBlueGraphic = new Graphic(pinStarBluePoint, pintStarBlueSymbol);
            mGraphicsOverlay.getGraphics().add(pinStarBlueGraphic);
        });

    }

    public void showPointIsHereMap(Point point){
        BitmapDrawable pinStarBlueDrawable = (BitmapDrawable) ContextCompat.getDrawable(this, R.mipmap.ic_is_here);
        final PictureMarkerSymbol pinStarBlueSymbol = new PictureMarkerSymbol(pinStarBlueDrawable);
        pinStarBlueSymbol.setOffsetY(30);
        pinStarBlueSymbol.setOffsetX(-25);
        pinStarBlueSymbol.loadAsync();
        pinStarBlueSymbol.addDoneLoadingListener(() -> {
            Graphic pinStarBlueGraphic = new Graphic(point, pinStarBlueSymbol);
            mGraphicsOverlay.getGraphics().add(pinStarBlueGraphic);
        });
    }

    @Override
    protected void onPause() {
        super.onPause();
        binding.baseMapFrameLayoutArcgisMap.pause();
    }

    @Override
    protected void onResume() {
        super.onResume();
        binding.baseMapFrameLayoutArcgisMap.resume();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        binding.baseMapFrameLayoutArcgisMap.dispose();
    }

    /*
    @Override
    public void onBackPressed() {
        if (template_relativeLayout_generalView.getVisibility() == View.VISIBLE) {
            hideTemplatePoint();
        } else {
            super.onBackPressed();
        }
    }*/



}
