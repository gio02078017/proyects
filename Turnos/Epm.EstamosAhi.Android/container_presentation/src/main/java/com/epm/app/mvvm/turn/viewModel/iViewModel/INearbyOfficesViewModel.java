package com.epm.app.mvvm.turn.viewModel.iViewModel;

import androidx.lifecycle.MutableLiveData;
import android.location.Location;

import com.epm.app.mvvm.turn.network.response.nearbyOffices.NearbyOfficesItem;

import java.util.List;

public interface INearbyOfficesViewModel {

    void getNearbyOffices(Location location);
    void getLocation();
    MutableLiveData<Boolean> getProgressDialog();
    MutableLiveData<Boolean> getSuccessNearbyOffices();
    List<NearbyOfficesItem> getListNearbyOffices();
}
