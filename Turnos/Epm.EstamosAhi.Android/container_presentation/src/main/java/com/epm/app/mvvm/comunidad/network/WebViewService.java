package com.epm.app.mvvm.comunidad.network;

import com.epm.app.mvvm.comunidad.network.response.webViews.InformationInterest;
import com.epm.app.mvvm.comunidad.network.response.webViews.PrivacyPolicy;

import io.reactivex.Single;
import retrofit2.Response;
import retrofit2.http.GET;
import retrofit2.http.Header;

public interface WebViewService {

    @GET("MaestrosAlerta/ObtenerInformacionDeInteres")
    Single<Response<InformationInterest>> getUrlInformationInterest(
            @Header("authToken") String token
    );

    @GET("MaestrosAlerta/ObtenerPoliticaDePrivacidad")
    Single<Response<PrivacyPolicy>> getUrlPolicyPrivacy(
            @Header("authToken") String token
    );

}
