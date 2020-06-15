package app.epm.com.factura_presentation.dependency_injection;

import app.epm.com.facturadomain.factura.IFacturaRepository;
import app.epm.com.factura_presentation.repositories.FacturaRepository;
import app.epm.com.factura_presentation.repositories.FacturaRepositoryTest;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by ocadavid on 16/12/2016.
 */
class RepositoryLocator {

    static IFacturaRepository getFacturaRepositoryInstance(ICustomSharedPreferences customSharedPreferences) {

        if (Constants.IS_DEBUG) {
            return new FacturaRepositoryTest();
        } else {
            return new FacturaRepository(customSharedPreferences);
        }
    }
}
