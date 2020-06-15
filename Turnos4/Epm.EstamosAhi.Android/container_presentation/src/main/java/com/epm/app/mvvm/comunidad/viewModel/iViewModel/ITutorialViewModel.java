package com.epm.app.mvvm.comunidad.viewModel.iViewModel;

import android.arch.lifecycle.MutableLiveData;
import android.content.Context;

public interface ITutorialViewModel {

    void checkedTutorial(Context context);
    MutableLiveData<String> getChecked();

}
