package app.epm.com.utilities.helpers;


import com.epm.app.business_models.business_models.ItemGeneral;

import java.util.List;

/**
 * Created by JoseTabares on 13/05/16.
 */
public interface ICustomSharedPreferences {

    String getString(String key);

    Integer getInt(String key);

    void addString(String key, String value);

    void addBoolean(String key, boolean value);

    boolean getBoolean(String key);

    void addInt(String key, Integer value);

    void deleteValue(String key);

    void addSetArray(String key, List<ItemGeneral> value);

    List<ItemGeneral> getItemGeneralList(String key);

}
