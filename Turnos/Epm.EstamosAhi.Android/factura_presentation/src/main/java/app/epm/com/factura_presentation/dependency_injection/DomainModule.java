package app.epm.com.factura_presentation.dependency_injection;

import app.epm.com.facturadomain.factura.FacturaBL;

import app.epm.com.utilities.helpers.ICustomSharedPreferences;

/**
 * Created by ocadavid on 16/12/2016.
 */
public class DomainModule {

    public static FacturaBL getFacturaBLInstance(ICustomSharedPreferences customSharedPreferences) {
        return new FacturaBL(RepositoryLocator.getFacturaRepositoryInstance(customSharedPreferences));
    }
}
