package com.epm.app.mvvm.turn.viewModel;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;
import android.content.res.Resources;
import android.graphics.drawable.Drawable;
import com.epm.app.mvvm.turn.models.CustomerServiceMenu;
import com.epm.app.mvvm.turn.models.CustomerServiceMenuItem;
import com.epm.app.mvvm.turn.repository.JsonStringRepository;
import app.epm.com.utilities.utils.ConvertUtilities;
import javax.inject.Inject;
import app.epm.com.utilities.utils.Constants;
public class DashboardCustomerServiceViewModel extends ViewModel {

    private MutableLiveData<CustomerServiceMenu>  listCustomerServiceMenu;

    public final MutableLiveData<String>  textItemCustomerServiceDescription;
    public final MutableLiveData<String> btnAccionCustomerService;
    public final MutableLiveData<Drawable> imageCustomerService;
    private Resources resources;
    private CustomerServiceMenuItem customerServiceMenuItem;

    @Inject
    public JsonStringRepository jsonStringRepository;

    @Inject
    public DashboardCustomerServiceViewModel(Resources resources){
        this.textItemCustomerServiceDescription = new MutableLiveData<>();
        this.btnAccionCustomerService = new MutableLiveData<>();
        this.imageCustomerService = new MutableLiveData<>();
        this.listCustomerServiceMenu = new MutableLiveData<>();
        this.resources = resources;
    }

    public void getCustomerServiceMenu(){
        if(jsonStringRepository.getDataMenu() != null && !jsonStringRepository.getDataMenu().getCustomerServiceMenuItem().isEmpty() ) {
            listCustomerServiceMenu.setValue(jsonStringRepository.getDataMenu());
        }
    }

    public CustomerServiceMenuItem getCustomerServiceMenuItem() {
        return customerServiceMenuItem;
    }

    public void setCustomerServiceMenuItem(CustomerServiceMenuItem customerServiceMenuItem) {
        this.customerServiceMenuItem = customerServiceMenuItem;
    }

    public void drawInformation(){
        this.textItemCustomerServiceDescription.setValue(this.customerServiceMenuItem.getTextItemCustomerServiceDescription());
        this.btnAccionCustomerService.setValue(this.customerServiceMenuItem.getTextButtonCustomerService());
        this.imageCustomerService.setValue(resources.getDrawable(ConvertUtilities.resourceId(Constants.TYPE_RESOURCE_DRAWABLE,customerServiceMenuItem.getImageItemCustomerService())));
    }

    public MutableLiveData<CustomerServiceMenu> getListCustomerServiceMenu() {
        return listCustomerServiceMenu;
    }

    public void setListCustomerServiceMenu(MutableLiveData<CustomerServiceMenu> listCustomerServiceMenu) {
        this.listCustomerServiceMenu = listCustomerServiceMenu;
    }
}
