package com.epm.app.mvvm.comunidad.views.activities;

import androidx.lifecycle.ViewModelProviders;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import androidx.databinding.DataBindingUtil;
import android.net.Uri;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentPagerAdapter;
import androidx.viewpager.widget.ViewPager;
import android.os.Bundle;
import com.epm.app.R;
import com.epm.app.databinding.ActivityTutorialBinding;
import com.epm.app.mvvm.comunidad.network.response.places.Municipio;
import com.epm.app.mvvm.comunidad.viewModel.TutorialViewModel;
import com.epm.app.mvvm.comunidad.views.fragments.Tutorial1;
import com.epm.app.mvvm.comunidad.views.fragments.Tutorial2;
import java.util.ArrayList;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;
import me.relex.circleindicator.CircleIndicator;

public class TutorialActivity extends BaseActivityWithDI implements Tutorial1.OnFragmentInteractionListener, Tutorial2.OnFragmentInteractionListener  {


    private SectionsPagerAdapter mSectionsPagerAdapter;

    TutorialViewModel tutorialViewModel;
    private ViewPager mViewPager;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        ActivityTutorialBinding binding = DataBindingUtil.setContentView(this,R.layout.activity_tutorial);
        configureDagger();
        tutorialViewModel = ViewModelProviders.of(this,viewModelFactory).get(TutorialViewModel.class);
        binding.setTutorialViewModel((TutorialViewModel) tutorialViewModel);
        mSectionsPagerAdapter = new SectionsPagerAdapter(getSupportFragmentManager());
        mViewPager = (ViewPager) findViewById(R.id.container);
        mViewPager.setAdapter(mSectionsPagerAdapter);
        CircleIndicator indicator = (CircleIndicator) findViewById(R.id.indicator);
        indicator.setViewPager(mViewPager);
        tutorialViewModel.checkedTutorial(getApplicationContext());
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_PORTRAIT);
        goToTheNextActivity();
    }


    @Override
    public void onFragmentInteraction(Uri uri) {

    }


    public static class PlaceholderFragment extends Fragment {
        private static final String ARG_SECTION_NUMBER = "section_number";

        public PlaceholderFragment() {
        }
        public static Fragment newInstance(int sectionNumber) {
            Fragment fragment = null;
            switch (sectionNumber){
                case 1:
                    fragment= new Tutorial1();
                    break;
                case 2:
                    fragment=new Tutorial2();
                    break;
            }
            Bundle args = new Bundle();
            args.putInt(ARG_SECTION_NUMBER, sectionNumber);

            return fragment;
        }

    }


    public class SectionsPagerAdapter extends FragmentPagerAdapter {

        public SectionsPagerAdapter(FragmentManager fm) {
            super(fm);
        }
        @Override
        public Fragment getItem(int position) {
            return PlaceholderFragment.newInstance(position + 1);
        }
        @Override
        public int getCount() {
            return 2;
        }

        @Override
        public CharSequence getPageTitle(int position){
            switch (position){
                case 0:
                    return "tutorial1";

                case 2:
                    return "tutorial2";

            }
            return null;
        }

    }

    private void gotoAlertasActivity(){
        if(getIntent().getStringExtra(Constants.SUSCRIPTION_ALERTAS).equals(Constants.TRUE)){
            goToDashboardItuangoAlerts();
        }else {
            goToSubscriptionItuangoAlerts();
        }
    }

    private void goToTheNextActivity(){
        tutorialViewModel.getChecked().observe(this, s -> {
            gotoAlertasActivity();
        });
    }

    private void goToSubscriptionItuangoAlerts(){
        Intent intent=new Intent(getApplicationContext(), AlertasActivity.class);
        ArrayList<Municipio> list = getIntent().getParcelableArrayListExtra(Constants.MUNICIPALITIES);
        intent.putParcelableArrayListExtra(Constants.MUNICIPALITIES,list);
        startActivityWithOutDoubleClick(intent);
    }

    private void goToDashboardItuangoAlerts(){
        Intent intent=new Intent(TutorialActivity.this, DashboardComunityAlertActivity.class);
        startActivityWithOutDoubleClick(intent);
    }


    @Override
    public void onBackPressed() {

    }
}
