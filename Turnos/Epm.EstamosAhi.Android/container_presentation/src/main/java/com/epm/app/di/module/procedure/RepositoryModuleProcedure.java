package com.epm.app.di.module.procedure;


import com.epm.app.mvvm.procedure.network.ProcedureApiServices;
import com.epm.app.mvvm.procedure.repository.ProcedureServicesRepository;

import dagger.Module;
import dagger.Provides;

@Module(includes = ViewModelModuleProcedure.class)
public class RepositoryModuleProcedure {

    @Provides
    public ProcedureServicesRepository providerProcedureServicesRepository(ProcedureApiServices procedureApiServices){
        return new ProcedureServicesRepository(procedureApiServices);
    }


}
