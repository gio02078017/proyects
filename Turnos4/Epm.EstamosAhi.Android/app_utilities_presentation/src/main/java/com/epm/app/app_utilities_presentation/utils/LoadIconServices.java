package com.epm.app.app_utilities_presentation.utils;

import android.support.v7.app.AppCompatActivity;

import app.epm.com.utilities.R;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by leidycarolinazuluagabastidas on 10/05/17.
 */

public class LoadIconServices extends AppCompatActivity {

    public int setIdIconoServicio(String name) {
        int image = 0;
        switch (name) {
            case Constants.AGUA:
                image = R.mipmap.icon_service_water_green;
                break;
            case Constants.ENERGIAS:
                image = R.mipmap.icon_service_energy_green;
                break;
            case Constants.GAS:
                image = R.mipmap.icon_service_gas_green;
                break;
            default:
                break;
        }
        return image;
    }
}