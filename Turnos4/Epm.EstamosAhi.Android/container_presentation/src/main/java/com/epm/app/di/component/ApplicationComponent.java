package com.epm.app.di.component;


import com.epm.app.App;
import com.epm.app.di.module.community.ActivityModuleCommunity;
import com.epm.app.di.module.ApplicationModule;
import com.epm.app.di.module.FragmentModule;
import com.epm.app.di.module.ServiceBuilderModule;
import com.epm.app.di.module.community.RepositoryModuleCommunity;
import com.epm.app.di.module.procedure.ActivityModuleProcedure;
import com.epm.app.di.module.procedure.RepositoryModuleProcedure;
import com.epm.app.di.module.turn.ActivityModuleTurn;
import com.epm.app.di.module.turn.RepositoryModuleTurn;

import javax.inject.Singleton;

import dagger.Component;
import dagger.android.AndroidInjector;
import dagger.android.support.AndroidSupportInjectionModule;




@Singleton
@Component(modules = {
        AndroidSupportInjectionModule.class,
        ActivityModuleCommunity.class,
        ActivityModuleTurn.class,
        ActivityModuleProcedure.class,
        FragmentModule.class,
        ApplicationModule.class,
        ServiceBuilderModule.class,
        RepositoryModuleCommunity.class,
        RepositoryModuleTurn.class,
        RepositoryModuleProcedure.class
})
public interface ApplicationComponent extends AndroidInjector<App> {


    @Component.Builder
    abstract class Builder extends AndroidInjector.Builder<App> {
    }

    void inject(App app);

}




