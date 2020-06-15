package com.epm.app.di.module.procedure;

import androidx.lifecycle.ViewModel;

import com.epm.app.di.util.ViewModelKey;
import com.epm.app.mvvm.procedure.viewModel.FrequentProceduresViewModel;
import com.epm.app.mvvm.procedure.viewModel.GuideProceduresAndRequirementsViewModel;
import com.epm.app.mvvm.procedure.viewModel.TypePersonViewModel;

import dagger.Binds;
import dagger.Module;
import dagger.multibindings.IntoMap;

@Module
public abstract class ViewModelModuleProcedure {


    @Binds
    @IntoMap
    @ViewModelKey(GuideProceduresAndRequirementsViewModel.class)
    abstract ViewModel bindGuideProceduresAndRequirementsViewModel(GuideProceduresAndRequirementsViewModel guideProceduresAndRequirementsViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(FrequentProceduresViewModel.class)
    abstract ViewModel bindFrequentProceduresViewModel(FrequentProceduresViewModel frequentProceduresViewModel);

    @Binds
    @IntoMap
    @ViewModelKey(TypePersonViewModel.class)
    abstract ViewModel bindTypePersonViewModel(TypePersonViewModel typePersonViewModel);

}
