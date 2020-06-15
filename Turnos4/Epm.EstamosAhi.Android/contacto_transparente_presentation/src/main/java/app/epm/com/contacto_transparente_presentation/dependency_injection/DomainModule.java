package app.epm.com.contacto_transparente_presentation.dependency_injection;

import app.epm.com.contacto_transparente_domain.contacto_transparente.ContactoTransparenteBusinessLogic;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;

/**
 * Created by leidycarolinazuluagabastidas on 9/03/17.
 */

public class DomainModule {

    public static ContactoTransparenteBusinessLogic getContactoTransparenteBusinessLogicInstance
            (ICustomSharedPreferences customSharedPreferences) {
        return new ContactoTransparenteBusinessLogic(RepositoryLocator.
                getContactoTransparenteRepositoryInstance(customSharedPreferences));
    }
}
