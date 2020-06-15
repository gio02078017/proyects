package com.epm.app.menu_presentation.helpers;

import com.epm.app.menu_presentation.dto.EmailUsuarioDTO;

import app.epm.com.security_domain.business_models.EmailUsuarioRequest;
import app.epm.com.utilities.helpers.BaseMapper;

/**
 * Created by juan on 11/04/17.
 */

public class Mapper extends BaseMapper {
    public static EmailUsuarioDTO convertLogOutDTOToDomain(EmailUsuarioRequest emailUsuarioRequest) {
        EmailUsuarioDTO emailUsuarioDTO = new EmailUsuarioDTO();
        emailUsuarioDTO.setCorreoElectronico(emailUsuarioRequest.getCorreoElectronico());
        return emailUsuarioDTO;
    }
}
