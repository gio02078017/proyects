package com.epm.app.services;


import com.epm.app.business_models.dto.DataDTO;
import com.epm.app.business_models.dto.MensajeDTO;

import com.epm.app.dto.AuthokenDTO;
import com.epm.app.dto.InformacionEspacioPromocionalDTO;
import com.epm.app.dto.ListasGeneralesDTO;
import retrofit.http.Body;
import retrofit.http.GET;
import retrofit.http.Header;
import retrofit.http.POST;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public interface ISecurityServices {
    /**
     * Permite consultar las listas.
     * @return ListasGeneralesDTO.
     */
    @GET("/Consultas/ConsultarListas")
    ListasGeneralesDTO listGenerals();

    /**
     * Permite realizar un Login Guest.
     * @return AuthokenDTO.
     */
    @GET("/AdministracionUsuario/AutenticarInvitado")
    AuthokenDTO guestLogin();

    /**
     * Permite realizar un AutoLogin.
     * @param token token.
     * @return AuthokenDTO.
     */
    @GET("/AdministracionUsuario/AutoLogin")
    AuthokenDTO autoLogin(@Header("authoken") String token);

    @POST("/AdministracionUsuario/CerrarSesion")
    MensajeDTO logOut(@Body DataDTO dataDTO, @Header("idSuscripcionOneSignal") String idOneSignal);

    @GET("/EspacioPromocional/ObtenerContenidoEspacioPromocional")
    InformacionEspacioPromocionalDTO getInformacionEspacioPromocional();
}