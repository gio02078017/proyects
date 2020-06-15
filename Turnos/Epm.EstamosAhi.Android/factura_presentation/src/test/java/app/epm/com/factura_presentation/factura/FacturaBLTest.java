package app.epm.com.factura_presentation.factura;

import com.epm.app.business_models.business_models.Mensaje;
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

import app.epm.com.facturadomain.business_models.ComprobantePago;
import app.epm.com.facturadomain.business_models.DataContratos;
import app.epm.com.facturadomain.business_models.DataPagar;
import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.facturadomain.business_models.FacturasPorTransaccion;
import app.epm.com.facturadomain.business_models.InformacionPSE;
import app.epm.com.facturadomain.business_models.GestionContrato;
import app.epm.com.facturadomain.business_models.ProcesarInformacionPSE;
import app.epm.com.facturadomain.business_models.ServicioFacturaResponse;
import app.epm.com.facturadomain.business_models.TransaccionPSEResponse;
import app.epm.com.facturadomain.factura.FacturaBL;
import app.epm.com.facturadomain.factura.IFacturaRepository;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

/**
 * Created by ocadavid on 19/12/2016.
 */

@RunWith(MockitoJUnitRunner.class)
public class FacturaBLTest {

    @Rule
    public ExpectedException expectedException = ExpectedException.none();

    FacturaBL facturaBL;

    @Mock
    IFacturaRepository facturaRepository;

    @Before
    public void setUp() {
        facturaBL = new FacturaBL(facturaRepository);
    }

