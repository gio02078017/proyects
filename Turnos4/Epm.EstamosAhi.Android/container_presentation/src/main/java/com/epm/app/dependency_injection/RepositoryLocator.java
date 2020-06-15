package com.epm.app.dependency_injection;


import app.epm.com.container_domain.eventos.IEventosRepository;
import app.epm.com.container_domain.noticias.INoticiasRepository;
import app.epm.com.container_domain.container.IContainerRepository;

import com.epm.app.repositories.eventos.EventosRepository;
import com.epm.app.repositories.eventos.EventosRepositoryTest;
import com.epm.app.repositories.noticias.NoticiasRepository;
import com.epm.app.repositories.noticias.NoticiasRepositoryTest;

import app.epm.com.container_domain.lineas_de_atencion.ILineasDeAtencionRepository;

import com.epm.app.repositories.lineas_de_atencion.LineasDeAtencionRepositoryTest;
import com.epm.app.repositories.lineas_de_atencion.LineasDeAtencionRepository;

import com.epm.app.repositories.container.ContainerRepository;
import com.epm.app.repositories.container.ContainerRepositoryTest;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

class RepositoryLocator {

    static IContainerRepository getSecurityRepositoryInstance(ICustomSharedPreferences customSharedPreferences) {
        if (Constants.IS_DEBUG) {
            return new ContainerRepositoryTest();
        } else {
            return new ContainerRepository(customSharedPreferences);
        }
    }


    static INoticiasRepository getNoticiasRepositoryInstance(ICustomSharedPreferences customSharedPreferences) {
        if (Constants.IS_DEBUG) {
            return new NoticiasRepositoryTest();
        } else {
            return new NoticiasRepository(customSharedPreferences);
        }
    }

    static IEventosRepository getEventosRepositoryInstance(ICustomSharedPreferences customSharedPreferences) {
        if (Constants.IS_DEBUG) {
            return new EventosRepositoryTest();
        } else {
            return new EventosRepository(customSharedPreferences);
        }
    }

    static ILineasDeAtencionRepository getLineasDeAtencionRepositoryInstance(ICustomSharedPreferences customSharedPreferences) {
        if (Constants.IS_DEBUG) {
            return new LineasDeAtencionRepositoryTest();
        } else {
            return new LineasDeAtencionRepository(customSharedPreferences);
        }
    }

}
