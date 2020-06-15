package com.epm.app.helpers;

import androidx.annotation.Nullable;

import com.epm.app.business_models.dto.ItemGeneralDTO;
import com.epm.app.business_models.dto.MensajeDTO;
import com.epm.app.business_models.business_models.ItemGeneral;
import com.epm.app.business_models.business_models.ListasGenerales;
import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.Usuario;

import com.epm.app.dto.InformacionEspacioPromocionalDTO;
import com.epm.app.dto.NoticiasEventosDTO;

import com.epm.app.dto.LineaDeAtencionDTO;

import com.google.common.base.Function;
import com.google.common.collect.Lists;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.container_domain.business_models.Authoken;
import app.epm.com.container_domain.business_models.EmailUsuarioRequest;

import com.epm.app.dto.AuthokenDTO;
import com.epm.app.dto.EmailUsuarioDTO;
import com.epm.app.dto.ListasGeneralesDTO;


import app.epm.com.container_domain.business_models.InformacionEspacioPromocional;
import app.epm.com.container_domain.business_models.NoticiasEventos;

import app.epm.com.container_domain.business_models.LineaDeAtencion;

import app.epm.com.security_presentation.dto.UsuarioDTO;
import app.epm.com.utilities.helpers.BaseMapper;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by josetabaresramirez on 18/11/16.
 */

public class Mapper extends BaseMapper {

    public static ListasGenerales convertListasGeneralesDTOToDomain(ListasGeneralesDTO listasGeneralesDTO) {
        final ListasGenerales listasGenerales = new ListasGenerales();
        listasGenerales.setTiposIdentificacion(convertListaItemGeneralDTOToDomain(listasGeneralesDTO.getTiposIdentificacion()));
        listasGenerales.setTiposPersonas(convertListaItemGeneralDTOToDomain(listasGeneralesDTO.getTiposPersonas()));
        listasGenerales.setTiposViviendas(convertListaItemGeneralDTOToDomain(listasGeneralesDTO.getTiposVivienda()));
        listasGenerales.setGeneros(convertListaItemGeneralDTOToDomain(listasGeneralesDTO.getGeneros()));
        return listasGenerales;
    }

    private static List<ItemGeneral> convertListaItemGeneralDTOToDomain(List<ItemGeneralDTO> itemGeneralDTO) {
        return Lists.transform(itemGeneralDTO, new Function<ItemGeneralDTO, ItemGeneral>() {
            @Nullable
            @Override
            public ItemGeneral apply(ItemGeneralDTO itemGeneralDTO) {
                ItemGeneral itemGeneral = new ItemGeneral();
                itemGeneral.setCodigo(itemGeneralDTO.getCodigo());
                itemGeneral.setId(itemGeneralDTO.getId());
                itemGeneral.setDescripcion(itemGeneralDTO.getDescripcion());
                return itemGeneral;
            }
        });
    }

    public static Authoken convertAuthokenDTOToDomain(AuthokenDTO authokenDTO) {
        final Authoken authoken = new Authoken();
        authoken.setMensaje(convertMensajeDTOToDomain(authokenDTO.getMensaje()));
        authoken.setUsuario(convertUsuarioDTOToDomain(authokenDTO.getUsuario()));
        authoken.setInvitado(authokenDTO.isInvitado());
        return authoken;
    }

    public static Mensaje convertMensajeDTOToDomain(MensajeDTO mensajeDTO) {
        final Mensaje mensaje = new Mensaje();

        if (mensajeDTO != null) {
            mensaje.setText(mensajeDTO.getTexto());
            mensaje.setCode(mensajeDTO.getCodigo());
        } else {
            mensaje.setText(Constants.EMPTY_STRING);
            mensaje.setCode(0);
        }
        return mensaje;
    }

