package co.gov.ins.guardianes.presentation.view.welcome;

import android.content.Context;
import android.os.Bundle;

import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentPagerAdapter;

import co.gov.ins.guardianes.helper.Constants;

public class WelcomePagerAdapter extends FragmentPagerAdapter {

    private final Context context;
    private final Welcome[] welcomeArray;

    WelcomePagerAdapter(final FragmentManager fragmentManager, final Context context, final Welcome[] welcomeArray) {
        super(fragmentManager);

        this.context = context;
        this.welcomeArray = welcomeArray;
    }

    @Override
    public int getCount() {
        return welcomeArray.length;
    }

    @Override
    public Fragment getItem(final int position) {

        final Bundle bundle = new Bundle();

        bundle.putSerializable(Constants.Bundle.WELCOME, welcomeArray[position]);

        return Fragment.instantiate(context, WelcomePageFragment.class.getName(), bundle);
    }
}
