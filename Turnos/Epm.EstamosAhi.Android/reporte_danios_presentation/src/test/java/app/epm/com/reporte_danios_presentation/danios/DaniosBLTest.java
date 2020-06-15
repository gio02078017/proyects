package app.epm.com.reporte_danios_presentation.danios;

import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.epm.app.business_models.business_models.RepositoryError;

import org.junit.Assert;
import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.junit.rules.ExpectedException;
import org.junit.runner.RunWith;
import org.mockito.Mock;
import org.mockito.junit.MockitoJUnitRunner;

import java.util.ArrayList;
import java.util.List;

import com.epm.app.business_models.business_models.InformacionDeUbicacion;
import app.epm.com.reporte_danios_domain.danios.business_models.ParametrosEnviarCorreo;
import app.epm.com.reporte_danios_domain.danios.danios.DaniosBL;
import app.epm.com.reporte_danios_domain.danios.danios.IDaniosRepository;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by josetabaresramirez on 22/02/17.
 */
@RunWith(MockitoJUnitRunner.class)
public class DaniosBLTest {

    @Rule
    public ExpectedException expectedException = ExpectedException.none();

    DaniosBL daniosBL;

    @Mock
    IDaniosRepository daniosRepository;

    @Before
    public void setUp() throws Exception {
        daniosBL = new DaniosBL(daniosRepository);
    }

