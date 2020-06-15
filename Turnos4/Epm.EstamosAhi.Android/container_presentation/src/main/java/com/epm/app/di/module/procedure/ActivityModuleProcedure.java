package com.epm.app.di.module.procedure;

import com.epm.app.mvvm.procedure.views.activities.FrequentProceduresActivity;
import com.epm.app.mvvm.procedure.views.activities.GuideProceduresAndRequirementsActivity;
import com.epm.app.mvvm.procedure.views.activities.TypePersonActivity;

import dagger.Module;
import dagger.android.ContributesAndroidInjector;

@Module
public abstract class ActivityModuleProcedure {

    @ContributesAndroidInjector
    abstract GuideProceduresAndRequirementsActivity contributeGuideProceduresAndRequirementsActivity();

    @ContributesAndroidInjector
    abstract FrequentProceduresActivity contributeFrequentProceduresActivity();

    @ContributesAndroidInjector
    abstract TypePersonActivity contributeTypePersonActivity();

}
