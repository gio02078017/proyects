package com.epm.app.services;

import com.epm.app.dto.NoticiasEventosDTO;

import java.util.List;

import retrofit.http.GET;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */

public interface IEventosServices {

    @GET("/Novedades/ConsultarEventos")
    List<NoticiasEventosDTO> Eventos();
}
