package com.epm.app.menu_presentation.dependecy_injection;

import app.epm.com.security_domain.profile.ProfileBL;
import app.epm.com.security_domain.security.SecurityBusinessLogic;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;

/**
 * Created by juan on 11/04/17.
 */

public class DomainModule {

    public static SecurityBusinessLogic getSecurityBLInstance(ICustomSharedPreferences customSharedPreferences) {
        return new SecurityBusinessLogic(RepositoryLocator.getSecurityRepositoryInstance(customSharedPreferences));
    }

}