    @Test
    public void methodGetInformacionDeUbicacionWithParameterLongitudeNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);

        daniosBL.getInformacionDeUbicacion("7.556", null);
    }

    @Test
    public void methodGetInformacionDeUbicacionWithParameterLatitudeNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);

        daniosBL.getInformacionDeUbicacion(null, "-6.3366");
    }

    @Test
    public void methodGetInformacionDeUbicacionWithParameterLatitudeAndLongitudeNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);

        daniosBL.getInformacionDeUbicacion(null, null);
    }

    @Test
    public void methodGetInformacionDeUbicacionWithParameterLatitudeAndLongitudeEmptyShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);

        daniosBL.getInformacionDeUbicacion("", "");
    }

    @Test
    public void methodGetInformacionDeUbicacionWithParameterLatitudeEmptyShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);

        daniosBL.getInformacionDeUbicacion("", "-6.4456");
    }

    @Test
    public void methodGetInformacionDeUbicacionWithParameterLongitudeEmptyShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);

        daniosBL.getInformacionDeUbicacion("7.222", "");
    }

    @Test
    public void methodGetInformacionDeUbicacionWithCorrectParametersShouldCallMethodGetInformacionDeUbicacionWithAddress() throws Exception {

        String lat = "7.55";
        String lon = "-6.33";

        daniosBL.getInformacionDeUbicacion(lat, lon);

        verify(daniosRepository).getInformacionDeUbicacionWithAddress(lat, lon);

    }

    @Test
    public void methodGetInformacionDeUbicacionWithCorrectParametersShouldCallMethodGetBasicInformacionDeUbicacionWhenGetInformacionDeUbicacionWithAddressReturnNull() throws Exception {

        String lat = "7.55";
        String lon = "-6.33";

        when(daniosRepository.getInformacionDeUbicacionWithAddress(lat, lon)).thenReturn(null);

        daniosBL.getInformacionDeUbicacion(lat, lon);

        verify(daniosRepository).getInformacionDeUbicacionWithAddress(lat, lon);
        verify(daniosRepository).getBasicInformacionDeUbicacion(lat, lon);
    }

    @Test
    public void methodGetInformacionDeUbicacionWithCorrectParametersShouldNotCallMethodGetBasicInformacionDeUbicacionWhenGetInformacionDeUbicacionWithAddressReturnAnInstance() throws Exception {

        String lat = "7.55";
        String lon = "-6.33";

        when(daniosRepository.getInformacionDeUbicacionWithAddress(lat, lon)).thenReturn(new InformacionDeUbicacion());

        daniosBL.getInformacionDeUbicacion(lat, lon);

        verify(daniosRepository).getInformacionDeUbicacionWithAddress(lat, lon);
        verify(daniosRepository, never()).getBasicInformacionDeUbicacion(lat, lon);
    }

    @Test
    public void methodGetInformacionDeUbicacionShouldReturnInstanteWithTheAllDataWhenGetInformacionDeUbicacionWithAddressReturnAnInstance() throws RepositoryError {
        String lat = "7.55";
        String lon = "-6.33";
        InformacionDeUbicacion informacionDeUbicacion = new InformacionDeUbicacion();
        informacionDeUbicacion.setDeparatamento("Antioquia");
        informacionDeUbicacion.setMunicipio("Bello");
        informacionDeUbicacion.setDireccion("Calle 32");
        when(daniosRepository.getInformacionDeUbicacionWithAddress(lat, lon)).thenReturn(informacionDeUbicacion);

        InformacionDeUbicacion result = daniosBL.getInformacionDeUbicacion(lat, lon);

        Assert.assertEquals(informacionDeUbicacion, result);
        Assert.assertEquals(informacionDeUbicacion.getDireccion(), result.getDireccion());
        verify(daniosRepository, never()).getBasicInformacionDeUbicacion(lat, lon);
    }

    @Test
    public void methodGetInormacionDeUbicacionShouldReturnIntanceWhenGetInformacionDeUbicacionWithAddressReturnNull() throws Exception {
        String lat = "7.55";
        String lon = "-6.33";
        InformacionDeUbicacion informacionDeUbicacion = new InformacionDeUbicacion();
        informacionDeUbicacion.setDeparatamento("Antioquia");
        informacionDeUbicacion.setMunicipio("Bello");
        informacionDeUbicacion.setDireccion("Bello, Antioquia");
        when(daniosRepository.getInformacionDeUbicacionWithAddress(lat, lon)).thenReturn(null);
        when(daniosRepository.getBasicInformacionDeUbicacion(lat, lon)).thenReturn(informacionDeUbicacion);

        InformacionDeUbicacion result = daniosBL.getInformacionDeUbicacion(lat, lon);

        Assert.assertEquals(informacionDeUbicacion, result);
        Assert.assertEquals(informacionDeUbicacion.getDireccion(), result.getDireccion());
    }

    @Test
    public void methodGetInformacionDeUbicacionShouldReturnNullIfGetInformacionDeUbicacionWithAddressAndGetBasicInformacionDeUbicacionReturnNull() throws Exception {
        String lat = "7.55";
        String lon = "-6.33";
        when(daniosRepository.getInformacionDeUbicacionWithAddress(lat, lon)).thenReturn(null);
        when(daniosRepository.getBasicInformacionDeUbicacion(lat, lon)).thenReturn(null);

        InformacionDeUbicacion result = daniosBL.getInformacionDeUbicacion(lat, lon);

        Assert.assertEquals(null, result);
    }

    @Test
    public void methodGetMunicipiosWithParameterNullShouldReturnAnError() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);

        ParametrosCobertura parametrosCobertura = null;

        daniosBL.getMunicipios(parametrosCobertura);
    }

    @Test
    public void methodGetMunicipiosWithDepartamentoNullShouldReturnAnError() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);

        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        parametrosCobertura.setDepartamento(null);

        daniosBL.getMunicipios(parametrosCobertura);
    }

    @Test
    public void methodGetMunicipiosWithDepartamentoEmptyShouldReturnAnError() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);

        ParametrosCobertura parametrosCobertura = getParametrosCobertura();
        parametrosCobertura.setDepartamento("");

        daniosBL.getMunicipios(parametrosCobertura);
    }

    @Test
    public void methodGetMunicipiosWithCorrectParametersShouldCallGetMunicipiosInRepository() throws RepositoryError {
        ParametrosCobertura parametrosCobertura = getParametrosCobertura();

        daniosBL.getMunicipios(parametrosCobertura);

        verify(daniosRepository).getMunicipios(parametrosCobertura);
    }

    @Test
    public void methodGetMunicipiosWithCorrectParametersShouldReturnMunicipiosFromRepository() throws RepositoryError {
        List<String> municipiosExpected = new ArrayList<>();
        municipiosExpected.add("Bello");

        ParametrosCobertura parametrosCobertura = getParametrosCobertura();

        when(daniosRepository.getMunicipios(parametrosCobertura)).thenReturn(municipiosExpected);

        List<String> municipiosResult = daniosBL.getMunicipios(parametrosCobertura);

        Assert.assertEquals(municipiosExpected, municipiosResult);
    }

    private ParametrosCobertura getParametrosCobertura() {
        ParametrosCobertura parametrosCobertura = new ParametrosCobertura();
        parametrosCobertura.setTipoServicio(1);
        parametrosCobertura.setDepartamento("Antioquia");
        return parametrosCobertura;
    }

    @Test
    public void methodEnviarCorreoWithEntityNullShouldReturnAnError() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);

        ParametrosEnviarCorreo parametrosEnviarCorreo = null;

        daniosBL.sendEmail(parametrosEnviarCorreo);
    }

    @Test
    public void methodEnviarCorreoWithNumeroRadicadoNullShouldReturnAnError() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);

        ParametrosEnviarCorreo parametrosEnviarCorreo = getParametrosEnviarCorreo();
        parametrosEnviarCorreo.setNumeroRadicado(null);

        daniosBL.sendEmail(parametrosEnviarCorreo);
    }

    @Test
    public void methodEnviarCorreoWithNombreServicioNullShouldReturnAnError() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);

        ParametrosEnviarCorreo parametrosEnviarCorreo = getParametrosEnviarCorreo();
        parametrosEnviarCorreo.setNombreServicio(null);

        daniosBL.sendEmail(parametrosEnviarCorreo);
    }

    @Test
    public void methodEnviarCorreoWithNumeroRadicadoEmptyShouldReturnAnError() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);

        ParametrosEnviarCorreo parametrosEnviarCorreo = getParametrosEnviarCorreo();
        parametrosEnviarCorreo.setNumeroRadicado("");

        daniosBL.sendEmail(parametrosEnviarCorreo);
    }

    @Test
    public void methodEnviarCorreoWithNombreServicioEmptyShouldReturnAnError() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);

        ParametrosEnviarCorreo parametrosEnviarCorreo = getParametrosEnviarCorreo();
        parametrosEnviarCorreo.setNombreServicio("");

        daniosBL.sendEmail(parametrosEnviarCorreo);
    }

    @Test
    public void methodEnviarCorreoWithCorrectParametersShouldReturnMensaje() throws RepositoryError {

        Mensaje mensaje = getMensaje();

        ParametrosEnviarCorreo parametrosEnviarCorreo = getParametrosEnviarCorreo();
        when(daniosRepository.sendEmail(parametrosEnviarCorreo)).thenReturn(mensaje);
        Mensaje mensajeResult = daniosBL.sendEmail(parametrosEnviarCorreo);
        Assert.assertEquals(mensajeResult, mensaje);
    }

    private ParametrosEnviarCorreo getParametrosEnviarCorreo() {
        ParametrosEnviarCorreo parametrosEnviarCorreo = new ParametrosEnviarCorreo();
        parametrosEnviarCorreo.setCorreoElectronico("jqpipe@gmail.com");
        parametrosEnviarCorreo.setNombreServicio("AGUA");
        parametrosEnviarCorreo.setNumeroRadicado("123");
        return parametrosEnviarCorreo;
    }

    private Mensaje getMensaje() {
        Mensaje mensaje = new Mensaje();
        mensaje.setCode(1);
        mensaje.setText("Gracias.");
        return mensaje;
    }
}
