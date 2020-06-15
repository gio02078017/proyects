package com.epm.app.view.activities;
import com.epm.app.R;
import com.esri.arcgisruntime.geometry.Point;
import com.esri.arcgisruntime.geometry.SpatialReferences;
import com.esri.arcgisruntime.mapping.view.LocationDisplay;

import java.util.Map;
import java.util.Set;

import app.epm.com.utilities.entities.FeaturesPointMap;
import app.epm.com.utilities.helpers.ProcessXMLData;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by leidycarolinazuluagabastidas on 7/04/17.
 */

public class EstacionesDeGasActivity extends BaseOficinasDeAtencionOEstacionesDeGasActivity {

    private FeaturesPointMap featuresPointMap;

    public EstacionesDeGasActivity() {
        super(Constants.ESTACIONES_DE_GAS, Constants.URL_BASE_MAP, Constants.ID_MAP_ESTACIONES_DE_GAS) ;
    }

    @Override
    protected String getTitleCustom() {
        return Constants.TITLE_ESTACIONES_DE_GAS;
    }

    @Override
    public String getTitleActivity() {
        return getResources().getString(R.string.title_estaciones_de_gas);
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
    protected void setDataOfficePoint(Set<String> keys, Map<String, Object> attr) {
        featuresPointMap = ProcessXMLData.convertXMLToObject(this, Constants.NAME_FEATURE_POINT_MAP_GAS_STATION);
        featuresPointMap.setVerified(true);
        for (String key : keys) {
            Object value = attr.get(key);
            for (int i = 0; i < featuresPointMap.getListFeaturesPointMapItem().size(); i++){
                if(key.equalsIgnoreCase(featuresPointMap.getListFeaturesPointMapItem().get(i).getItemSearch())){
                    if(dataIsValidated(value)){
                        featuresPointMap.getListFeaturesPointMapItem().get(i).setValue(String.valueOf(value));
                    }else if(featuresPointMap.getListFeaturesPointMapItem().get(i).isRequire()){
                        featuresPointMap.setVerified(false);
                    }else if(featuresPointMap.getListFeaturesPointMapItem().get(i).isEmpty()){
                        featuresPointMap.getListFeaturesPointMapItem().get(i).setValue("");
                    }
                }
            }
        }
        evaluateFeaturesPointMap();
    }

    public void evaluateFeaturesPointMap(){
        if(featuresPointMap.isVerified()){
            dialogEasyPointOffice(featuresPointMap, false);
        }else{
            showAlertDialog(getString(R.string.text_title_it_is_not_information), getString(R.string.text_it_is_not_information));
        }
    }

    @Override
    protected void startLocation() {
        getLocationDisplayManager().startAsync();
        getLocationDisplayManager().addLocationChangedListener(locationChangedEvent -> {
            if (!isFirstPointLoaded() || isCallHandLocation()) {
                setFirstPointLoaded(true);
                setCallHandLocation(false);
                sendLoadLocation(locationChangedEvent);
            }
        });

    }

    private void sendLoadLocation(LocationDisplay.LocationChangedEvent locationChangedEvent){
        if(locationChangedEvent.getLocation() != null && locationChangedEvent.getLocation().getPosition() !=null)
            loadCurrentLocation(locationChangedEvent.getLocation().getPosition().getX(), locationChangedEvent.getLocation().getPosition().getY());
    }

    @Override
    protected void loadDefaulLocation() {
        Point centerPoint = new Point(-75.566667, 6.6516667, SpatialReferences.getWgs84());
        itDoesLoadLocation(centerPoint, 2000000);
    }

    private void loadCurrentLocation(double Latitud, double Longitud){
        Point centerPoint = new Point(Latitud, Longitud, SpatialReferences.getWgs84());
        itDoesLoadLocation(centerPoint, 100000);
    }
}
