package app.epm.com.security_presentation.services;


import com.epm.app.business_models.dto.DataDTO;
import com.epm.app.business_models.dto.MensajeDTO;

import app.epm.com.security_presentation.dto.ChangePasswordResponseDTO;
import app.epm.com.security_presentation.dto.UsuarioResponseDTO;
import retrofit.http.Body;
import retrofit.http.Header;
import retrofit.http.POST;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public interface ISecurityServices {

    /**
     * Permite autenticarse
     * @param dataDTO informacion encriptada del usuario
     * @param idOneSignal
     * @return UsuarioResponseDTO
     */
    @POST("/AdministracionUsuario/Autenticar")
    UsuarioResponseDTO login(@Body DataDTO dataDTO, @Header("idSuscripcionOneSignal") String idOneSignal);

    @POST("/AdministracionUsuario/Registrar")
    MensajeDTO registerUser(@Body DataDTO dataDTO);

    @POST("/AdministracionUsuario/ResetearContrasenia")
    MensajeDTO resetPassword(@Body DataDTO dataDTO);

    /**
     * Permite actualizar el perfil
     * @param dataDTO informacion encriptada del usuario
     * @return MensajeDTO
     */
    @POST("/AdministracionUsuario/Actualizar")
    MensajeDTO updateProfile(@Body DataDTO dataDTO);

    @POST("/AdministracionUsuario/CambiarContrasenia")
    ChangePasswordResponseDTO changePassword(@Body DataDTO dataDTO);

    @POST("/AdministracionUsuario/CerrarSesion")
    MensajeDTO logOut(@Body DataDTO dataDTO, @Header("idSuscripcionOneSignal") String idOneSignal);

}
