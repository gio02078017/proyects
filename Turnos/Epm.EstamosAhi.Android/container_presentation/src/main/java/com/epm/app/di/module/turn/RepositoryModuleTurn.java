package com.epm.app.di.module.turn;


import android.app.Application;

import com.epm.app.mvvm.turn.network.TurnApiServices;
import com.epm.app.mvvm.turn.repository.JsonStringRepository;
import com.epm.app.mvvm.turn.repository.TurnServicesRepository;
import com.epm.app.services.ServiceWithControlErrorGPS;
import dagger.Module;
import dagger.Provides;

@Module(includes = ViewModelModuleTurn.class)
public class RepositoryModuleTurn {

    @Provides
    public JsonStringRepository providerJsonStringRepository(Application application){
        return new JsonStringRepository(application);
    }

    @Provides
    public TurnServicesRepository providerTurnServicesRepository(TurnApiServices turnApiServices){
        return new TurnServicesRepository(turnApiServices);
    }

    @Provides
    public ServiceWithControlErrorGPS providerServiceWithControlErrorGPS(Application application){
        return new ServiceWithControlErrorGPS(application);
    }
}
