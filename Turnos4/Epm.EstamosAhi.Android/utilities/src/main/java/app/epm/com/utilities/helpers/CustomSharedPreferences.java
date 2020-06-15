package app.epm.com.utilities.helpers;


import android.content.Context;
import android.content.SharedPreferences;

import com.epm.app.business_models.business_models.ItemGeneral;

import java.util.ArrayList;
import java.util.Collections;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Set;

import app.epm.com.utilities.utils.Constants;

/**
 * Created by JoseTabares on 13/05/16.
 */
public class CustomSharedPreferences implements ICustomSharedPreferences {

    private SharedPreferences sharedPreferences;

    public CustomSharedPreferences(Context context) {
        this.sharedPreferences = context.getSharedPreferences(Constants.MY_PREFERENCES, Context.MODE_PRIVATE);
    }

    @Override
    public String getString(String key) {
        if (sharedPreferences.contains(key)) {
            return sharedPreferences.getString(key, null);
        }
        return null;
    }

    @Override
    public boolean getBoolean(String key) {
        if (sharedPreferences.contains(key)) {
            return sharedPreferences.getBoolean(key, false);
        }
        return false;
    }

    @Override
    public void addBoolean(String key, boolean value) {
        addValue(key, value);
    }

    @Override
    public Integer getInt(String key) {
        if (sharedPreferences.contains(key)) {
            return sharedPreferences.getInt(key, Constants.EMPTY_INT);
        }
        return null;
    }

    @Override
    public void addString(String key, String value) {
        if (value == null) {
            deleteValue(key);
        } else {
            addValue(key, value);
        }
    }

    @Override
    public void addInt(String key, Integer value) {
        if (value == null) {
            deleteValue(key);
        } else {
            addValue(key, value);
        }
    }


    @Override
    public void deleteValue(String key) {
        sharedPreferences.edit().remove(key).apply();
    }

    @Override
    public void addSetArray(String key, List<ItemGeneral> value) {
        if (value == null) {
            deleteValue(key);
        } else {
            Set<String> stringSet = convertGeneralListToSet(value);
            addValue(key, stringSet);
        }
    }

    @Override
    public List<ItemGeneral> getItemGeneralList(String key) {
        Set<String> stringSet = getSetString(key);
        if (stringSet != null) {
            return convertSetStringToItemGeneralList(stringSet);
        }
        return null;
    }

    private List<ItemGeneral> convertSetStringToItemGeneralList(Set<String> stringSet) {
        List<ItemGeneral> itemGeneralList = new ArrayList<>();
        for (String string : stringSet) {
            String[] itemGeneralData = string.split("/.");
            ItemGeneral itemGeneral = new ItemGeneral();
            itemGeneral.setId(Integer.parseInt(itemGeneralData[0]));
            String codigo = itemGeneralData[1];
            itemGeneral.setCodigo(codigo.equals("null") ? null : codigo);
            itemGeneral.setDescripcion(itemGeneralData[2]);
            itemGeneralList.add(itemGeneral);

        }
        Collections.sort(itemGeneralList, (itemGeneral, t1) -> itemGeneral.getDescripcion().compareTo(t1.getDescripcion()));
        return itemGeneralList;
    }

    private Set<String> getSetString(String key) {
        if (sharedPreferences.contains(key)) {
            return sharedPreferences.getStringSet(key, null);
        }
        return null;
    }

    private Set<String> convertGeneralListToSet(List<ItemGeneral> value) {
        Set<String> stringSet = new LinkedHashSet<>();
        for (ItemGeneral itemGeneral : value) {
            String string = String.format(Constants.FORMAT_TO_SAVE_GENERAL_LIST, Integer.toString(itemGeneral.getId()),
                    itemGeneral.getCodigo(), itemGeneral.getDescripcion());
            stringSet.add(string);
        }
        return stringSet;
    }

    private void addValue(String key, String value) {
        sharedPreferences.edit().putString(key, value).apply();
    }

    private void addValue(String key, Integer value) {
        sharedPreferences.edit().putInt(key, value).apply();
    }

    private void addValue(String key, Set<String> value) {
        sharedPreferences.edit().putStringSet(key, value).apply();
    }

    private void addValue(String key, boolean value) {
        sharedPreferences.edit().putBoolean(key, value).apply();
    }
}