    /**
     * Start Consultar factura por referente de pago.
     * @throws
     */
    @Test
    public void methodConsulFacturaPorReferenteDePagoWithNumberNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        facturaBL.consulFacturaPorReferenteDePago(null);
    }

    @Test
    public void methodWithCorrectParametersShouldCallMethodConsulFacturaPorReferenteDePagoInRepository() throws RepositoryError {
        String number = "123";
        facturaBL.consulFacturaPorReferenteDePago(number);
        verify(facturaRepository).consultFacturaPorReferenteDePago(number);
    }

    /**
     * Start Consultar factura por contrato.
     * @throws
     */
    @Test
    public void methodConsulFacturaPorContratoWithNumberNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        facturaBL.consulFacturaPorContrato(null);
    }

    @Test
    public void methodWithCorrectParametersShouldCallMethodConsulFacturaPorContratoInRepository() throws RepositoryError {
        String number = "123";
        facturaBL.consulFacturaPorContrato(number);
        verify(facturaRepository).consultFacturaPorContrato(number);
    }

    @Test
    public void methodConsulDetalleFacturasWithNumberFacturaNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        String numberContrato = null;
        facturaBL.consulDetalleFacturas(numberContrato);
        verify(facturaRepository).consultDetalleFacturas(numberContrato);
    }

    /**
     * Start Inscribir contrato.
     * @return inscribirContrato.
     */
    private GestionContrato getInscribirContrato() {
        GestionContrato inscribirContrato = new GestionContrato();
        inscribirContrato.setCorreoElectronico("test@test.com");
        List<DataContratos> itemsContratos = new ArrayList<>();
        DataContratos dataContratos = new DataContratos();
        dataContratos.setDescripcion("test");
        dataContratos.setNumero("0123");
        dataContratos.setRecibirFacturaDigital(true);
        dataContratos.setOperacion(1);
        itemsContratos.add(dataContratos);
        inscribirContrato.setContratos(itemsContratos);
        return inscribirContrato;
    }

    @Test
    public void methodInscribirContratoWithResponseNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        facturaBL.inscribirContrato(null);
    }

    @Test
    public void methodInscribirContratoWithEmailNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        GestionContrato inscribirContrato = getInscribirContrato();
        inscribirContrato.setCorreoElectronico(null);
        facturaBL.inscribirContrato(inscribirContrato);
    }

    @Test
    public void methodInscribirContratoWithContratosNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        GestionContrato inscribirContrato = getInscribirContrato();
        inscribirContrato.setContratos(null);
        facturaBL.inscribirContrato(inscribirContrato);
    }

    @Test
    public void methodInscribirContratoWithEmailEmptyShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_STRING);
        GestionContrato inscribirContrato = getInscribirContrato();
        inscribirContrato.setCorreoElectronico(Constants.EMPTY_STRING);
        facturaBL.inscribirContrato(inscribirContrato);
    }

    @Test
    public void methodInscribirContratoShouldReturnAnMessageWhenCallInscribirContratoInRepository() throws RepositoryError {
        GestionContrato inscribirContrato = getInscribirContrato();
        Mensaje mensaje = new Mensaje();
        when(facturaRepository.inscribirContrato(inscribirContrato)).thenReturn(mensaje);
        Mensaje result = facturaBL.inscribirContrato(inscribirContrato);
        Assert.assertEquals(mensaje, result);
    }

    /**
     * Start Pagar.
     * @return dataPagar.
     */
    private DataPagar getDataPagar() {
        DataPagar dataPagar = new DataPagar();
        FacturaResponse facturaResponse = new FacturaResponse();
        List<FacturaResponse> itemsFactura = new ArrayList<>();
        facturaResponse.setDescripcionContrato("test");
        facturaResponse.setId(1);
        facturaResponse.setDocumentoReferencia("test");
        facturaResponse.setFechaCreacion("test");
        facturaResponse.setFechaRecargo("test");
        facturaResponse.setFechaVencimiento("test");
        facturaResponse.setNumeroContrato("123");
        facturaResponse.setNumeroFactura("123");
        facturaResponse.setValorFactura(1);
        facturaResponse.setUrl("test");
        facturaResponse.setEstadoPagoFactura(true);
        facturaResponse.setFacturaVencida(false);
        facturaResponse.setEstaSeleccionadaParaPago(true);
        facturaResponse.setEstaPendiente(false);
        facturaResponse.setCode(1);
        facturaResponse.setText("test");
        itemsFactura.add(facturaResponse);
        dataPagar.setEntidadFinanciera(1);
        dataPagar.setIdTipoDocumento(1);
        dataPagar.setIdTipoPersona(1);
        dataPagar.setNumeroDocumento("123");
        dataPagar.setFacturasPagar(itemsFactura);
        return dataPagar;
    }

    @Test
    public void methodDatosPagarWithResponseNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        facturaBL.datosPagar(null);
    }

    @Test
    public void methodDatosPagarWithEntidadFinancieraNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        DataPagar dataPagar = getDataPagar();
        dataPagar.setEntidadFinanciera(null);
        facturaBL.datosPagar(dataPagar);
    }

    @Test
    public void methodDatosPagarWithTipoDocumentoNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        DataPagar dataPagar = getDataPagar();
        dataPagar.setIdTipoDocumento(null);
        facturaBL.datosPagar(dataPagar);
    }

    @Test
    public void methodDatosPagarWithTipoPersonaNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        DataPagar dataPagar = getDataPagar();
        dataPagar.setIdTipoPersona(null);
        facturaBL.datosPagar(dataPagar);
    }

    @Test
    public void methodDatosPagarWithNumeroDocumentoNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        DataPagar dataPagar = getDataPagar();
        dataPagar.setNumeroDocumento(null);
        facturaBL.datosPagar(dataPagar);
    }

    @Test
    public void methodDatosPagarWithFacturasPagarNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        DataPagar dataPagar = getDataPagar();
        dataPagar.setFacturasPagar(null);
        facturaBL.datosPagar(dataPagar);
    }

    @Test
    public void methodDatosPagarShouldReturnAnMessageWhenCallDatosPagarInRepository() throws RepositoryError {
        DataPagar dataPagar = getDataPagar();
        InformacionPSE informacionPSE = new InformacionPSE();
        when(facturaRepository.datosPagar(dataPagar)).thenReturn(informacionPSE);
        InformacionPSE result = facturaBL.datosPagar(dataPagar);
        Assert.assertEquals(informacionPSE, result);
    }

    /**
     * Start comprobante de pago.
     * @return comprobantePago.
     */
    private ComprobantePago getComprobantePago() {
        ComprobantePago comprobantePago = new ComprobantePago();
        List<FacturasPorTransaccion> itemsFacturasTransaccion = new ArrayList<>();
        comprobantePago.setCorreos("test");
        comprobantePago.setValorTotalPago(1);
        comprobantePago.setIdTransaccion(1);
        comprobantePago.setCodigoTrazabilidad("test");
        comprobantePago.setEstadoTransaccion(1);
        comprobantePago.setDireccionIp("test");
        comprobantePago.setNombreEntidadFinanciera("test");
        comprobantePago.setFacturasPorTransaccion(itemsFacturasTransaccion);
        return comprobantePago;
    }

    @Test
    public void methodComprobantePagoWithResponseNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        facturaBL.comprobantePago(null);
    }

    @Test
    public void methodComprobantePagoWithCorreosNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        ComprobantePago comprobantePago = getComprobantePago();
        comprobantePago.setCorreos(null);
        facturaBL.comprobantePago(comprobantePago);
    }

    @Test
    public void methodComprobantePagoWithValorTotalPagoNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        ComprobantePago comprobantePago = getComprobantePago();
        comprobantePago.setValorTotalPago(null);
        facturaBL.comprobantePago(comprobantePago);
    }

    @Test
    public void methodComprobantePagoWithIdTransaccionNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        ComprobantePago comprobantePago = getComprobantePago();
        comprobantePago.setIdTransaccion(null);
        facturaBL.comprobantePago(comprobantePago);
    }

    @Test
    public void methodComprobantePagoWithCodigoTrazabilidadNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        ComprobantePago comprobantePago = getComprobantePago();
        comprobantePago.setCodigoTrazabilidad(null);
        facturaBL.comprobantePago(comprobantePago);
    }

    @Test
    public void methodComprobantePagoWithEstadoTransaccionNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        ComprobantePago comprobantePago = getComprobantePago();
        comprobantePago.setEstadoTransaccion(null);
        facturaBL.comprobantePago(comprobantePago);
    }

    @Test
    public void methodComprobantePagoWithNombreEntidadFinancieraNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        ComprobantePago comprobantePago = getComprobantePago();
        comprobantePago.setNombreEntidadFinanciera(null);
        facturaBL.comprobantePago(comprobantePago);
    }

    @Test
    public void methodComprobantePagoWithDireccionIpNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        ComprobantePago comprobantePago = getComprobantePago();
        comprobantePago.setDireccionIp(null);
        facturaBL.comprobantePago(comprobantePago);
    }

    @Test
    public void methodComprobantePagoWithFacturasPorTransaccionNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        ComprobantePago comprobantePago = getComprobantePago();
        comprobantePago.setFacturasPorTransaccion(null);
        facturaBL.comprobantePago(comprobantePago);
    }

    @Test
    public void methodComprobantePagoShouldReturnAnMessageWhenCallComprobantePagoInRepository() throws RepositoryError {
        ComprobantePago comprobantePago = getComprobantePago();
        Mensaje mensaje = new Mensaje();
        when(facturaRepository.comprobantePago(comprobantePago)).thenReturn(mensaje);
        Mensaje result = facturaBL.comprobantePago(comprobantePago);
        Assert.assertEquals(mensaje, result);
    }

    /**
     * Start procesar información PSE.
     * @return
     */
    private ProcesarInformacionPSE getProcesarInformacionPSE(){
        ProcesarInformacionPSE procesarInformacionPSE = new ProcesarInformacionPSE();
        ArrayList<FacturasPorTransaccion> itemsFacturasTransaccion = new ArrayList<>();
        procesarInformacionPSE.setEstadoTransaccion(1);
        procesarInformacionPSE.setCodigoTrazabilidad("test");
        procesarInformacionPSE.setEntidadFinanciera(1);
        procesarInformacionPSE.setIdTransaccion(1);
        procesarInformacionPSE.setFacturasPorTransaccion(itemsFacturasTransaccion);
        return procesarInformacionPSE;
    }

    @Test
    public void methodProcesarInformacionPSEWithResponseNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        facturaBL.procesarInformacionPSE(null);
    }

    @Test
    public void methodProcesarInformacionPSEWithEstadoTransaccionNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        ProcesarInformacionPSE procesarInformacionPSE = getProcesarInformacionPSE();
        procesarInformacionPSE.setEstadoTransaccion(null);
        facturaBL.procesarInformacionPSE(procesarInformacionPSE);
    }

    @Test
    public void methodProcesarInformacionPSEWithCodigoTrazabilidadNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        ProcesarInformacionPSE procesarInformacionPSE = getProcesarInformacionPSE();
        procesarInformacionPSE.setCodigoTrazabilidad(null);
        facturaBL.procesarInformacionPSE(procesarInformacionPSE);
    }

    @Test
    public void methodProcesarInformacionPSEWithEntidadFinancieraNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        ProcesarInformacionPSE procesarInformacionPSE = getProcesarInformacionPSE();
        procesarInformacionPSE.setEntidadFinanciera(null);
        facturaBL.procesarInformacionPSE(procesarInformacionPSE);
    }

    @Test
    public void methodProcesarInformacionPSEWithIdTransaccionNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        ProcesarInformacionPSE procesarInformacionPSE = getProcesarInformacionPSE();
        procesarInformacionPSE.setIdTransaccion(null);
        facturaBL.procesarInformacionPSE(procesarInformacionPSE);
    }

    @Test
    public void methodProcesarInformacionPSEWithFacturasPorTransaccionNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        ProcesarInformacionPSE procesarInformacionPSE = getProcesarInformacionPSE();
        procesarInformacionPSE.setFacturasPorTransaccion(null);
        facturaBL.procesarInformacionPSE(procesarInformacionPSE);
    }

    @Test
    public void methodProcesarInformacionPSEShouldReturnAnMessageWhenCallProcesarInformacionPSEInRepository() throws RepositoryError {
        ProcesarInformacionPSE procesarInformacionPSE = getProcesarInformacionPSE();
        TransaccionPSEResponse transaccionPSEResponse = new TransaccionPSEResponse();
        when(facturaRepository.procesarInformacionPSE(procesarInformacionPSE)).thenReturn(transaccionPSEResponse);
        TransaccionPSEResponse result = facturaBL.procesarInformacionPSE(procesarInformacionPSE);
        Assert.assertEquals(transaccionPSEResponse, result);
    }

    /**
     * Start updateContratos
     */

    @Test
    public void methodUpdateContratosWithInfoContratosNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        facturaBL.updateContratos(null);
    }

    @Test
    public void methodUpdateContratosWithEmailNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        GestionContrato infoContratos = getInscribirContrato();
        infoContratos.setCorreoElectronico(null);
        facturaBL.updateContratos(infoContratos);
    }

    @Test
    public void methodUpdateContratosWithContratosNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        GestionContrato infoContratos = getInscribirContrato();
        infoContratos.setContratos(null);
        facturaBL.updateContratos(infoContratos);
    }

    @Test
    public void methodUpdateContratosWithEmailEmptyShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_STRING);
        GestionContrato infoContratos = getInscribirContrato();
        infoContratos.setCorreoElectronico(Constants.EMPTY_STRING);
        facturaBL.updateContratos(infoContratos);
    }

    @Test
    public void methodUpdateContratosShouldReturnAnMessageWhenCallInscribirContratoInRepository() throws RepositoryError {
        GestionContrato infoContratos = getInscribirContrato();
        Mensaje mensaje = new Mensaje();
        when(facturaRepository.updateContratos(infoContratos)).thenReturn(mensaje);
        Mensaje result = facturaBL.updateContratos(infoContratos);
        Assert.assertEquals(mensaje, result);
    }

    /**
     * End updateContratos
     */

    /**
     * Start consulContratosInscritos
     */

    @Test
    public void methodConsulContratosInscritosWithEmailNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        facturaBL.consulContratosInscritos(null);
    }

    @Test
    public void methodConsulContratosInscritosShouldReturnAnMessageWhenCallConsulContratosInscritosInRepository() throws RepositoryError {
        List<DataContratos> dataContratos = new ArrayList<>();
        when(facturaRepository.consultContratosInscritos("email@correo.com")).thenReturn(dataContratos);
        List<DataContratos>  result = facturaBL.consulContratosInscritos("email@correo.com");
        Assert.assertEquals(dataContratos, result);
    }

    /**
     * End consulContratosInscritos
     */

    /**
     * Start Detalle factura
     */

    @Test
    public void methodDetailFacturaWithEmptyNumberFacturaShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.EMPTY_PARAMETERS);
        String numberFactura = "";
        facturaBL.getDetailFactura(numberFactura);
    }

    @Test
    public void methodDetailFacturaWithNumberFacturaNullShouldReturnAnException() throws RepositoryError {
        expectedException.expect(IllegalArgumentException.class);
        expectedException.expectMessage(Constants.NULL_PARAMETERS);
        facturaBL.getDetailFactura(null);
    }

    @Test
    public void methodDetailFacturaWithCorrectParametersShouldCallMethodDetailFacturaInRepository() throws RepositoryError {
        String numberFactura = "12344";
        facturaBL.getDetailFactura(numberFactura);
        verify(facturaRepository).detailFactura(numberFactura);
    }

    @Test
    public void methodDetailFacturaShouldreturnAnListServicioFacturaResponseWhenCallDetailFacturaInRepository() throws RepositoryError {
        List<ServicioFacturaResponse> servicioFacturaResponses = new ArrayList<>();
        servicioFacturaResponses.add(new ServicioFacturaResponse());
        String numberFactura = "12344";
        when(facturaRepository.detailFactura(numberFactura)).thenReturn(servicioFacturaResponses);
        List<ServicioFacturaResponse> result = facturaBL.getDetailFactura(numberFactura);
        Assert.assertEquals(servicioFacturaResponses,result);
    }

    /**
     * End Detalle factura
     */
}