    public static Usuario convertUsuarioDTOToDomain(UsuarioDTO usuarioDTO) {
        final Usuario usuario = new Usuario();

        if (usuarioDTO != null) {
            usuario.setApellido(usuarioDTO.getApellidos());
            usuario.setCelular(usuarioDTO.getCelular() != null ? usuarioDTO.getCelular() : "");
            usuario.setCorreoAlternativo(usuarioDTO.getCorreoAlternativo() != null ? usuarioDTO.getCorreoAlternativo() : "");
            usuario.setCorreoElectronico(usuarioDTO.getCorreoElectronico());
            usuario.setDireccion(usuarioDTO.getDireccion() != null ? usuarioDTO.getDireccion() : "");
            usuario.setEnvioNotificacion(usuarioDTO.isEnvioNotificacion());
            usuario.setFechaNacimiento(usuarioDTO.getFechaNacimiento() != null ? usuarioDTO.getFechaNacimiento() : "");
            usuario.setIdGenero(usuarioDTO.getIdGenero());
            usuario.setNombres(usuarioDTO.getNombres());
            usuario.setNumeroIdentificacion(usuarioDTO.getNumeroIdentificacion());
            usuario.setPais(usuarioDTO.getPais() != null ? usuarioDTO.getPais() : "");
            usuario.setTelefono(usuarioDTO.getTelefono() != null ? usuarioDTO.getTelefono() : "");
            usuario.setIdTipoIdentificacion(usuarioDTO.getIdTipoIdentificacion());
            usuario.setIdTipoPersona(usuarioDTO.getIdTipoPersona());
            usuario.setIdTipoVivienda(usuarioDTO.getIdTipoVivienda());
            usuario.setToken(usuarioDTO.getToken());
            usuario.setActivo(usuarioDTO.isActivo());
            usuario.setFechaRegistro(usuarioDTO.getFechaRegistro());
            usuario.setAceptoTerminosyCondiciones(usuarioDTO.isAceptoTerminosyCondiciones());
            usuario.setIdUsuario(usuarioDTO.getIdUsuario());
            usuario.setContrasenia(usuarioDTO.getContrasenia() != null ? usuarioDTO.getContrasenia() : "");
        }
        return usuario;
    }

    public static EmailUsuarioDTO convertLogOutDTOToDomain(EmailUsuarioRequest emailUsuarioRequest) {
        final EmailUsuarioDTO emailUsuarioDTO = new EmailUsuarioDTO();
        emailUsuarioDTO.setCorreoElectronico(emailUsuarioRequest.getCorreoElectronico());
        return emailUsuarioDTO;
    }

    public static List<NoticiasEventos> convertListNoticiasEventosDTOToDomain(List<NoticiasEventosDTO> noticiasEventosDTO) {
        final ArrayList<NoticiasEventos> noticiasEventosList = new ArrayList<>();
        for (NoticiasEventosDTO noticiasEventosDTOs : noticiasEventosDTO) {
            NoticiasEventos noticiasEventos = new NoticiasEventos();
            noticiasEventos.setId(noticiasEventosDTOs.getIdNews());
            noticiasEventos.setDescripcion(noticiasEventosDTOs.getNewsText());
            noticiasEventos.setTitulo(noticiasEventosDTOs.getTitle());
            noticiasEventos.setFecha(noticiasEventosDTOs.getDate());
            noticiasEventos.setResumen(noticiasEventosDTOs.getSummary());
            noticiasEventos.setImagen(noticiasEventosDTOs.getUrlImage());
            noticiasEventosList.add(noticiasEventos);
        }
        return noticiasEventosList;
    }

    public static List<LineaDeAtencion> convertListLineasDeAtencionDTOToDomain(List<LineaDeAtencionDTO> lineaDeAtencionDTOs) {
        final ArrayList<LineaDeAtencion> lineaDeAtencionList = new ArrayList<>();

        for (LineaDeAtencionDTO lineaDeAtencionDTO : lineaDeAtencionDTOs) {
            LineaDeAtencion lineaDeAtencion = new LineaDeAtencion();
            lineaDeAtencion.setName(lineaDeAtencionDTO.getTitle());
            lineaDeAtencion.setNumber(lineaDeAtencionDTO.getNumber());

            lineaDeAtencionList.add(lineaDeAtencion);
        }
        return lineaDeAtencionList;
    }

    public static InformacionEspacioPromocional convertInformacionDTOToDomain(InformacionEspacioPromocionalDTO informacionEspacioPromocionalDTO) {
        InformacionEspacioPromocional informacionEspacioPromocional = new InformacionEspacioPromocional();
        informacionEspacioPromocional.setFechaCreacion(informacionEspacioPromocionalDTO.getFechaCreacion());
        informacionEspacioPromocional.setImagen(informacionEspacioPromocionalDTO.getImagen());
        informacionEspacioPromocional.setModulo(informacionEspacioPromocionalDTO.getModulo());
        return informacionEspacioPromocional;
    }
}