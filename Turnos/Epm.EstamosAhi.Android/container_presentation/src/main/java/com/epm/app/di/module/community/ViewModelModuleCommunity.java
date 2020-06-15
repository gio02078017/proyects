package com.epm.app.di.module.community;

import androidx.lifecycle.ViewModel;
import androidx.lifecycle.ViewModelProvider;
import com.epm.app.di.util.ViewModelKey;
import com.epm.app.mvvm.comunidad.viewModel.AlertasViewModel;
import com.epm.app.mvvm.comunidad.viewModel.ConfigurationNotificationViewModel;
import com.epm.app.mvvm.comunidad.viewModel.FactoryViewModel;
import com.epm.app.mvvm.comunidad.viewModel.InformationViewModel;
import com.epm.app.mvvm.comunidad.viewModel.ItemsRecyclerViewModel;
import com.epm.app.mvvm.comunidad.viewModel.NotificationCenterViewModel;
import com.epm.app.mvvm.comunidad.viewModel.PrivacyPolicyViewModel;
import com.epm.app.mvvm.comunidad.viewModel.RecuperationSubscriptionViewModel;
import com.epm.app.mvvm.comunidad.viewModel.RedAlertViewModel;
import com.epm.app.mvvm.comunidad.viewModel.SplashViewModel;
import com.epm.app.mvvm.comunidad.viewModel.TutorialViewModel;
import com.epm.app.mvvm.comunidad.viewModel.UpdateSubscriptionViewModel;

import dagger.Binds;
import dagger.Module;
import dagger.multibindings.IntoMap;

@Module
public abstract class ViewModelModuleCommunity {

    @Binds
    @IntoMap
    @ViewModelKey(SplashViewModel.class)
    abstract ViewModel bindSplasViewModel(SplashViewModel splashViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(TutorialViewModel.class)
    abstract ViewModel bindTutorialViewModel(TutorialViewModel tutorialViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(AlertasViewModel.class)
    abstract ViewModel bindAlertasViewModel(AlertasViewModel alertasViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(ItemsRecyclerViewModel.class)
    abstract ViewModel bindItemsRecyclerViewModel(ItemsRecyclerViewModel itemsRecyclerViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(InformationViewModel.class)
    abstract ViewModel bindItemRecyclerViewModel(InformationViewModel informationViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(UpdateSubscriptionViewModel.class)
    abstract ViewModel bindUpdateSubscriptionViewModel(UpdateSubscriptionViewModel updateSubscriptionViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(RedAlertViewModel.class)
    abstract ViewModel bindRedAlertViewModel(RedAlertViewModel redAlertViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(PrivacyPolicyViewModel.class)
    abstract ViewModel bindPrivacyPolicyViewModel(PrivacyPolicyViewModel privacyPolicyViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(NotificationCenterViewModel.class)
    abstract ViewModel bindNotificationCenterViewModel(NotificationCenterViewModel notificationCenterViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(ConfigurationNotificationViewModel.class)
    abstract ViewModel bindConfigurationNotificationViewModel(ConfigurationNotificationViewModel configurationNotificationViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(RecuperationSubscriptionViewModel.class)
    abstract ViewModel bindRecuperationSubscriptionViewModel(RecuperationSubscriptionViewModel recuperationSubscriptionViewModel);

    @Binds
    abstract ViewModelProvider.Factory bindViewModelFactory(FactoryViewModel factory);

}
