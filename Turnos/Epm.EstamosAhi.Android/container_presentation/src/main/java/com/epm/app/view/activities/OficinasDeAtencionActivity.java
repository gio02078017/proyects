package com.epm.app.view.activities;
import androidx.lifecycle.ViewModelProviders;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;

import com.epm.app.R;
import com.epm.app.mvvm.procedure.models.ProcedureInformation;
import com.epm.app.mvvm.turn.network.response.AssignedTurn;
import com.epm.app.mvvm.turn.viewModel.OficinasDeAtencionViewModel;
import com.epm.app.mvvm.turn.viewModel.iViewModel.IOficinasDeAtencionViewModel;
import com.epm.app.mvvm.turn.views.activities.BaseOfficeDetailActivity;
import com.epm.app.mvvm.turn.views.activities.OfficeDetailActivity;
import com.epm.app.mvvm.turn.views.activities.ShiftInformationActivity;
import com.epm.app.mvvm.util.CustomLocation;
import app.epm.com.utilities.helpers.InformationOffice;
import com.esri.arcgisruntime.geometry.Point;
import com.esri.arcgisruntime.geometry.SpatialReferences;
import com.esri.arcgisruntime.mapping.view.LocationDisplay;

import java.util.Map;
import java.util.Set;

import app.epm.com.utilities.entities.FeaturesPointMap;
import app.epm.com.utilities.entities.FeaturesPointMapItem;
import app.epm.com.utilities.helpers.ProcessXMLData;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by leidycarolinazuluagabastidas on 7/04/17.
 */

public class OficinasDeAtencionActivity extends BaseOficinasDeAtencionOEstacionesDeGasActivity{


    private IOficinasDeAtencionViewModel oficinasDeAtencionViewModel;
    private FeaturesPointMap featuresPointMap;
    private InformationOffice informationOffice;
    private InformationOffice informationOfficeTurn;
    private boolean callHandOrServiceLocation = false;
    private boolean controlCallOfficeDetailActivity;
    private CustomLocation customLocation;
    private Bundle datosBeforeActivity;
    private ProcedureInformation procedureInformation;


    public OficinasDeAtencionActivity() {
        super(Constants.OFICINAS_DE_ATENCION, Constants.URL_BASE_MAP, Constants.ID_MAP_OFICINAS_DE_ATENCION );
        informationOffice = new InformationOffice();
    }

    @Override
    protected void onStart() {
        super.onStart();
        controlCallOfficeDetailActivity = false;
    }

    @Override
    public String getTitleActivity() {
        return getResources().getString(R.string.title_oficinas_de_atencion);
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.configureDagger();
        oficinasDeAtencionViewModel = ViewModelProviders.of(this,viewModelFactory).get(OficinasDeAtencionViewModel.class);
        loadBinding();
        getInformationBeforeActivity();
    }

    protected void loadService() {
        validateAlertDialog();
        oficinasDeAtencionViewModel.getAssignedTurn();
    }

    private void loadBinding() {
        oficinasDeAtencionViewModel.getProgressDialog().observe(this, progress -> {
            if (progress != null){
                showOrDimissProgressBar(progress);
            }
        });
        oficinasDeAtencionViewModel.getError().observe(this, errorMessage -> {
            dismissProgressDialog();
            validateAlertDialog();
            showAlertDialogTryAgain(errorMessage.getTitle(),errorMessage.getMessage(),R.string.text_intentar, R.string.text_cancelar);
        });
        oficinasDeAtencionViewModel.getResponseAssignedTurn().observe(this, response -> {
            if (response != null){
                drawTurn(response);
            }
        });
        oficinasDeAtencionViewModel.getWithOutAssignedTurn().observe(this, withOutTurn -> {
            if(withOutTurn != null){
                drawErrorOrDoesNotTurn();
            }
        });
        oficinasDeAtencionViewModel.getExpiredToken().observe(this, errorMessage ->{
            setExpiredToken(true);
            showAlertDialogUnauthorized(errorMessage.getTitle(),errorMessage.getMessage(),R.string.accept_button_text);
        } );
    }

    private void getInformationBeforeActivity(){
        datosBeforeActivity = this.getIntent().getExtras();
        if(datosBeforeActivity.getSerializable(Constants.EXTRAS_LOCATION) != null){
            customLocation = (CustomLocation)datosBeforeActivity.getSerializable(Constants.EXTRAS_LOCATION);
        }
        if(datosBeforeActivity.getSerializable(Constants.PROCEDURE_INFORMATION) != null){
            procedureInformation = (ProcedureInformation)datosBeforeActivity.getSerializable(Constants.PROCEDURE_INFORMATION);
        }
    }

