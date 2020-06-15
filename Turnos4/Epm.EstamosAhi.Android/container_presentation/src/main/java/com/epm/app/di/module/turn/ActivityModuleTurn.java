package com.epm.app.di.module.turn;

import com.epm.app.mvvm.turn.views.activities.ChannelsOfAttentionActivity;
import com.epm.app.mvvm.turn.views.activities.CustomerServiceMenuOptionActivity;
import com.epm.app.mvvm.turn.views.activities.DetailsOfTheTransactionActivity;
import com.epm.app.mvvm.procedure.views.activities.GuideProceduresAndRequirementsActivity;
import com.epm.app.mvvm.turn.views.activities.NearbyOfficesActivity;
import com.epm.app.mvvm.turn.views.activities.OfficeDetailActivity;
import com.epm.app.mvvm.turn.views.activities.ShiftInformationActivity;
import com.epm.app.view.activities.BaseOficinasDeAtencionOEstacionesDeGasActivity;
import com.epm.app.view.activities.EstacionesDeGasActivity;
import com.epm.app.view.activities.OficinasDeAtencionActivity;

import dagger.Module;
import dagger.android.ContributesAndroidInjector;

@Module
public abstract class ActivityModuleTurn {

    @ContributesAndroidInjector
    abstract CustomerServiceMenuOptionActivity contributeCustomerServiceMenuOptionActivity();

    @ContributesAndroidInjector
    abstract ChannelsOfAttentionActivity contributeChannelsOfAttentionActivity();

    @ContributesAndroidInjector
    abstract NearbyOfficesActivity contributeNearbyOfficesActivity();

    @ContributesAndroidInjector
    abstract OfficeDetailActivity contributeOfficeDetailActivity();

    @ContributesAndroidInjector
    abstract ShiftInformationActivity contributeShiftInformationActivity();

    @ContributesAndroidInjector
    abstract EstacionesDeGasActivity contributeEstacionesDeGasActivity();

    @ContributesAndroidInjector
    abstract OficinasDeAtencionActivity contributeOficinasDeAtencionActivity();

    @ContributesAndroidInjector
    abstract BaseOficinasDeAtencionOEstacionesDeGasActivity contributeBaseOficinasDeAtencionOEstacionesDeGasActivity();

    @ContributesAndroidInjector
    abstract DetailsOfTheTransactionActivity contributeDetailsOfTheTransactionActivity();

}
