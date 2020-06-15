package com.epm.app.menu_presentation.dependecy_injection;

import com.epm.app.menu_presentation.repositories.menu_fragment.MenuFragmentRepository;
import com.epm.app.menu_presentation.repositories.menu_fragment.MenuFragmentRepositoryTest;

import app.epm.com.security_domain.profile.IProfileRepository;
import app.epm.com.security_domain.security.ISecurityRepository;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by juan on 11/04/17.
 */

public class RepositoryLocator {
    static ISecurityRepository getSecurityRepositoryInstance(ICustomSharedPreferences customSharedPreferences) {
        if (Constants.IS_DEBUG) {
            return new MenuFragmentRepositoryTest();
        } else {
            return new MenuFragmentRepository(customSharedPreferences);
        }
    }
}
