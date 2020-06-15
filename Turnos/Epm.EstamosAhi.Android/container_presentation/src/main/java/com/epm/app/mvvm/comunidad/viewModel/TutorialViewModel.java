package com.epm.app.mvvm.comunidad.viewModel;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;
import android.content.Context;

import com.epm.app.mvvm.comunidad.bussinesslogic.ISharedBL;
import com.epm.app.mvvm.comunidad.repository.RepositoryShared;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.ITutorialViewModel;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;

public class TutorialViewModel extends BaseViewModel implements ITutorialViewModel {

    public final MutableLiveData<String> checked;
    ISharedBL repositoryShared;
    private ICustomSharedPreferences customSharedPreferences;

    @Inject
    public TutorialViewModel(RepositoryShared repositoryShared, CustomSharedPreferences customSharedPreferences){
        checked = new MutableLiveData<>();
        this.repositoryShared= repositoryShared;
        this.customSharedPreferences = customSharedPreferences;
    }

    public void saltarIntro(){
        checked.setValue(validateSubscription());
    }

    @Override
    public MutableLiveData<String> getChecked(){
        return checked;
    }


    @Override
    public void checkedTutorial(Context context){
       repositoryShared.getTutorialsuccessfull(context);

    }

    public String validateSubscription(){
        String validate = customSharedPreferences.getString(Constants.SUSCRIPTION_ALERTAS);
        if(validate != null){
            return validate;
        }else{
            return Constants.FALSE;
        }
    }






}
