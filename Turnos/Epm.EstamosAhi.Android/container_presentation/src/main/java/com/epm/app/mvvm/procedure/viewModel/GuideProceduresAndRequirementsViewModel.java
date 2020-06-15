package com.epm.app.mvvm.procedure.viewModel;

import androidx.lifecycle.MutableLiveData;
import android.content.res.Resources;
import android.graphics.drawable.Drawable;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.viewModel.BaseViewModel;
import com.epm.app.mvvm.procedure.bussinessLogic.IProcedureServicesBL;
import com.epm.app.mvvm.procedure.repository.ProcedureServicesRepository;
import com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory.GuideProceduresAndRequirementsCategoryItem;
import com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory.GuideProceduresAndRequirementsCategoryResponse;
import com.epm.app.mvvm.turn.viewModel.iViewModel.IGuideProceduresAndRequirementsViewModel;

import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.ValidateInternet;

import java.util.List;
import java.util.Objects;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.ConvertUtilities;
import io.reactivex.Observable;

public class GuideProceduresAndRequirementsViewModel extends BaseViewModel implements IGuideProceduresAndRequirementsViewModel {

    public final MutableLiveData<String> textNameCategory;
    public final MutableLiveData<Drawable> imageGuideProceduresAndRequirementsCategory;
    public final MutableLiveData<Drawable> iconState;
    private MutableLiveData<List<GuideProceduresAndRequirementsCategoryItem>> listGuideProceduresAndRequirementsCategory;
    private IProcedureServicesBL procedureServicesBL;
    private IValidateInternet validateInternet;
    private ICustomSharedPreferences customSharedPreferences;
    private Resources resources;
    private GuideProceduresAndRequirementsCategoryItem guideProceduresAndRequirementsCategoryItem;

    @Inject
    public GuideProceduresAndRequirementsViewModel(ProcedureServicesRepository procedureServicesRepository, CustomSharedPreferences customSharedPreferences, ValidateInternet validateInternet) {
        this.procedureServicesBL = procedureServicesRepository;
        this.customSharedPreferences = customSharedPreferences;
        this.textNameCategory = new MutableLiveData<>();
        this.listGuideProceduresAndRequirementsCategory = new MutableLiveData<>();
        this.validateInternet = validateInternet;
        this.imageGuideProceduresAndRequirementsCategory = new MutableLiveData<>();
        this.iconState = new MutableLiveData<>();
    }

    public GuideProceduresAndRequirementsViewModel(Resources resources) {
        this.textNameCategory = new MutableLiveData<>();
        this.imageGuideProceduresAndRequirementsCategory = new MutableLiveData<>();
        this.iconState = new MutableLiveData<>();
        this.resources = resources;
    }

    @Override
    public void getGuideProceduresAndRequirementsCategories() {
        Observable<GuideProceduresAndRequirementsCategoryResponse> result = procedureServicesBL.getGuideProceduresAndRequirementsCategory(customSharedPreferences.getString(Constants.TOKEN));
        fetchService(result, validateInternet);
    }

    public void drawInformation() {
        this.textNameCategory.setValue(this.guideProceduresAndRequirementsCategoryItem.getCategoryName());
        this.iconState.setValue(resources.getDrawable(getIconStatusButton(guideProceduresAndRequirementsCategoryItem.isState())));
        this.imageGuideProceduresAndRequirementsCategory.setValue(resources.getDrawable(getIconItem("ic_" + guideProceduresAndRequirementsCategoryItem.getCategoryId().toLowerCase())));
    }

    public int getIconItem(String iconName) {
        return ConvertUtilities.resourceId(Constants.TYPE_RESOURCE_DRAWABLE, iconName);
    }

    public int getIconStatusButton(boolean state) {
        return state ? R.drawable.ic_arrow_right_turns_green : R.drawable.ic_arrow_right_turns_gray;
    }

    @Override
    public MutableLiveData<List<GuideProceduresAndRequirementsCategoryItem>> getListGuideProceduresAndRequirementsCategory() {
        return listGuideProceduresAndRequirementsCategory;
    }

    public void setGuideProceduresAndRequirementsCategoryItem(GuideProceduresAndRequirementsCategoryItem guideProceduresAndRequirementsCategoryItem) {
        this.guideProceduresAndRequirementsCategoryItem = guideProceduresAndRequirementsCategoryItem;
    }

    public GuideProceduresAndRequirementsCategoryItem getItemGuideProceduresAndRequirementsCategory(int position){
        return Objects.requireNonNull(listGuideProceduresAndRequirementsCategory.getValue()).get(position);
    }

    public GuideProceduresAndRequirementsCategoryItem getGuideProceduresAndRequirementsCategoryItem() {
        return guideProceduresAndRequirementsCategoryItem;
    }

    public MutableLiveData<Drawable> getIconState() {
        return iconState;
    }

    @Override
    protected void handleResponse(Object responseService) {
        GuideProceduresAndRequirementsCategoryResponse result = (GuideProceduresAndRequirementsCategoryResponse) responseService;
        validateResponse(result);
    }

    public void validateResponse(GuideProceduresAndRequirementsCategoryResponse result) {
        if (result != null && result.getTransactionState() && validateIfListOfCategoriesIsNullOrEmpty(result.getGuideProceduresAndRequirementsCategoryItems())) {
            result.getGuideProceduresAndRequirementsCategoryItems().add(getItemSoonMoreCategory());
            listGuideProceduresAndRequirementsCategory.setValue(result.getGuideProceduresAndRequirementsCategoryItems());
        }
    }

    public boolean validateIfListOfCategoriesIsNullOrEmpty(List<GuideProceduresAndRequirementsCategoryItem> listCategories) {
        if (listCategories != null){
            return !listCategories.isEmpty();
        }
        return false;
    }

    public GuideProceduresAndRequirementsCategoryItem getItemSoonMoreCategory() {
        return new GuideProceduresAndRequirementsCategoryItem(
                Constants.NAME_ICON_SOON_MORE_CATEGORY,
                Constants.TEXT_SOON_TOU_WILL_SEE_MORE_INFORMATION_ABOUT_PROCEDURES,
                false
        );
    }
}
