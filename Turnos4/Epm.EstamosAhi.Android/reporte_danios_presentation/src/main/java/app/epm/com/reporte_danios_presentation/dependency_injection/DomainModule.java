package app.epm.com.reporte_danios_presentation.dependency_injection;

import app.epm.com.reporte_danios_domain.danios.danios.DaniosBL;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;

/**
 * Created by josetabaresramirez on 2/02/17.
 */

public class DomainModule {

    public static DaniosBL getDaniosBLInstance(ICustomSharedPreferences customSharedPreferences){
        return new DaniosBL(RepositoryLocator.getDaniosRepositoryInstance(customSharedPreferences));
    }
}
