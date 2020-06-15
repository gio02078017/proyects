package com.epm.app.mvvm.procedure.viewModel;

import androidx.lifecycle.MutableLiveData;
import android.content.res.Resources;
import android.graphics.drawable.Drawable;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.viewModel.BaseViewModel;
import com.epm.app.mvvm.procedure.bussinessLogic.IProcedureServicesBL;
import com.epm.app.mvvm.procedure.network.request.TypePersonRequest;
import com.epm.app.mvvm.procedure.network.response.TypePerson.TypePersonItem;
import com.epm.app.mvvm.procedure.network.response.TypePerson.TypePersonResponse;
import com.epm.app.mvvm.procedure.repository.ProcedureServicesRepository;
import com.epm.app.mvvm.procedure.viewModel.iViewModel.ITypePersonViewModel;

import java.util.List;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.ConvertUtilities;
import io.reactivex.Observable;

public class TypePersonViewModel extends BaseViewModel implements ITypePersonViewModel {


    private MutableLiveData<List<TypePersonItem>> listTypePerson;
    private IProcedureServicesBL procedureServicesBL;
    private IValidateInternet validateInternet;
    private ICustomSharedPreferences customSharedPreferences;
    private TypePersonItem typePersonItem;
    List<TypePersonItem> listTypePeople;
    public final MutableLiveData<String> textNameTypePerson;
    public final MutableLiveData<Drawable> imageTypePerson;
    public final MutableLiveData<Drawable> iconState;
    private Resources resources;


    @Inject
    public TypePersonViewModel(ProcedureServicesRepository procedureServicesBL, ValidateInternet validateInternet, CustomSharedPreferences customSharedPreferences) {
        this.procedureServicesBL = procedureServicesBL;
        this.validateInternet = validateInternet;
        this.customSharedPreferences = customSharedPreferences;
        this.listTypePerson = new MutableLiveData<>();
        this.textNameTypePerson = new MutableLiveData<>();
        this.imageTypePerson = new MutableLiveData<>();
        this.iconState = new MutableLiveData<>();
    }

    public TypePersonViewModel(Resources resources) {
        this.textNameTypePerson = new MutableLiveData<>();
        this.imageTypePerson = new MutableLiveData<>();
        this.iconState = new MutableLiveData<>();
        this.resources = resources;
    }


    @Override
    public void fetchTypePerson(TypePersonRequest typePersonRequest) {
        Observable<TypePersonResponse> result = procedureServicesBL.getTypePerson(typePersonRequest,customSharedPreferences.getString(Constants.TOKEN));
        fetchService(result, validateInternet);
    }

    @Override
    public MutableLiveData<List<TypePersonItem>> getListTypePerson() {
        return listTypePerson;
    }

    @Override
    public TypePersonItem getTypePersonItem(int position) {
        return listTypePeople.get(position);
    }

    @Override
    protected void handleResponse(Object responseService) {
        TypePersonResponse result = (TypePersonResponse) responseService;
        validateResponse(result);
    }

    public void validateResponse(TypePersonResponse result) {
        if (result != null && validateIfTypePersonIsNullOrEmpty(result)) {
            listTypePeople = result.getMasterProcess().getTypePersonItem();
            listTypePerson.setValue(result.getMasterProcess().getTypePersonItem());
        }
    }

    private boolean validateIfTypePersonIsNullOrEmpty(TypePersonResponse result) {
        if (result.getMasterProcess() != null && result.getMasterProcess().getTypePersonItem()!= null){
            return !result.getMasterProcess().getTypePersonItem().isEmpty();
        }
        return false;
    }

    public List<TypePersonItem> getListTypePeople() {
        return listTypePeople;
    }

    public void setListTypePeople(List<TypePersonItem> listTypePeople) {
        this.listTypePeople = listTypePeople;
    }

    public TypePersonItem getTypePersonItem() {
        return typePersonItem;
    }

    public void setTypePersonItem(TypePersonItem typePersonItem) {
        this.typePersonItem = typePersonItem;
    }

    public void drawInformation() {
        this.textNameTypePerson.setValue(this.typePersonItem.getTypePersonName());
        this.iconState.setValue(resources.getDrawable(getIconStatusButton(typePersonItem.isActive())));
        this.imageTypePerson.setValue(resources.getDrawable(getIconItem("ic_" + typePersonItem.getTypePersonId().toLowerCase())));
    }

    public int getIconItem(String iconName) {
        return ConvertUtilities.resourceId(Constants.TYPE_RESOURCE_DRAWABLE, iconName);
    }

    public int getIconStatusButton(boolean state) {
        return state ? R.drawable.ic_arrow_right_turns_green : R.drawable.ic_arrow_right_turns_gray;
    }

}
