package app.epm.com.contacto_transparente_presentation.dependency_injection;

import app.epm.com.contacto_transparente_domain.contacto_transparente.IContactoTransparenteRepository;
import app.epm.com.contacto_transparente_presentation.repositories.ContactoTransparenteRepository;
import app.epm.com.contacto_transparente_presentation.repositories.ContactoTransparenteRepositoryTest;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by leidycarolinazuluagabastidas on 9/03/17.
 */

public class RepositoryLocator {

    static IContactoTransparenteRepository getContactoTransparenteRepositoryInstance
            (ICustomSharedPreferences customSharedPreferences) {
        if (Constants.IS_DEBUG) {
            return new ContactoTransparenteRepositoryTest();
        } else {
            return new ContactoTransparenteRepository(customSharedPreferences);
        }
    }
}
