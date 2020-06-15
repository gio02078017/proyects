package app.epm.com.security_presentation.dependency_injection;

import app.epm.com.security_domain.profile.ProfileBL;
import app.epm.com.security_domain.security.SecurityBusinessLogic;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public class  DomainModule {
    public static SecurityBusinessLogic getSecurityBLInstance(ICustomSharedPreferences customSharedPreferences) {
        return new SecurityBusinessLogic(RepositoryLocator.getSecurityRepositoryInstance(customSharedPreferences));
    }

    public static ProfileBL getProfileBLInstance(ICustomSharedPreferences customSharedPreferences) {
        return new ProfileBL(RepositoryLocator.getProfileRepositoryInstance(customSharedPreferences));
    }
}
