package app.epm.com.security_presentation.view.adapters;

import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentStatePagerAdapter;

import app.epm.com.security_presentation.view.fragments.LoginFragment;
import app.epm.com.security_presentation.view.fragments.RegisterFragment;


/**
 * Created by mateoquicenososa on 21/11/16.
 */

public class RegisterLoginTabsAdapter extends FragmentStatePagerAdapter {

    public RegisterLoginTabsAdapter(FragmentManager fragmentManager) {
        super(fragmentManager);
    }

    @Override
    public Fragment getItem(int position) {
        if (position == 0) {
            return new RegisterFragment();
        }
        if (position == 1) {
            return new LoginFragment();
        }
        return null;
    }

    @Override
    public int getCount() {
        return 2;
    }
}
