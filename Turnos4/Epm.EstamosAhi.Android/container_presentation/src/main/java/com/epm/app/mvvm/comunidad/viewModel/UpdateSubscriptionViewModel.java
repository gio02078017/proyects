package com.epm.app.mvvm.comunidad.viewModel;

import android.arch.lifecycle.MutableLiveData;
import android.util.Log;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.bussinesslogic.IPlacesBL;
import com.epm.app.mvvm.comunidad.bussinesslogic.ISubscriptionBL;
import com.epm.app.mvvm.comunidad.network.request.CancelSubscriptionRequest;
import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.CancelSubscriptionResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetSubscriptionNotificationsPushAlertasItuango;
import com.epm.app.mvvm.comunidad.network.response.places.Municipio;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.RequestUpdateSubscription;
import com.epm.app.mvvm.comunidad.network.response.places.Sectores;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.UpdateSubscription;
import com.epm.app.mvvm.comunidad.repository.PlacesRepository;
import com.epm.app.mvvm.comunidad.repository.SubscriptionRepository;

import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.ValidateInternet;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.IUpdateSubscriptionViewModel;

import static io.fabric.sdk.android.services.common.CommonUtils.isNullOrEmpty;

import java.util.List;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.helpers.Validations;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.DisposableManager;
import io.reactivex.Observable;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.Disposable;
import io.reactivex.observers.DisposableObserver;
import io.reactivex.schedulers.Schedulers;

public class UpdateSubscriptionViewModel extends BaseViewModel implements IUpdateSubscriptionViewModel {

    private ISubscriptionBL subscriptionBL;
    private ICustomSharedPreferences customSharedPreferences;
    private IPlacesBL placesBL;
    private GetSubscriptionNotificationsPushAlertasItuango subscription;
    private RequestUpdateSubscription solicitudActualizar;
    private CancelSubscriptionRequest cancelSubscriptionRequest;
    private List<Municipio> listMunicipio;
    private List<Sectores> listSector;
    private IValidateInternet validateInternet;
    public final MutableLiveData<Boolean> enableSector;
    public final MutableLiveData<Boolean>  enableMunicipio;
    public final MutableLiveData<String>  name;
    public final MutableLiveData<String>  lastName;
    public final MutableLiveData<String>  telephone;
    public final MutableLiveData<String>  email;
    public final MutableLiveData<String>  municipality;
    public final MutableLiveData<String>  sector;
    public final MutableLiveData<String> nameRecivied;
    public final MutableLiveData<String> lastNameRecivied;
    public final MutableLiveData<String> telephoneRecivied;
    public final MutableLiveData<String> emailRecivied;
    private MutableLiveData<String>  messageSubscriptionCanceled;
    private MutableLiveData<Mensaje> actionMessageRecoverySubscription;
    private MutableLiveData<Mensaje> actionMessageCancelSubscription;
    public final MutableLiveData<String> receivedMunicipality;
    private int idmuncipio;
    private int  idsector;
    private boolean validateMessage;
    public final MutableLiveData<Boolean> pushMunicipio;
    public final MutableLiveData<Boolean>  pushSector;
    private ErrorMessage errorMessage;

    @Inject
    public UpdateSubscriptionViewModel(SubscriptionRepository subscriptionRepository, CustomSharedPreferences customSharedPreferences,
                                       PlacesRepository placesRepository, ValidateInternet validateInternet) {
        this.subscriptionBL = subscriptionRepository;
        this.customSharedPreferences = customSharedPreferences;
        this.placesBL = placesRepository;
        name = new MutableLiveData<>();
        lastName = new MutableLiveData<>();
        telephone = new MutableLiveData<>();
        email = new MutableLiveData<>();
        municipality = new MutableLiveData<>();
        sector = new MutableLiveData<>();
        pushMunicipio = new MutableLiveData<>();
        pushSector = new MutableLiveData<>();
        enableMunicipio = new MutableLiveData<>();
        enableSector = new MutableLiveData<>();
        messageSubscriptionCanceled = new MutableLiveData<>();
        actionMessageRecoverySubscription = new MutableLiveData<>();
        actionMessageCancelSubscription = new MutableLiveData<>();
        nameRecivied = new MutableLiveData<>();
        lastNameRecivied = new MutableLiveData<>();
        telephoneRecivied = new MutableLiveData<>();
        emailRecivied = new MutableLiveData<>();
        solicitudActualizar = new RequestUpdateSubscription();
        cancelSubscriptionRequest = new CancelSubscriptionRequest();
        receivedMunicipality = new MutableLiveData<>();
        this.validateInternet = validateInternet;
        errorMessage = new ErrorMessage();

    }

