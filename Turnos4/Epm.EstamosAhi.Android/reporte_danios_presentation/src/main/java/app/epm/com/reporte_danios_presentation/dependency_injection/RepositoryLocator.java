package app.epm.com.reporte_danios_presentation.dependency_injection;

import app.epm.com.reporte_danios_domain.danios.danios.IDaniosRepository;
import app.epm.com.reporte_danios_presentation.repositories.DaniosRepository;
import app.epm.com.reporte_danios_presentation.repositories.DaniosRepositoryTest;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by leidycarolinazuluagabastidas on 2/02/17.
 */

public class RepositoryLocator {

    static IDaniosRepository getDaniosRepositoryInstance(ICustomSharedPreferences customSharedPreferences) {

        if (Constants.IS_DEBUG) {
            return new DaniosRepositoryTest();
        } else {
            return new DaniosRepository(customSharedPreferences);
        }
    }

}
