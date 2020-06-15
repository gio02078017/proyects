package app.epm.com.utilities.services;


import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.squareup.okhttp.OkHttpClient;

import java.util.concurrent.TimeUnit;

import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;
import retrofit.RequestInterceptor;
import retrofit.RestAdapter;
import retrofit.client.OkClient;
import retrofit.converter.Converter;
import retrofit.converter.GsonConverter;


/**
 * Created by JoseTabares on 13/05/16.
 */
public class ServicesFactory {

   private static final String API_BASE_PATH = Constants.URL_DLLO_EPM;
   // private static final String API_BASE_PATH = Constants.URL_PRODUCCION_EPM;
    private RestAdapter restAdapter;

    public ServicesFactory(ICustomSharedPreferences customSharedPreferences, String baseURL) {
        createServicesFactoryInstance(customSharedPreferences, getGsonConverter(), baseURL);
    }

    /**
     * Permite configurar la petición http seimpre con el convert gson
     *
     * @param customSharedPreferences
     */
    public ServicesFactory(ICustomSharedPreferences customSharedPreferences) {
        createServicesFactoryInstance(customSharedPreferences, getGsonConverter(), API_BASE_PATH);
    }

    private void createServicesFactoryInstance(ICustomSharedPreferences customSharedPreferences, Converter converter, String baseUrl) {
        RequestInterceptor requestInterceptor = getRequestInterceptor(customSharedPreferences);
        final OkHttpClient okHttpClient = new OkHttpClient();
        okHttpClient.setReadTimeout(Constants.SIXTY, TimeUnit.SECONDS);
        okHttpClient.setConnectTimeout(Constants.SIXTY, TimeUnit.SECONDS);
        restAdapter = new RestAdapter.Builder()
                .setEndpoint(baseUrl)
                .setRequestInterceptor(requestInterceptor)
                .setConverter(converter)
                .setClient(new OkClient(okHttpClient))
                .build();
    }

    private Converter getGsonConverter() {
        Gson gson = new GsonBuilder().setDateFormat("yyyy-MM-dd").create();
        return new GsonConverter(gson);
    }

    private RequestInterceptor getRequestInterceptor(final ICustomSharedPreferences customSharedPreferences) {
        return new RequestInterceptor() {
            @Override
            public void intercept(RequestFacade requestFacade) {
                requestFacade.addHeader("Accept", "application/json");
                String token = customSharedPreferences.getString(Constants.TOKEN);
                if (token != null) {
                    requestFacade.addHeader(Constants.TOKEN, token);
                }
            }
        };
    }

    /**
     * Obtiene la instancia del servicio.
     *
     * @param service Tipo de servicio.
     * @return Instancia del servicio.
     */
    public Object getInstance(Class service) {
        return restAdapter.create(service);
    }
}
