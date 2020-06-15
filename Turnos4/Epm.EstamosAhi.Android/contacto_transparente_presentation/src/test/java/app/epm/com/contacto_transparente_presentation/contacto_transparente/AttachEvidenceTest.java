package app.epm.com.contacto_transparente_presentation.contacto_transparente;

import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.runners.MockitoJUnitRunner;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

import app.epm.com.contacto_transparente_domain.business_models.Evidencia;
import app.epm.com.contacto_transparente_domain.business_models.Incidente;
import app.epm.com.contacto_transparente_domain.business_models.ItemUI;
import app.epm.com.contacto_transparente_domain.business_models.ParametrosEvidencia;
import app.epm.com.contacto_transparente_domain.business_models.Respuesta;
import app.epm.com.contacto_transparente_domain.contacto_transparente.ContactoTransparenteBusinessLogic;
import app.epm.com.contacto_transparente_domain.contacto_transparente.IContactoTransparenteRepository;
import app.epm.com.contacto_transparente_presentation.R;
import app.epm.com.contacto_transparente_presentation.presenters.AttachEvidencePresenter;
import app.epm.com.contacto_transparente_presentation.view.views_activities.IAttachEvidenceView;
import app.epm.com.utilities.helpers.IFileManager;
import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by leidycarolinazuluagabastidas on 16/03/17.
 */


@RunWith(MockitoJUnitRunner.class)
public class AttachEvidenceTest {

    AttachEvidencePresenter attachEvidencePresenter;
    ContactoTransparenteBusinessLogic contactoTransparenteBusinessLogic;

    @Mock
    IContactoTransparenteRepository contactoTransparenteRepository;

    @Mock
    IAttachEvidenceView attachEvidenceView;

    @Mock
    IValidateInternet validateInternet;

    @Mock
    IFileManager fileManager;


