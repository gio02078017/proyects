package com.epm.app.mvvm.comunidad.repository;

import android.content.Context;

import com.epm.app.mvvm.comunidad.bussinesslogic.ISharedBL;
import com.epm.app.mvvm.comunidad.models.ISharedPreferences;
import com.epm.app.mvvm.comunidad.models.SharedPreferencesModel;

import javax.inject.Inject;

public class RepositoryShared implements ISharedBL {


    private ISharedPreferences sharedPreferencesModel;

    @Inject
    public RepositoryShared(SharedPreferencesModel sharedPreferencesModel){
        this.sharedPreferencesModel = sharedPreferencesModel;
    }

    @Override
    public boolean getValidate(Context context) {
        return sharedPreferencesModel.validateVariableSharedpreferencesOfTheTutorial(context);
    }

    @Override
    public void getTutorialsuccessfull(Context context){
        sharedPreferencesModel.createVariableSharedPreferenceOfTutorial(context);
    }



}
