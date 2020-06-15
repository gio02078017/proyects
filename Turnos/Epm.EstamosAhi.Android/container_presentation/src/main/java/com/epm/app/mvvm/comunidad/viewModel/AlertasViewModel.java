package com.epm.app.mvvm.comunidad.viewModel;

import androidx.lifecycle.MutableLiveData;

import com.epm.app.R;
import com.epm.app.business_models.business_models.Usuario;
import com.epm.app.mvvm.comunidad.bussinesslogic.ISubscriptionBL;
import com.epm.app.mvvm.comunidad.bussinesslogic.IPlacesBL;
import com.epm.app.mvvm.comunidad.network.response.places.Municipio;
import com.epm.app.mvvm.comunidad.network.response.places.Sectores;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.SolicitudSuscripcionNotificacionPush;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.Subscription;
import com.epm.app.mvvm.comunidad.repository.PlacesRepository;
import com.epm.app.mvvm.comunidad.repository.SubscriptionRepository;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.IAlertasViewModel;
import app.epm.com.utilities.helpers.ErrorMessage;

import static io.fabric.sdk.android.services.common.CommonUtils.isNullOrEmpty;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IdDispositive;

import java.util.List;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.Validations;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.DisposableManager;

public class AlertasViewModel extends BaseViewModel implements IAlertasViewModel {

    public final MutableLiveData<String> name;
    public final MutableLiveData<String> email;
    public final MutableLiveData<String> lastname;
    public final MutableLiveData<String> telephone;
    public final MutableLiveData<Boolean> information;
    public final MutableLiveData<Boolean> backPressed;
    public final MutableLiveData<String> municipio;
    public final MutableLiveData<String> sector;
    public final MutableLiveData<Boolean> privacity;
    public final MutableLiveData<Boolean> enableSector;
    public final MutableLiveData<Boolean> enableMunicipio;
    public final MutableLiveData<Boolean> pushMunicipio;
    public final MutableLiveData<Boolean> pushSector;
    private int errorUnauthorized;
    private MutableLiveData<String> responseService;
    private MutableLiveData<String> titleResponseService;
    private MutableLiveData<Boolean> progressDialog;
    private MutableLiveData<Boolean> expiredToken;
    private Validations validation;
    private String idUser;
    private List<Municipio> listMunicipio;
    private List<Sectores> listSector;
    private IPlacesBL placesRepository;
    private ISubscriptionBL subscriptionBL;
    private ICustomSharedPreferences customSharedPreferences;
    private int idSector;
    private boolean statusSubscription;
    private SolicitudSuscripcionNotificacionPush solicitudSuscripcion;


    @Inject
    public AlertasViewModel(PlacesRepository placesRepository, CustomSharedPreferences customSharedPreferences, SolicitudSuscripcionNotificacionPush solicitudSuscripcion, SubscriptionRepository subscriptionBL){
        this.placesRepository = placesRepository;
        email = new MutableLiveData<>();
        lastname = new MutableLiveData<>();
        name = new MutableLiveData<>();
        information = new MutableLiveData<>();
        information.setValue(false);
        telephone = new MutableLiveData<>();
        municipio = new MutableLiveData<>();
        validation = new Validations();
        privacity = new MutableLiveData<>();
        privacity.setValue(false);
        enableSector = new MutableLiveData<>();
        pushMunicipio = new MutableLiveData<>();
        pushMunicipio.setValue(false);
        pushSector = new MutableLiveData<>();
        pushSector.setValue(false);
        sector = new MutableLiveData<>();
        backPressed = new MutableLiveData<>();
        backPressed.setValue(false);
        responseService = new MutableLiveData<>();
        titleResponseService = new MutableLiveData<>();
        enableMunicipio = new MutableLiveData<>();
        expiredToken = new MutableLiveData<>();
        this.customSharedPreferences = customSharedPreferences;
        this.solicitudSuscripcion = solicitudSuscripcion;
        progressDialog = new MutableLiveData<>();
        this.subscriptionBL = subscriptionBL;



    }

    public void loadInfoProfile(Usuario usuario) {
        if (validation.dataIsNotNull(usuario)){
            validateUserisguest(usuario);
        }
    }


    public void validateUserisguest(Usuario usuario){
        if(!usuario.isInvitado()){
            email.setValue(usuario.getCorreoElectronico());
            name.setValue(usuario.getNombres());
            lastname.setValue(usuario.getApellido());
            telephone.setValue(usuario.getCelular());
            idUser = usuario.getNombres();
        }else{
            idUser = Constants.ID_USER;
        }

    }

    public void onBackPressedArrow(){
        backPressed.setValue(true);
    }

    public void onClickMunicipio(){
        if(listMunicipio != null && !listMunicipio.isEmpty()) {
            pushMunicipio.setValue(true);
        }
    }

    public void onClickSectores(){
        if(listSector != null && !listSector.isEmpty()){
            pushSector.setValue(true);
        }

    }

    @Override
    public void loadMunicipio(List<Municipio> list){
        if(list != null && !list.isEmpty()){
            setListMunicipio(list);
            enableMunicipio.setValue(true);
        }
    }

    public void showError(){
        subscriptionBL.showError().observeForever(errorMessage -> validateErrorCode(errorMessage));
        placesRepository.showError().observeForever(errorMessage -> {
            validateErrorCode(errorMessage);
            municipio.setValue(Constants.EMPTY_STRING);
            sector.setValue(Constants.EMPTY_STRING);
        });


    }

