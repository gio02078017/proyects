package com.epm.app.dependency_injection;


import app.epm.com.container_domain.eventos.EventosBussinesLogic;
import app.epm.com.container_domain.noticias.NoticiasBussinesLogic;
import app.epm.com.container_domain.lineas_de_atencion.LineasDeAtencionBusinessLogic;
import app.epm.com.container_domain.container.ContainerBusinessLogic;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public class DomainModule {

    public static ContainerBusinessLogic getSecurityBLInstance(ICustomSharedPreferences customSharedPreferences) {
        return new ContainerBusinessLogic(RepositoryLocator.getSecurityRepositoryInstance(customSharedPreferences));
    }

    public static NoticiasBussinesLogic getNoticiasBussinesLogicInstance(ICustomSharedPreferences customSharedPreferences) {
        return new NoticiasBussinesLogic(RepositoryLocator.getNoticiasRepositoryInstance(customSharedPreferences));
    }

    public static EventosBussinesLogic getEventosBussinesLogicInstance(ICustomSharedPreferences customSharedPreferences) {
        return new EventosBussinesLogic(RepositoryLocator.getEventosRepositoryInstance(customSharedPreferences));
    }

    public static LineasDeAtencionBusinessLogic getLineasDeAtencionBusinessLogicInstance(ICustomSharedPreferences customSharedPreferences) {
        return new LineasDeAtencionBusinessLogic(RepositoryLocator.getLineasDeAtencionRepositoryInstance(customSharedPreferences));
    }


}