    @Before
    public void setUp() throws Exception {
        contactoTransparenteBusinessLogic = Mockito.spy(new ContactoTransparenteBusinessLogic(contactoTransparenteRepository));
        attachEvidencePresenter = Mockito.spy(new AttachEvidencePresenter(contactoTransparenteBusinessLogic));
        attachEvidencePresenter.inject(attachEvidenceView, validateInternet, fileManager);
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
    public void methodChangeNameWithImageShouldReturnImageNameFormat(){
        String jpg = "jpg";
        ItemUI itemUI = new ItemUI();
        itemUI.setLlave("123");
        itemUI.setValor("23244");
        String timeStamp = new SimpleDateFormat(Constants.FORMAT_DATE_ATTACH).format(new Date());
        String nameImage = Constants.IMAGE.concat(timeStamp).concat(".").concat(jpg);
        ArrayList<String> fields = new ArrayList<>();
        fields.add(new String("/storage/emulated/0/Pictures/JPEG_20170322_113937-901179537.jpg"));
        when(fileManager.getExtensionFile(fields.get(0))).thenReturn(jpg);
        attachEvidencePresenter.changeNameFilesToSend(fields, itemUI);
        String imageNameFormat = attachEvidencePresenter.changeFileName(jpg, timeStamp);
        Assert.assertEquals(nameImage,imageNameFormat);
    }

    @Test
    public void methodChangeNameWithAudioShouldReturnAudioNameFormat(){
        ItemUI itemUI = new ItemUI();
        itemUI.setLlave("23");
        itemUI.setValor("23244");
        String timeStamp = new SimpleDateFormat(Constants.FORMAT_DATE_ATTACH).format(new Date());
        String nameImage = Constants.AUDIO.concat(timeStamp).concat(".").concat(Constants.MP3);
        ArrayList<String> fields = new ArrayList<>();
        fields.add(new String("/storage/emulated/0/AUDIORECORD_20170323_082225.mp3"));
        when(fileManager.getExtensionFile(fields.get(0))).thenReturn(Constants.MP3);
        attachEvidencePresenter.changeNameFilesToSend(fields, itemUI);
        String imageNameFormat = attachEvidencePresenter.changeFileName(Constants.MP3, timeStamp);
        Assert.assertEquals(nameImage,imageNameFormat);
    }

    @Test
    public void methodValidateInternetWithConnectionShouldCallMethodCreateThreadSendIncident(){
        Incidente incident = getIncident();
        when(validateInternet.isConnected()).thenReturn(true);
        attachEvidencePresenter.validateInternetToSendIncident(incident);
        verify(attachEvidencePresenter).createThreadToSendIncident(incident);
        verify(attachEvidenceView, never()).showAlertDialogGeneralInformationOnUiThread(attachEvidenceView.getName(), R.string.text_validate_internet);
    }

    @Test
    public void methodValidateInternetWhitOutConnectionShouldShowAlertDialog(){
        Incidente incident = getIncident();
        when(validateInternet.isConnected()).thenReturn(false);
        attachEvidencePresenter.validateInternetToSendIncident(incident);
        verify(attachEvidenceView).showAlertDialogGeneralInformationOnUiThread(attachEvidenceView.getName(), R.string.text_validate_internet);
        verify(attachEvidencePresenter,never()).createThreadToSendIncident(incident);
    }


    @Test
    public void methodCreateThreadToSendIncidentShouldShowProgressDialog(){
        Incidente incident = getIncident();
        attachEvidencePresenter.createThreadToSendIncident(incident);
        verify(attachEvidenceView).showProgressDIalog(R.string.text_please_wait);
    }

    @Test
    public void methodCreateThreadToSendIncidentShouldCallMethodSendIncident() throws RepositoryError {
        Incidente incident = getIncident();
        ItemUI itemUI = new ItemUI();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(contactoTransparenteRepository.sendIncident(incident)).thenReturn(itemUI);
        attachEvidencePresenter.sendIncident(incident);
        verify(contactoTransparenteBusinessLogic).sendIncident(incident);
        verify(attachEvidenceView).sendAttach(itemUI);
        verify(attachEvidenceView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_error,repositoryError.getMessage());
        verify(attachEvidenceView).dismissProgressDialog();
    }

    @Test
    public void methodsendIncidentshouldShowAnAlertDialogWhenReturnAnException() throws RepositoryError {
        Incidente incident = getIncident();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(0);
        when(contactoTransparenteRepository.sendIncident(incident)).thenThrow(repositoryError);
        attachEvidencePresenter.sendIncident(incident);
        verify(attachEvidenceView).showAlertDialogGeneralInformationOnUiThread(R.string.title_error,repositoryError.getMessage());
        verify(attachEvidenceView, never()).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(attachEvidenceView).dismissProgressDialog();
    }

    @Test
    public void methodSendIncidentShouldShowAnAlertDialogWhenReturnAnUnauthorized() throws RepositoryError {
        Incidente incident = getIncident();
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(401);
        when(contactoTransparenteRepository.sendIncident(incident)).thenThrow(repositoryError);
        attachEvidencePresenter.sendIncident(incident);
        verify(attachEvidenceView).showAlertDialogUnauthorized(R.string.title_error, R.string.text_unauthorized);
        verify(attachEvidenceView, never()).showAlertDialogGeneralInformationOnUiThread(R.string.title_error,repositoryError.getMessage());
        verify(attachEvidenceView).dismissProgressDialog();
    }

    @Test
    public void methodValidateInternetWithConnectionShouldCallMethodCreateThreadSendEvidenceFiles(){
        String message = "dsd";
        ItemUI itemUI = new ItemUI();
        ArrayList<Evidencia> evidenceList = new ArrayList<>();
        evidenceList.add(new Evidencia());
        ParametrosEvidencia evidenceParameter = new ParametrosEvidencia();
        evidenceParameter.setCodigoDelActoIndebido("12344");
        evidenceParameter.setEvidenciasDelActoIndebido(evidenceList);
        when(validateInternet.isConnected()).thenReturn(true);
        attachEvidencePresenter.validateInternetToSendEvidenceFiles(evidenceParameter,itemUI);
        verify(attachEvidencePresenter).createThreadToSendEvidenceFiles(evidenceParameter, itemUI);
    }

    @Test
    public void methodCreateThreadToSendEvidenceFilesShouldCallMethodSendEvidenceFiles() throws RepositoryError {
        String message = "dsd";
        ItemUI itemUI = new ItemUI();
        Respuesta respuesta = new Respuesta();
        respuesta.setEstado(true);
        ArrayList<Evidencia> evidenceList = new ArrayList<>();
        evidenceList.add(new Evidencia());
        ParametrosEvidencia evidenceParameter = new ParametrosEvidencia();
        evidenceParameter.setCodigoDelActoIndebido("12344");
        evidenceParameter.setEvidenciasDelActoIndebido(evidenceList);
        when(contactoTransparenteRepository.sendEvidenceFiles(evidenceParameter)).thenReturn(respuesta);
        attachEvidencePresenter.sendEvidenceFiles(evidenceParameter,itemUI);
        verify(contactoTransparenteBusinessLogic).sendEvidenceFiles(evidenceParameter);
        verify(attachEvidenceView).showAlertDialogToGoHome(R.string.title_register_successful, itemUI);
        verify(attachEvidenceView).dismissProgressDialog();
    }
}