    public void validateErrorCode(ErrorMessage errorMessage){
        if(ValidateServiceCode.getErrorCode() == Constants.UNAUTHORIZED_ERROR_CODE){
            errorUnauthorized=error.getValue().getMessage();
            expiredToken.setValue(true);
        }else{
            this.error.setValue(errorMessage);
        }
    }


    @Override
    public void loadSector(int id){
        progressDialog.setValue(true);
        placesRepository.getSectores(customSharedPreferences.getString(Constants.TOKEN),id).observeForever(obtenerSectores -> {
            if(obtenerSectores != null ){
                if(obtenerSectores.getSectores() != null && !obtenerSectores.getSectores().isEmpty()) {
                    setListSector(obtenerSectores.sectores);
                    enableSector.setValue(true);
                    progressDialog.setValue(false);
                }
            }
        });
    }


    public void subscription() {
        if(validateDates()){
            progressDialog.setValue(true);
            saveSuscriptionData();
            subscriptionBL.saveSubscription(customSharedPreferences.getString(Constants.TOKEN),solicitudSuscripcion).observeForever(saveSuscription -> {
                progressDialog.setValue(false);
                validateSubscription(saveSuscription);
                validateSuscriptionMessage(saveSuscription.getMensaje().getTitulo(), saveSuscription.getMensaje().getContenido());
            });
        }
    }

    public void validateSubscription(Subscription saveSubscription){
        if(saveSubscription.stateTransaction) {
            customSharedPreferences.addString(Constants.SUSCRIPTION_ALERTAS, Constants.TRUE);
            statusSubscription = true;
        }else {
            statusSubscription = false;
        }
    }

    public void validateSuscriptionMessage(String titleMessage,String message){
         if((message != null && titleMessage !=null) && (!message.isEmpty() && !titleMessage.isEmpty())){
             titleResponseService.setValue(titleMessage);
             responseService.setValue(message);
         }
    }

    public void saveSuscriptionData(){
        solicitudSuscripcion.setName(name.getValue());
        solicitudSuscripcion.setLastName(lastname.getValue());
        solicitudSuscripcion.setCelular(telephone.getValue());
        solicitudSuscripcion.setEmail(email.getValue());
        solicitudSuscripcion.setIdSector(idSector);
        solicitudSuscripcion.setNotification(information.getValue().booleanValue());
        solicitudSuscripcion.setTermsConditions(privacity.getValue().booleanValue());
        solicitudSuscripcion.setIdUser(idUser);
        solicitudSuscripcion.setIdOneSignal(customSharedPreferences.getString(Constants.ONE_SIGNAL_ID));
        solicitudSuscripcion.setIdDispositve(IdDispositive.getIdDispositive());
        solicitudSuscripcion.setIdTypeSuscription(Constants.TYPE_SUSCRIPTION_ALERTAS);

    }


    public boolean validateDates(){
        if(isNullOrEmpty(name.getValue())){
            setError(R.string.title_empty_fields,R.string.text_empty_name);
            return false;
        }
        if(isNullOrEmpty(lastname.getValue())){
            setError(R.string.title_empty_fields,R.string.text_empty_lastname);
            return false;
        }
        if(isNullOrEmpty(telephone.getValue())){
            setError(R.string.title_empty_fields,R.string.text_empty_telephone);
            return false;
        }
        if(telephone.getValue().length() != 10){
            setError(R.string.title_empty_fields,R.string.text_error_telephone_number);
            return false;
        }
        if(isNullOrEmpty(email.getValue())){
            setError(R.string.title_empty_fields,R.string.text_empty_email);
            return false;
        }
        if(!Validations.validateEmail(email.getValue())){
            setError(R.string.title_empty_fields,R.string.text_incorrect_email);
            return false;
        }
        if (isNullOrEmpty(municipio.getValue())) {
            setError(R.string.title_empty_fields,R.string.text_empty_municipio);
            return false;
        }
        if(isNullOrEmpty(sector.getValue())) {
            setError(R.string.title_empty_fields,R.string.text_empty_sector);
            return false;
        }
        if(!information.getValue().booleanValue()){
            setError(R.string.title_empty_information,R.string.text_empty_information);
            return false;
        }
        if(!privacity.getValue().booleanValue()){
            setError(R.string.title_empty_privacy,R.string.text_empty_privacy);
            return false;
        }

        return true;

    }

    private void setError(int title, int message) {
        error.setValue(new ErrorMessage(title,message));
    }


    public void setListMunicipio(List<Municipio> listMunicipio) {
        this.listMunicipio = listMunicipio;
    }

    public void setListSector(List<Sectores> listSector) {
        this.listSector = listSector;
    }


    public SolicitudSuscripcionNotificacionPush getSolicitudSuscripcion() {
        return solicitudSuscripcion;
    }



    public void setIdSector(int idSector) {
        this.idSector = idSector;
    }

    @Override
    public int getErrorUnauthorized() {
        return errorUnauthorized;
    }

    @Override
    public MutableLiveData<String> getResponseService() {
        return responseService;
    }

    @Override
    public MutableLiveData<String> getTitleResponseService() {
        return titleResponseService;
    }



    public void setSolicitudSuscripcion(SolicitudSuscripcionNotificacionPush solicitudSuscripcion) {
        this.solicitudSuscripcion = solicitudSuscripcion;
    }



    @Override
    public List getListMunicipio() {
        return listMunicipio;
    }
    @Override
    public List<Sectores> getListSector() {
        return listSector;
    }

    public MutableLiveData<Boolean> getProgressDialog() {
        return progressDialog;
    }

    public boolean isStatusSubscription() {
        return statusSubscription;
    }

    @Override
    protected void onCleared() {
        super.onCleared();
        DisposableManager.dispose();
    }

}
