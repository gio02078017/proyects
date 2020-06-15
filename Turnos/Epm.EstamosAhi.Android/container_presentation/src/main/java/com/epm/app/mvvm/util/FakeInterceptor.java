package com.epm.app.mvvm.util;



import android.os.SystemClock;

import com.epm.app.BuildConfig;

import java.io.IOException;
import java.net.URI;
import java.util.concurrent.TimeUnit;

import okhttp3.Interceptor;
import okhttp3.MediaType;
import okhttp3.Protocol;
import okhttp3.Response;
import okhttp3.ResponseBody;

public class FakeInterceptor implements Interceptor {

    // FAKE RESPONSES.
    private final static String RESPONSE_FAST_TRANSACTION = "{\n" +
            "\n" +
            "    \"EstadoTransaccion\": true,\n" +
            "\n" +
            "    \"Tramites\": [{\n" +
            "\n" +
            "            \"Id\": \"TRAMF998\",\n" +
            "\n" +
            "            \"Nombre\": \"Duplicado de Factura\",\n" +
            "\n" +
            "            \"Descripcion\": \"Copia de tu factura habitual\",\n" +
            "\n" +
            "            \"Activo\": true,\n" +
            "\n" +
            "            \"ListaServicios\": [\n" +
            "\n" +
            "                \"SERV999\"\n" +
            "\n" +
            "            ]\n" +
            "\n" +
            "        },\n" +
            "\n" +
            "        {\n" +
            "\n" +
            "            \"Id\": \"TRAMF999\",\n" +
            "\n" +
            "            \"Nombre\": \"Saldo de factura\",\n" +
            "\n" +
            "            \"Descripcion\": \"Tiene el valor pendiente por pagar luego de un abono\",\n" +
            "\n" +
            "            \"Activo\": false,\n" +
            "\n" +
            "            \"ListaServicios\": [\n" +
            "\n" +
            "                \"SERV999\"\n" +
            "\n" +
            "            ]\n" +
            "\n" +
            "        }\n" +
            "\n" +
            "    ],\n" +
            "\n" +
            "    \"TransactionServiceMessage\": {\n" +
            "\n" +
            "        \"Identificador\": 1,\n" +
            "\n" +
            "        \"Titulo\": \"\",\n" +
            "\n" +
            "        \"Contenido\": \"Éxito genérico.\"\n" +
            "\n" +
            "    }\n" +
            "\n" +
            "}";


    private final static String RESPONSE_PDF = "{\"id\":1,\"age\":16,\"name\":\"Tovmas Apoyan\"}";

    @Override
    public Response intercept(Chain chain) throws IOException {
        Response response = null;
        if(BuildConfig.DEBUG) {
            String responseString;
            // Get Request URI.
            final URI uri = chain.request().url().uri();
            // Get Query String.
            final String query = uri.getQuery();
            // Parse the Query String.
            final String[] parsedQuery = query.split("=");
            if(parsedQuery[0].equalsIgnoreCase("id") && parsedQuery[1].equalsIgnoreCase("1")) {
                responseString = RESPONSE_FAST_TRANSACTION;
            }
            else if(parsedQuery[0].equalsIgnoreCase("id") && parsedQuery[1].equalsIgnoreCase("2")){
                responseString = RESPONSE_PDF;
            }
            else {
                responseString = "";
            }

            response = new Response.Builder()
                    .code(200)
                    .message(responseString)
                    .request(chain.request())
                    .protocol(Protocol.HTTP_1_0)
                    .body(ResponseBody.create(MediaType.parse("application/json"), responseString.getBytes()))
                    .addHeader("content-type", "application/json")
                    .build();

        }
        else {
            response = chain.proceed(chain.request());
        }
        return response;
    }
}
