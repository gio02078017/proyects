package com.epm.app.di.module.turn;

import androidx.lifecycle.ViewModel;

import com.epm.app.di.util.ViewModelKey;
import com.epm.app.mvvm.procedure.viewModel.DetailsOfTheTransactionViewModel;
import com.epm.app.mvvm.turn.viewModel.AskTurnViewModel;
import com.epm.app.mvvm.turn.viewModel.ChannelsOfAttentionViewModel;
import com.epm.app.mvvm.turn.viewModel.DashboardCustomerServiceViewModel;
import com.epm.app.mvvm.turn.viewModel.NearbyOfficesViewModel;
import com.epm.app.mvvm.turn.viewModel.OfficeDetailViewModel;
import com.epm.app.mvvm.turn.viewModel.OficinasDeAtencionViewModel;
import com.epm.app.mvvm.turn.viewModel.ShiftInformationViewModel;

import dagger.Binds;
import dagger.Module;
import dagger.multibindings.IntoMap;

@Module
public abstract class ViewModelModuleTurn {

    @Binds
    @IntoMap
    @ViewModelKey(DashboardCustomerServiceViewModel.class)
    abstract ViewModel bindCustomerServiceMenuOptionViewModel(DashboardCustomerServiceViewModel dashboardCustomerServiceViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(ChannelsOfAttentionViewModel.class)
    abstract ViewModel bindChannelsOfAttentionViewModel(ChannelsOfAttentionViewModel customerChannelsOfAttentionViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(NearbyOfficesViewModel.class)
    abstract ViewModel bindNearbyOfficesViewModel(NearbyOfficesViewModel customerNearbyOfficesViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(OfficeDetailViewModel.class)
    abstract ViewModel bindOfficeDetailViewModel(OfficeDetailViewModel customerOfficeDetailViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(AskTurnViewModel.class)
    abstract ViewModel bindAskTurnViewModel(AskTurnViewModel customerAskTurnViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(ShiftInformationViewModel.class)
    abstract ViewModel bindShiftInformationViewModel(ShiftInformationViewModel customerShiftInformationViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(OficinasDeAtencionViewModel.class)
    abstract ViewModel bindOficinasDeAtencionViewModel(OficinasDeAtencionViewModel oficinasDeAtencionViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(DetailsOfTheTransactionViewModel.class)
    abstract ViewModel bindDetailsOfTheTransactionViewModel(DetailsOfTheTransactionViewModel detailsOfTheTransactionViewModel);


}
