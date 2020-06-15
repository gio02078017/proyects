package com.epm.app.mvvm.util;

import android.content.pm.PackageManager;

import com.epm.app.mvvm.comunidad.network.response.places.Municipio;
import com.epm.app.mvvm.comunidad.network.response.places.Sectores;

import java.util.List;

public class Utils {
    public static boolean isPackageInstalled(String packagename, PackageManager packageManager) {
        try {
            packageManager.getPackageGids(packagename);
            return true;
        } catch (PackageManager.NameNotFoundException e) {
            return false;
        }
    }

    public static String[] getMunicipiosArrayFromItemGeneralList(List<Municipio> tiposList) {
        String[] vector = new String[tiposList.size()];
        for (int i = 0; i < tiposList.size(); i++) {
            vector[i] = tiposList.get(i).getDescripcion();
        }
        return vector;
    }

    public static String[] getSectoresArrayFromItemGeneralList(List<Sectores> tiposList) {
        String[] vector = new String[tiposList.size()];
        for (int i = 0; i < tiposList.size(); i++) {
            vector[i] = tiposList.get(i).getSector();
        }
        return vector;
    }
}
