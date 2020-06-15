package app.epm.com.contacto_transparente_presentation.contacto_transparente;

import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.junit.rules.ExpectedException;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.runners.MockitoJUnitRunner;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.contacto_transparente_domain.business_models.GrupoInteres;
import app.epm.com.contacto_transparente_domain.business_models.Incidente;
import app.epm.com.contacto_transparente_domain.business_models.ItemUI;
import app.epm.com.contacto_transparente_domain.contacto_transparente.ContactoTransparenteBusinessLogic;
import app.epm.com.contacto_transparente_domain.contacto_transparente.IContactoTransparenteRepository;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by leidycarolinazuluagabastidas on 14/03/17.
 */

@RunWith(MockitoJUnitRunner.class)
public class ContactoTransparenteBusinessLogicTest {

    ContactoTransparenteBusinessLogic contactoTransparenteBusinessLogic;

    @Rule
    public ExpectedException expectedException = ExpectedException.none();

    @Mock
    IContactoTransparenteRepository contactoTransparenteRepository;

    @Before
    public void setUp(){
        contactoTransparenteBusinessLogic = new ContactoTransparenteBusinessLogic(contactoTransparenteRepository);
    }


    private Incidente getIncident(){
        Incidente incident = new Incidente();
        incident.setCorreoElectronicoContacto("sdsd@gmail.com");
        incident.setNombreContacto("Carolina");
        incident.setTelefonoContacto("2344556");
        incident.setDescripcion("prueba");
        incident.setPersonasInvolucradas("varios");
        incident.setPersonasInvolucradasEnLaEmpresa("epm");
        incident.setFechaDeteccion("12 marzo 2017");
        incident.setIdGrupoInteres("334343-3434-3434");
        incident.setLugarEnDondeSucedio("medellin");
        return incident;
    }

    @Test
    public void methoWithCorrectParametersShouldMethodGetListInterestGroupInRepository() throws RepositoryError {
        contactoTransparenteBusinessLogic.getGruposDeInteres();
        verify(contactoTransparenteRepository).getGruposDeInteres();
    }

    @Test
    public void methodGetListInterestGroupShouldReturnAnListGrupoInteresWhenCallGetListInterestGroupInRepository() throws RepositoryError {
        List<GrupoInteres> grupoInteresList = new ArrayList<>();
        when(contactoTransparenteRepository.getGruposDeInteres()).thenReturn(grupoInteresList);
        List<GrupoInteres> grupoInteresListBL = contactoTransparenteBusinessLogic.getGruposDeInteres();
        Assert.assertEquals(grupoInteresList,grupoInteresListBL);
    }

    @Test
    public void methodGetIncidenteShouldReturnEntityIncidenteFromRepository() throws RepositoryError {
        Incidente incidente = new Incidente();
        String codigoIncidente = "123";
        when(contactoTransparenteRepository.getIncidente(codigoIncidente)).thenReturn(incidente);
        Incidente incidenteBL = contactoTransparenteBusinessLogic.getIncidente(codigoIncidente);
        Assert.assertEquals(incidente,incidenteBL);
    }

    @Test
    public void methodSendIncidentShouldReturnAnItemUIWhenCallSendIncidentInRepository() throws RepositoryError {
        Incidente incident =  getIncident();
        ItemUI itemUI = new ItemUI();
        when(contactoTransparenteRepository.sendIncident(incident)).thenReturn(itemUI);
        ItemUI item = contactoTransparenteBusinessLogic.sendIncident(incident);
        Assert.assertEquals(itemUI, item);
    }

    @Test
    public void methodSendIncidentWithDescripcionnNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        Incidente incident =  getIncident();
        incident.setDescripcion(null);
        contactoTransparenteBusinessLogic.sendIncident(incident);

    }@Test
    public void methodSendIncidentWithDescripcionnEmptyShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_STRING);
        Incidente incident =  getIncident();
        incident.setDescripcion(Constants.EMPTY_STRING);
        contactoTransparenteBusinessLogic.sendIncident(incident);
    }

    @Test
    public void methodSendIncidentWithLugarNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        Incidente incident =  getIncident();
        incident.setLugarEnDondeSucedio(null);
        contactoTransparenteBusinessLogic.sendIncident(incident);

    }@Test
    public void methodSendIncidentWithLugarEmptyShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_STRING);
        Incidente incident =  getIncident();
        incident.setLugarEnDondeSucedio(Constants.EMPTY_STRING);
        contactoTransparenteBusinessLogic.sendIncident(incident);
    }

    @Test
    public void methodSendIncidentWithFechaNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        Incidente incident =  getIncident();
        incident.setFechaDeteccion(null);
        contactoTransparenteBusinessLogic.sendIncident(incident);
    }

    @Test
    public void methodSendIncidentWithFechaEmptyShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_STRING);
        Incidente incident =  getIncident();
        incident.setFechaDeteccion(Constants.EMPTY_STRING);
        contactoTransparenteBusinessLogic.sendIncident(incident);
    }

}