    public void getSuscription() {
        progressDialog.setValue(true);
        subscriptionBL.getSubscriptionAlertasItuango(customSharedPreferences.getString(Constants.TOKEN), IdDispositive.getIdDispositive()).observeForever(this::loadSubscription);
        Log.e("municipios","getSuscription");
    }

    public void loadSubscription(GetSubscriptionNotificationsPushAlertasItuango subscription) {
        if (subscription != null) {
            validateCodeIdentify(subscription);

        }
    }

    private void validateCodeIdentify(GetSubscriptionNotificationsPushAlertasItuango subscription) {
        if(subscription.getMensaje().getIdentificador() == 22){
            titleMessage.setValue(subscription.getMensaje().getTitulo());
            actionMessageRecoverySubscription.setValue(subscription.getMensaje());
        }else{
            this.subscription = subscription;
            name.setValue(subscription.getSuscripcionNotificacionesPushComunidad().getName());
            lastName.setValue(subscription.getSuscripcionNotificacionesPushComunidad().getLastName());
            telephone.setValue(subscription.getSuscripcionNotificacionesPushComunidad().getTelephone());
            email.setValue(subscription.getSuscripcionNotificacionesPushComunidad().getEmail());
            Log.e("municipios","load Subscription");
            loadPlaces();
        }
    }

    public void loadPlaces() {
        Log.e("loadPlaces","loadPlaces");
        placesBL.getMunicipios(customSharedPreferences.getString(Constants.TOKEN)).observeForever(obtenerMunicipios -> {
            if (obtenerMunicipios.getMunicipios() != null && !obtenerMunicipios.getMunicipios().isEmpty()) {
                listMunicipio = obtenerMunicipios.getMunicipios();
                enableMunicipio.setValue(true);
                progressDialog.setValue(false);
                Log.e("municipios",obtenerMunicipios.getMunicipios().size()+"");
                showMunicipality(obtenerMunicipios.getMunicipios());

            }

        });
    }

    public void loadSector(int id) {
        Log.e("id sectores",id+"-");
        placesBL.getSectores(customSharedPreferences.getString(Constants.TOKEN), id).observeForever(obtenerSectores -> {
            if (obtenerSectores.getSectores() != null && !obtenerSectores.getSectores().isEmpty()) {
                enableSector.setValue(true);
                listSector = obtenerSectores.getSectores();
                if (this.subscription != null) {
                    showSector(obtenerSectores.sectores);
                }
            }
        });
    }

    public void showSector(List<Sectores> getSectores) {
        Log.e("sectores",getSectores.size()+"-");
        for (Sectores sectores : getSectores) {
            if (sectores.getIdSector() == this.subscription.getSuscripcionNotificacionesPushComunidad().getSector()) {
                idsector = sectores.getIdSector();
                Log.e("sectores id",sectores.getIdSector()+"-");
                this.sector.setValue(sectores.getSector());
                validateMessage = true;
                break;
            }
        }
        progressDialog.setValue(false);

    }

    public void showMunicipality(List<Municipio> municipioList) {
        for (Municipio municipio : municipioList) {
            if (municipio.getIdMunicipio() == this.subscription.getSuscripcionNotificacionesPushComunidad().getMunicipality()) {
                idmuncipio = municipio.getIdMunicipio();
                this.municipality.setValue(municipio.getDescripcion());
                loadSector(municipio.getIdMunicipio());
                break;
            }
        }
    }

    public boolean validateData() {
        if (isNullOrEmpty(nameRecivied.getValue())) {
            setError(R.string.title_empty_fields,R.string.text_empty_name);
            return false;
        }
        if (isNullOrEmpty(lastNameRecivied.getValue())) {
            setError(R.string.title_empty_fields,R.string.text_empty_lastname);
            return false;
        }
        if (isNullOrEmpty(telephoneRecivied.getValue())) {
            setError(R.string.title_empty_fields,R.string.text_empty_telephone);
            return false;
        }
        if (telephoneRecivied.getValue().length() != 10) {
            setError(R.string.title_empty_fields,R.string.text_error_telephone_number);
            return false;
        }
        if (isNullOrEmpty(emailRecivied.getValue())) {
            setError(R.string.title_empty_fields,R.string.text_empty_email);
            return false;
        }
        if (!Validations.validateEmail(emailRecivied.getValue())) {
            setError(R.string.title_empty_fields,R.string.text_incorrect_email);

            return false;
        }
        if (isNullOrEmpty(municipality.getValue())) {
            setError(R.string.title_empty_fields,R.string.text_empty_municipio);
            return false;
        }
        if (isNullOrEmpty(sector.getValue())) {
            setError(R.string.title_empty_fields,R.string.text_empty_sector);
            return false;
        }

        return true;

    }

