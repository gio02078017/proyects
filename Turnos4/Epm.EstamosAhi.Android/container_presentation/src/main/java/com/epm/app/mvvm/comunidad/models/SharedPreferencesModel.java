package com.epm.app.mvvm.comunidad.models;

import android.content.Context;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;

import app.epm.com.utilities.utils.Constants;

public class SharedPreferencesModel implements ISharedPreferences{

    public boolean validate;


    @Override
    public void createVariableSharedPreferenceOfTutorial(Context context){
        SharedPreferences prefs = PreferenceManager.getDefaultSharedPreferences(context);
        SharedPreferences.Editor editor = prefs.edit();
        editor.putBoolean(Constants.tutorialPreferenceAlerts,true);
        editor.commit();
    }


    @Override
    public boolean validateVariableSharedpreferencesOfTheTutorial(Context context){
        SharedPreferences prefs = PreferenceManager.getDefaultSharedPreferences(context);
        validate = prefs.getBoolean(Constants.tutorialPreferenceAlerts,false);
        return validate;

    }
}
