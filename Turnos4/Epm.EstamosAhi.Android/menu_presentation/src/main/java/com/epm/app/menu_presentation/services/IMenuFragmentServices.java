package com.epm.app.menu_presentation.services;

import com.epm.app.business_models.dto.DataDTO;
import com.epm.app.business_models.dto.MensajeDTO;

import retrofit.http.Body;
import retrofit.http.Header;
import retrofit.http.POST;

/**
 * Created by juan on 11/04/17.
 */

public interface IMenuFragmentServices {
    @POST("/AdministracionUsuario/CerrarSesion")
    MensajeDTO logOut(@Body DataDTO dataDTO, @Header("idSuscripcionOneSignal") String idOneSignal);
}
