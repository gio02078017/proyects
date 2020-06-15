package com.epm.app.mvvm.comunidad.bussinesslogic;


import android.content.Context;

import dagger.Provides;

public interface ISharedBL {

    boolean getValidate(Context context);
    void getTutorialsuccessfull(Context context);


}
