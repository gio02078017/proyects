package com.epm.app.di.module;


import com.epm.app.mvvm.comunidad.views.fragments.ConfigurationFragment;

import dagger.Module;
import dagger.android.ContributesAndroidInjector;

/**
 * Created by Philippe on 02/03/2018.
 */

@Module
public abstract class FragmentModule {

    @ContributesAndroidInjector
    abstract ConfigurationFragment contributeConfigurationFragment();
}
