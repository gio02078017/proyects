package com.epm.app.services;

import com.epm.app.dto.LineaDeAtencionDTO;

import java.util.List;


import retrofit.http.GET;
import retrofit.http.Header;

/**
 * Created by root on 29/03/17.
 */

public interface ILineasDeAtencionServices {

    @GET("/PuntosGeograficos/ConsultarLineasDeAtencion")
    List<LineaDeAtencionDTO> consultLineasDeAtencion();
}