    @Override
    protected String  getTitleCustom() {
        return Constants.TITLE_OFICINAS_DE_ATENCION;
    }

    @Override
    protected String getAdrresCustom() {
        return Constants.ADDRESS_OFICINAS_DE_ATENCION;
    }

    @Override
    protected String getScheduleCustom() {
        return Constants.SCHEDULE_OFICINAS_DE_ATENCION;
    }

    @Override
    protected String getImageCustom() {
        return Constants.IMAGE_OFICINAS_DE_ATENCION;
    }

    @Override
    protected void setDataOfficePoint(Set<String> keys, Map<String, Object> attr) {
        featuresPointMap = ProcessXMLData.convertXMLToObject(this, Constants.NAME_FEATURE_ATTENTION_OFFICES);
        featuresPointMap.setVerified(true);
        for (String key : keys) {
            Object value = attr.get(key);
            validateListPointMap(value,key);
        }
        evaluateFeaturesPointMap();
    }

    private void validateListPointMap(Object value,String key){
        for (int i = 0; i < featuresPointMap.getListFeaturesPointMapItem().size(); i++){
            if(key.equalsIgnoreCase(featuresPointMap.getListFeaturesPointMapItem().get(i).getItemSearch())){
               validateDataModal(value,featuresPointMap.getListFeaturesPointMapItem().get(i));
            }
        }
    }

    private void validateDataModal(Object value,FeaturesPointMapItem featuresPointMapItem){
        if(dataIsValidated(value)){
            featuresPointMapItem.setValue(String.valueOf(value));
        }else if(featuresPointMapItem.isRequire()){
            featuresPointMap.setVerified(false);
        }else if(featuresPointMapItem.isEmpty()){
            featuresPointMapItem.setValue("");
        }
    }

    public void evaluateFeaturesPointMap(){
        if(featuresPointMap.isVerified()){
            boolean itIsEasyPoint = itIsEasyPoint(featuresPointMap, Constants.IT_IS_EASY_POINT);
            evaluateEasyPoint(itIsEasyPoint, featuresPointMap);
        }else{
            showAlertDialog(getString(R.string.text_title_it_is_not_information), getString(R.string.text_it_is_not_information));
        }
    }

    @Override
    protected void startLocation() {
        if(!toDoZoom()) {
            getLocationDisplayManager().startAsync();
            getLocationDisplayManager().addLocationChangedListener(locationChangedEvent -> {
                setLocation(locationChangedEvent);
                if (!isFirstPointLoaded()) {
                    sendLoadLocation(locationChangedEvent);
                    setCallHandLocation(false);
                    setFirstPointLoaded(true);
                }
            });
        }
    }

    private void sendLoadLocation(LocationDisplay.LocationChangedEvent locationChangedEvent){
        if(locationChangedEvent.getLocation() != null && locationChangedEvent.getLocation().getPosition() !=null){
            loadCurrentLocation(locationChangedEvent.getLocation().getPosition().getX(), locationChangedEvent.getLocation().getPosition().getY());
            doPointCenter(locationChangedEvent.getLocation().getPosition().getX(), locationChangedEvent.getLocation().getPosition().getY());
        }
    }

    private void doPointCenter(double latitud, double longitud){
        if(callHandOrServiceLocation || isCallHandLocation() || !isFirstPointLoaded()){
            callHandOrServiceLocation = false;
            setCallHandLocation(false);
            pointCenter(latitud, longitud);
        }
    }

    @Override
    protected void loadDefaulLocation() {
        Point centerPoint = new Point(-75.566667, 6.6516667, SpatialReferences.getWgs84());
        double scale = 2000000;

        if(customLocation != null){
            centerPoint = new Point(customLocation.getLongitud(), customLocation.getLatitud(), SpatialReferences.getWgs84());
            scale = customLocation.getScale();
        }

        itDoesLoadLocation(centerPoint, scale);
    }

    private void loadCurrentLocation(double Latitud, double Longitud){
        Point centerPoint = new Point(Latitud, Longitud, SpatialReferences.getWgs84());
        itDoesLoadLocation(centerPoint, 30000);
    }

    public boolean itIsEasyPoint(FeaturesPointMap featuresPointMap, String itemSearch){
        String easyPoint = getValueFeaturesPointMap(featuresPointMap, itemSearch);
        if(easyPoint != null && !easyPoint.equalsIgnoreCase(""))  {
            if(easyPoint.equalsIgnoreCase("SI")) return true;
        }
        return false;
    }