    private void setError(int title, int message) {
        error.setValue(new ErrorMessage(title,message));
    }


    @Override
    public void showError() {
        subscriptionBL.showError().observeForever(errorMessage -> validateShowError(errorMessage));
        placesBL.showError().observeForever(errorMessage -> {
            receivedMunicipality.setValue(Constants.EMPTY_STRING);
            municipality.setValue(Constants.EMPTY_STRING);
            sector.setValue(Constants.EMPTY_STRING);
            validateShowError(errorMessage);
        });
    }

    public void updateSubscription() {
        if (validateData()) {
            progressDialog.setValue(true);
            saveDatesforUpdate();
            validateInternetForUpdateSubscription();
        }
    }

    public void validateInternetForUpdateSubscription(){
        if(validateInternet.isConnected()){
            serviceUpdateSubscription();
        }else{
            errorMessage.setTitle(R.string.title_appreciated_user);
            errorMessage.setMessage(R.string.text_validate_internet);
            validateShowError(errorMessage);
        }
    }

    public void serviceUpdateSubscription(){
        Observable<UpdateSubscription> result = subscriptionBL.updateSubscription(customSharedPreferences.getString(Constants.TOKEN), solicitudActualizar);
        Disposable disposable = result.subscribeOn(Schedulers.io())
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .retry(2)
                .subscribeWith(new DisposableObserver<UpdateSubscription>() {
                    @Override
                    public void onNext(UpdateSubscription updateResponse) {
                        setMessajeResponse(updateResponse.getMensaje().getTitulo(), updateResponse.getMensaje().getContenido());
                        validateActionMessageUpdateRecoverySubscription(updateResponse.getMensaje());
                    }

                    @Override
                    public void onError(Throwable e) {
                        setError(e);
                    }

                    @Override
                    public void onComplete() {
                        Log.e("complete ", "complete");
                    }
                });
        DisposableManager.add(disposable);

    }

    public void onClickMunicipio() {
        if (listMunicipio != null && !listMunicipio.isEmpty()) {
            pushMunicipio.setValue(true);
        }
    }

    public void onClickSectores() {
        if (listSector != null && !listSector.isEmpty()) {
            pushSector.setValue(true);
        }
    }

    public void saveDatesforUpdate() {
        solicitudActualizar.setName(nameRecivied.getValue());
        solicitudActualizar.setLastName(lastNameRecivied.getValue());
        solicitudActualizar.setTelephone(telephoneRecivied.getValue());
        solicitudActualizar.setAceptaTerminosCondiciones(subscription.getSuscripcionNotificacionesPushComunidad().isAcceptTermsConditions());
        solicitudActualizar.setEnvioNotificacion(subscription.getSuscripcionNotificacionesPushComunidad().isNotification());
        solicitudActualizar.setCorreoElectronico(emailRecivied.getValue());
        solicitudActualizar.setIdMunicipality(getIdmuncipio());
        solicitudActualizar.setIdSector(getIdsector());
        solicitudActualizar.setIdUser(customSharedPreferences.getString(Constants.USUARIO));
        solicitudActualizar.setIdSuscription(subscription.getSuscripcionNotificacionesPushComunidad().getIdSuscription());
        solicitudActualizar.setIdSuscripcionOneSignal(subscription.getSuscripcionNotificacionesPushComunidad().getIdOneSignal());
        solicitudActualizar.setIdDispositivo(subscription.getSuscripcionNotificacionesPushComunidad().getIdDispositive());
    }





    public void cancelSubscription() {
        progressDialog.setValue(true);
        setParameterCancelSubscriptionRequest(getIdSubscription(), IdDispositive.getIdDispositive());
        String token = customSharedPreferences.getString(Constants.TOKEN);
        CancelSubscriptionRequest cancelSubscriptionRequest = getCancelSubscriptionRequest();
        cancel(token, cancelSubscriptionRequest);

    }

