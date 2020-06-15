package app.epm.com.security_presentation.dependency_injection;

import app.epm.com.security_domain.profile.IProfileRepository;
import app.epm.com.security_domain.security.ISecurityRepository;
import app.epm.com.security_presentation.repositories.profile.ProfileRepository;
import app.epm.com.security_presentation.repositories.profile.ProfileRepositoryTest;
import app.epm.com.security_presentation.repositories.security.SecurityRepository;
import app.epm.com.security_presentation.repositories.security.SecurityRepositoryTest;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

class RepositoryLocator {

    static ISecurityRepository getSecurityRepositoryInstance(ICustomSharedPreferences customSharedPreferences) {
        if (Constants.IS_DEBUG) {
            return new SecurityRepositoryTest();
        } else {
            return new SecurityRepository(customSharedPreferences);
        }
    }

    static IProfileRepository getProfileRepositoryInstance(ICustomSharedPreferences customSharedPreferences){
        if (Constants.IS_DEBUG) {
            return new ProfileRepositoryTest();
        } else {
            return new ProfileRepository(customSharedPreferences);
        }
    }
}
