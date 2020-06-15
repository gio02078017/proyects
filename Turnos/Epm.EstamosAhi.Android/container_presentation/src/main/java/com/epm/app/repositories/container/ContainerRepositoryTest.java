package com.epm.app.repositories.container;

import com.epm.app.business_models.business_models.ItemGeneral;
import com.epm.app.business_models.business_models.ListasGenerales;
import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.Usuario;

import java.util.ArrayList;

import app.epm.com.container_domain.business_models.Authoken;
import app.epm.com.container_domain.business_models.EmailUsuarioRequest;
import app.epm.com.container_domain.business_models.InformacionEspacioPromocional;
import app.epm.com.container_domain.container.IContainerRepository;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public class ContainerRepositoryTest implements IContainerRepository {

    @Override
    public ListasGenerales getGeneralList() throws RepositoryError {
        ListasGenerales listasGenerales = new ListasGenerales();
        ArrayList<ItemGeneral> itemGenerals = new ArrayList<>();
        ItemGeneral itemGeneral = new ItemGeneral();
        itemGeneral.setCodigo("test");
        itemGeneral.setDescripcion("test");
        itemGeneral.setId(1);
        itemGenerals.add(itemGeneral);
        listasGenerales.setGeneros(itemGenerals);
        listasGenerales.setTiposIdentificacion(itemGenerals);
        listasGenerales.setTiposPersonas(itemGenerals);
        listasGenerales.setTiposViviendas(itemGenerals);
        return listasGenerales;
    }

    @Override
    public Authoken getGuestLogin() throws RepositoryError {
        Authoken authoken = new Authoken();
        Usuario usuario = new Usuario();
        usuario.setNombres("test");
        usuario.setApellido("tests tests");
        usuario.setActivo(true);
        usuario.setToken("sfgsfgsfg46465");
        authoken.setUsuario(usuario);
        authoken.setInvitado(false);
        return authoken;
    }

    @Override
    public Authoken getAutoLogin(String token) throws RepositoryError {
        Authoken authoken = new Authoken();
        Usuario usuario = new Usuario();
        usuario.setNombres("test");
        usuario.setApellido("tests tests");
        usuario.setActivo(true);
        usuario.setToken("sfgsfgsfg46465");
        authoken.setUsuario(usuario);
        authoken.setInvitado(false);
        return authoken;
    }

    @Override
    public InformacionEspacioPromocional getEspacioPromocional() throws RepositoryError {
        return null;
    }
}