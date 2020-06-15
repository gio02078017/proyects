package com.epm.app.mvvm.turn.viewModel;

import androidx.lifecycle.MutableLiveData;
import android.content.res.Resources;
import android.graphics.drawable.Drawable;
import android.location.Location;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.viewModel.BaseViewModel;
import com.epm.app.mvvm.turn.bussinesslogic.ITurnServicesBL;
import com.epm.app.mvvm.turn.network.request.RequestGetNearbyOffices;
import com.epm.app.mvvm.turn.network.response.nearbyOffices.NearbyOfficesItem;
import com.epm.app.mvvm.turn.network.response.nearbyOffices.NearbyOfficesResponse;
import com.epm.app.mvvm.turn.repository.TurnServicesRepository;
import com.epm.app.mvvm.turn.viewModel.iViewModel.INearbyOfficesViewModel;
import app.epm.com.utilities.helpers.ErrorMessage;
import com.epm.app.services.ServiceWithControlErrorGPS;
import java.util.List;
import javax.inject.Inject;
import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.utils.Constants;
import io.reactivex.Observable;


public class NearbyOfficesViewModel extends BaseViewModel implements INearbyOfficesViewModel {

    private ITurnServicesBL turnServicesBL;
    private ICustomSharedPreferences customSharedPreferences;
    private IValidateInternet validateInternet;
    private boolean waitLocation;
    private Resources resources;
    private List<NearbyOfficesItem> listNearbyOffices;
    private NearbyOfficesItem nearbyOfficesItem;
    private ServiceWithControlErrorGPS serviceWithControlErrorGPS;
    private RequestGetNearbyOffices solicitudObtenerOficinasCercanas;
    private String informationOffice = null;
    private NearbyOfficesResponse nearbyOfficesResponse;

    private MutableLiveData<Boolean> successNearbyOffices;
    public final MutableLiveData<String> textOfficeNameNearbyOffices;
    public final MutableLiveData<Integer> textColorOfficeNameNearbyOffices;
    public final MutableLiveData<String> textDistanceNearbyOffices;
    public final MutableLiveData<String> textStyleDistanceNearbyOffices;
    public final MutableLiveData<Integer> textColorDistanceNearbyOffices;
    public final MutableLiveData<String> textInformationNearbyOffices;
    public final MutableLiveData<String> textStyleInformationNearbyOffices;
    public final MutableLiveData<Integer> textColorInformationNearbyOffices;
    public final MutableLiveData<Drawable> imageNearbyOffice;
    public final MutableLiveData<Location> currentLocation;

    @Inject
    public NearbyOfficesViewModel(TurnServicesRepository turnServicesRepository,
                                  CustomSharedPreferences customSharedPreferences, ServiceWithControlErrorGPS serviceWithControlErrorGPS, ValidateInternet validateInternet) {
        this.turnServicesBL = turnServicesRepository;
        this.customSharedPreferences = customSharedPreferences;
        this.serviceWithControlErrorGPS = serviceWithControlErrorGPS;
        this.validateInternet = validateInternet;
        this.successNearbyOffices = new MutableLiveData<>();
        progressDialog = new MutableLiveData<>();
        currentLocation = new MutableLiveData<>();
        solicitudObtenerOficinasCercanas = new RequestGetNearbyOffices();
        this.textOfficeNameNearbyOffices = new MutableLiveData<>();
        this.textColorOfficeNameNearbyOffices = new MutableLiveData<>();
        this.imageNearbyOffice = new MutableLiveData<>();
        this.textInformationNearbyOffices = new MutableLiveData<>();
        this.textColorInformationNearbyOffices = new MutableLiveData<>();
        this.textStyleInformationNearbyOffices = new MutableLiveData<>();
        this.textDistanceNearbyOffices = new MutableLiveData<>();
        this.textColorDistanceNearbyOffices = new MutableLiveData<>();
        this.textStyleDistanceNearbyOffices = new MutableLiveData<>();
    }

    public NearbyOfficesViewModel(Resources resources) {
        this.textOfficeNameNearbyOffices = new MutableLiveData<>();
        this.textColorOfficeNameNearbyOffices = new MutableLiveData<>();
        this.imageNearbyOffice = new MutableLiveData<>();
        this.textInformationNearbyOffices = new MutableLiveData<>();
        this.textColorInformationNearbyOffices = new MutableLiveData<>();
        this.textStyleInformationNearbyOffices = new MutableLiveData<>();
        this.textDistanceNearbyOffices = new MutableLiveData<>();
        this.textColorDistanceNearbyOffices = new MutableLiveData<>();
        this.textStyleDistanceNearbyOffices = new MutableLiveData<>();
        this.resources = resources;
        currentLocation = new MutableLiveData<>();
    }

    @Override
    public void getLocation() {
        progressDialog.setValue(true);
        serviceWithControlErrorGPS.askLocation();
        waitLocation = true;
        serviceWithControlErrorGPS.getResponseLocation().observeForever(location -> {
            if (waitLocation) {
                waitLocation = false;
                currentLocation.setValue(location);
            }
        });
    }

    @Override
    public void getNearbyOffices(Location location) {
            putSolicitudObtenerOficinasCercanas(location);
            Observable<NearbyOfficesResponse> result = turnServicesBL.getNearbyOfficesResponse(customSharedPreferences.getString(Constants.TOKEN), solicitudObtenerOficinasCercanas);
            fetchService(result,validateInternet);
    }

    private static boolean isNearbyOfficesItemsNullOrEmpty(NearbyOfficesResponse responseOffices) {
        return responseOffices.getNearbyOfficesItems() != null && !responseOffices.getNearbyOfficesItems().isEmpty();
    }