    public void cancel(String token, CancelSubscriptionRequest cancelSubscriptionRequest) {
        if(validateInternet.isConnected() && cancelSubscriptionRequest != null){
            Observable<CancelSubscriptionResponse> result = subscriptionBL.cancelSubscription(token, cancelSubscriptionRequest);
             Disposable disposable = result.subscribeOn(Schedulers.io())
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .retry(2)
                    .subscribeWith(new DisposableObserver<CancelSubscriptionResponse>() {
                        @Override
                        public void onNext(CancelSubscriptionResponse cancelSubscriptionResponse) {
                            validateCancelTurn(cancelSubscriptionResponse);
                        }

                        @Override
                        public void onError(Throwable e) {
                            setError(e);
                        }

                        @Override
                        public void onComplete() {
                            Log.e("complete ", "complete");
                        }
                    });
             DisposableManager.add(disposable);
        }else {
            errorMessage.setTitle(R.string.title_appreciated_user);
            errorMessage.setMessage(R.string.text_validate_internet);
            validateShowError(errorMessage);
        }
    }

    public void validateCancelTurn(CancelSubscriptionResponse cancelSubscriptionResponse){
        progressDialog.setValue(false);
        customSharedPreferences.deleteValue(Constants.SUSCRIPTION_ALERTAS);
        setMessajeResponse(cancelSubscriptionResponse.getMessage().getTitulo(),cancelSubscriptionResponse.getMessage().getContenido());
        validateActionMessageCancelRecoverySubscription(cancelSubscriptionResponse.getMessage());
    }

    private void validateActionMessageCancelRecoverySubscription(Mensaje message) {
        if (message.getIdentificador() == Constants.SUBSCRIPTION_WENT_RECOVERY){
            actionMessageRecoverySubscription.setValue(message);
        }else{
            actionMessageCancelSubscription.setValue(message);
        }

    }

    private void validateActionMessageUpdateRecoverySubscription(Mensaje message) {
        if (message.getIdentificador() == Constants.SUBSCRIPTION_WENT_RECOVERY){
            actionMessageRecoverySubscription.setValue(message);
        }else{
            messageSuccesful.setValue(message.getContenido());
        }

    }



    private void setMessajeResponse(String title, String content) {
        titleMessage.setValue(title);
        messageSubscriptionCanceled.setValue(content);
        progressDialog.setValue(false);

    }



    public void setParameterCancelSubscriptionRequest(int idSubscription, String idDispositive) {
        cancelSubscriptionRequest.setIdSuscripcion(idSubscription);
        cancelSubscriptionRequest.setIdDispositivo(idDispositive);
    }

    public CancelSubscriptionRequest getCancelSubscriptionRequest() {
        return cancelSubscriptionRequest;
    }

    private int getIdSubscription() {
        if (subscription != null) {
            return subscription.getSuscripcionNotificacionesPushComunidad().idSuscription;
        }
        return 0;
    }





    @Override
    public List getListMunicipio() {
        return listMunicipio;
    }

    @Override
    public List<Sectores> getListSector() {
        return listSector;
    }

    public int getIdmuncipio() {
        return idmuncipio;
    }

    public void setIdmuncipio(int idmuncipio) {
        this.idmuncipio = idmuncipio;
    }

    public int getIdsector() {
        return idsector;
    }

    public void setIdsector(int idsector) {
        this.idsector = idsector;
    }

    public RequestUpdateSubscription getSolicitudActualizar() {
        return solicitudActualizar;
    }

    @Override
    public boolean isValidateMessage() {
        return validateMessage;
    }

    public void setSolicitudActualizar(RequestUpdateSubscription solicitudActualizar) {
        this.solicitudActualizar = solicitudActualizar;
    }

    public GetSubscriptionNotificationsPushAlertasItuango getSubscription() {
        return subscription;
    }

    public void setSubscription(GetSubscriptionNotificationsPushAlertasItuango subscription) {
        this.subscription = subscription;
    }

    public void setListMunicipio(List<Municipio> listMunicipio) {
        this.listMunicipio = listMunicipio;
    }

    public void setListSector(List<Sectores> listSector) {
        this.listSector = listSector;
    }

    public MutableLiveData<String> getMessageSubscriptionCanceled() {
        return messageSubscriptionCanceled;
    }

    public void setCancelSubscriptionRequest(CancelSubscriptionRequest cancelSubscriptionRequest) {
        this.cancelSubscriptionRequest = cancelSubscriptionRequest;
    }

    public MutableLiveData<Mensaje> getActionMessageRecoverySubscription() {
        return actionMessageRecoverySubscription;
    }


    public MutableLiveData<Mensaje> getActionMessageCancelSubscription() {
        return actionMessageCancelSubscription;
    }

    @Override
    protected void onCleared() {
        super.onCleared();
        DisposableManager.dispose();
    }
}