    public void evaluateEasyPoint(boolean itIsEasyPoint, FeaturesPointMap featuresPointMap){
        if(itIsEasyPoint){
            dialogEasyPointOffice(featuresPointMap, false);
        }else{
            sendInformation(itIsEasyPoint, featuresPointMap);
        }
    }

    public void sendInformation(boolean itIsEasyPoint, FeaturesPointMap featuresPointMap){
        informationOffice.setNombreOficina(getValueFeaturesPointMap(featuresPointMap, getTitleCustom()));
        informationOffice.setIdOficinaSentry(getValueFeaturesPointMap(featuresPointMap, Constants.ID_OFICINAS_DE_ATENCION));
        informationOffice.setLatitud(Double.parseDouble(getValueFeaturesPointMap(featuresPointMap, Constants.LATITUD_OFICINAS_DE_ATENCION)));
        informationOffice.setLongitud(Double.parseDouble(getValueFeaturesPointMap(featuresPointMap, Constants.LONGITUD_OFICINAS_DE_ATENCION)));
        informationOffice.setPuntoFacil(itIsEasyPoint);
        startActivityDetail(informationOffice);
    }

    private void startActivityDetail(InformationOffice informationOffice){
        if(!controlCallOfficeDetailActivity){
            startActivityWithOutDoubleClick(validateTurnAssigned(informationOffice));
            controlCallOfficeDetailActivity = true;
            setCallActivity(true);
        }
    }

    private Intent validateTurnAssigned(InformationOffice informationOffice){
        Intent intent;
        if (informationOfficeTurn != null && informationOffice.getIdOficinaSentry().equalsIgnoreCase(informationOfficeTurn.getIdOficinaSentry())) {
            intent = new Intent(OficinasDeAtencionActivity.this, ShiftInformationActivity.class);
        } else {
            intent = new Intent(OficinasDeAtencionActivity.this, OfficeDetailActivity.class);
        }
        AddProcedureInformation(intent);
        intent.putExtra(Constants.INFORMATION_OFFICE, informationOffice);
        return intent;
    }

    public void AddProcedureInformation(Intent intent){
        if(procedureInformation != null){
            intent.putExtra(Constants.PROCEDURE_INFORMATION, procedureInformation);
        }
    }

    protected void cancelDialogError(DialogInterface dialogInterface) {
        dialogInterface.dismiss();
        informationOfficeTurn = null;
        callHandOrServiceLocation = true;
        startLocation();
    }

    protected void callService(){
        getBase_mapView().getGraphicsOverlays().clear();
        setStartGraphicsOverlay(false);
        validateInternetToLoadMap();
    }

    public void drawTurn(AssignedTurn assignedTurn){
        informationOfficeTurn = new InformationOffice();
        informationOfficeTurn.setNombreOficina(assignedTurn.getOffice().getNombre());
        informationOfficeTurn.setIdOficinaSentry(assignedTurn.getOffice().getIdOficinaSentry());
        informationOfficeTurn.setLatitud(Double.parseDouble(assignedTurn.getOffice().getLatitud().toString()));
        informationOfficeTurn.setLongitud(Double.parseDouble(assignedTurn.getOffice().getLongitud().toString()));
        informationOfficeTurn.setPuntoFacil(false);
        setInformationOfficeTurn(informationOfficeTurn);
        callHandOrServiceLocation = true;
        startLocation();
    }

    public void drawErrorOrDoesNotTurn(){
        informationOfficeTurn = null;
        setInformationOfficeTurn(informationOfficeTurn);
        callHandOrServiceLocation = true;
        startLocation();
    }

    protected void showAlertDialogTryAgain(final int title, final int message, final int positive, final int negative) {
        runOnUiThread(() -> {
                getCustomAlertDialog().showAlertDialog((!getUsuario().isInvitado() && title == R.string.title_appreciated_user) ? getUsuario().getNombres(): getString(title), message, false, positive, (dialogInterface, i) -> {
                    callService();
                }, negative, (dialogInterface, i) -> cancelDialogError(dialogInterface) , null);
        });
    }

    public void showAlertDialogUnauthorized(final int title, final int message, final int positive) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(title, message, false,positive, (dialogInterface, i) -> {
            //setResultData(Constants.UNAUTHORIZED);
            Intent intent = new Intent(OficinasDeAtencionActivity.this, LandingActivity.class);
            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
            intent.putExtra("token",Constants.UNAUTHORIZED);
            startActivity(intent);
            finish();
        }, null));
    }
}