    private void putSolicitudObtenerOficinasCercanas(Location location) {
        solicitudObtenerOficinasCercanas.setLatitud(String.valueOf(location.getLatitude()));
        solicitudObtenerOficinasCercanas.setLongitud(String.valueOf(location.getLongitude()));
        solicitudObtenerOficinasCercanas.setIdDispositivo(IdDispositive.getIdDispositive());
    }

    @Override
    public void showError() {
        serviceWithControlErrorGPS.showError().observeForever(errorMessage -> validateShowError(errorMessage));
    }

    @Override
    public MutableLiveData<Boolean> getSuccessNearbyOffices() {
        return successNearbyOffices;
    }

    @Override
    public List<NearbyOfficesItem> getListNearbyOffices() {
        return listNearbyOffices;
    }

    public void setListNearbyOffices(List<NearbyOfficesItem> listNearbyOffices) {
        this.listNearbyOffices = listNearbyOffices;
    }

    public RequestGetNearbyOffices getSolicitudObtenerOficinasCercanas() {
        return solicitudObtenerOficinasCercanas;
    }

    public void setSolicitudObtenerOficinasCercanas(RequestGetNearbyOffices solicitudObtenerOficinasCercanas) {
        this.solicitudObtenerOficinasCercanas = solicitudObtenerOficinasCercanas;
    }

    public NearbyOfficesItem getNearbyOfficesItem() {
        return nearbyOfficesItem;
    }

    public void setNearbyOfficesItem(NearbyOfficesItem nearbyOfficesItem) {
        this.nearbyOfficesItem = nearbyOfficesItem;
    }

    public void drawInformation() {
        this.textOfficeNameNearbyOffices.setValue(this.nearbyOfficesItem.getNombreOficina());
        this.textDistanceNearbyOffices.setValue(String.format(Constants.TEXT_CONCATENATE_DISTANCE, String.valueOf(this.nearbyOfficesItem.getDistancia())));
        if (this.nearbyOfficesItem.getTurnoAsignado() == null) {
            this.imageNearbyOffice.setValue(resources.getDrawable(R.drawable.ic_unselectedlocatenearbyoffices));
            textColorOfficeNameNearbyOffices.setValue(resources.getColor(R.color.colorTextGuideProceduresAndRequirements));
            textStyleInformationNearbyOffices.setValue("normal");
            textStyleDistanceNearbyOffices.setValue("normal");
            textColorInformationNearbyOffices.setValue(resources.getColor(R.color.color_text_information_nearby_offices));
            this.textInformationNearbyOffices.setValue(String.format(Constants.TEXT_CONCATENAT_TURN_IN_WAIT, String.valueOf(this.nearbyOfficesItem.getTurnosEnEspera())));
        } else {
            this.imageNearbyOffice.setValue(resources.getDrawable(R.drawable.ic_selectedlocatenearbyoffices));
            textColorOfficeNameNearbyOffices.setValue(resources.getColor(R.color.color_text_title_turns));
            textStyleInformationNearbyOffices.setValue("bold");
            textStyleDistanceNearbyOffices.setValue("bold");
            textColorInformationNearbyOffices.setValue(resources.getColor(R.color.color_red_state_ofice));
            this.textInformationNearbyOffices.setValue(String.format(Constants.TEXT_CONCATENAT_TURN_ASSIGNED, String.valueOf(this.nearbyOfficesItem.getTurnoAsignado())));
        }
    }

    public String getInformationOffice() {
        return informationOffice;
    }

    public void setInformationOffice(String informationOffice) {
        this.informationOffice = informationOffice;
    }

    public boolean isWaitLocation() {
        return waitLocation;
    }

    public void setWaitLocation(boolean waitLocation) {
        this.waitLocation = waitLocation;
    }

    public NearbyOfficesResponse getNearbyOfficesResponse() {
        return nearbyOfficesResponse;
    }

    public void setNearbyOfficesResponse(NearbyOfficesResponse nearbyOfficesResponse) {
        this.nearbyOfficesResponse = nearbyOfficesResponse;
    }

    @Override
    protected void handleResponse(Object responseService) {
        NearbyOfficesResponse nearbyOfficesResponse = (NearbyOfficesResponse) responseService;
        if (isNearbyOfficesItemsNullOrEmpty(nearbyOfficesResponse)) {
            setNearbyOfficesResponse(nearbyOfficesResponse);
            setListNearbyOffices(nearbyOfficesResponse.getNearbyOfficesItems());
            successNearbyOffices.setValue(true);
            validateTurn();
        } else {
            validateShowError(new ErrorMessage(R.string.title_error_oficinas, R.string.text_error_oficianas));
        }
        progressDialog.setValue(false);
    }

    public void validateTurn() {
        if (customSharedPreferences.getString(Constants.ASSIGNED_TRUN) != null) {
            deleteTurn(searchTurn());
        }
    }

    public boolean searchTurn() {
        boolean foundTurn = false;
        for (int i = 0; i < nearbyOfficesResponse.getNearbyOfficesItems().size(); i++) {
            if (nearbyOfficesResponse.getNearbyOfficesItems().get(i).getTurnoAsignado() != null) {
                foundTurn = true;
               // customSharedPreferences.addString(Constants.ASSIGNED_TRUN,nearbyOfficesResponse.getNearbyOfficesItems().get(i).getTurnoAsignado());
            }
        }
        return foundTurn;
    }

    public void deleteTurn(boolean foundTurn) {
        if (!foundTurn) {
            customSharedPreferences.deleteValue(Constants.ASSIGNED_TRUN);
            customSharedPreferences.deleteValue(Constants.INFORMATION_OFFICE_JSON);
        }
    }
}
