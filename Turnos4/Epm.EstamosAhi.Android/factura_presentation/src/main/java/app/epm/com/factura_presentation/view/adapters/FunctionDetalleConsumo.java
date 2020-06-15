package app.epm.com.factura_presentation.view.adapters;

import android.app.Activity;
import android.content.res.Configuration;

import java.util.List;

import app.epm.com.facturadomain.business_models.DetalleServicioFactura;
import app.epm.com.facturadomain.business_models.ServicioFacturaResponse;
import app.epm.com.factura_presentation.R;
import app.epm.com.utilities.helpers.FormatDateFactura;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by ocadavid on 14/02/2017.
 */

public class FunctionDetalleConsumo {

    private Activity activity;
    private FormatDateFactura formatDateFactura;
    private float densidad;
    private int tamanioMayorDeLaBarra;
    private boolean isTablet;


    public FunctionDetalleConsumo(Activity activity) {
        this.activity= activity;
        this.formatDateFactura = new FormatDateFactura();
        this.densidad = activity.getApplicationContext().getResources().getDisplayMetrics().density;
        this.isTablet = (activity.getResources().getConfiguration().screenLayout & Configuration.SCREENLAYOUT_SIZE_MASK) >= Configuration.SCREENLAYOUT_SIZE_LARGE;
        this.tamanioMayorDeLaBarra = isTablet ? 300 : 180;
    }

    /**
     * Obtiene el subtitulo de la gráfica.
     * @param unidadDeMedida
     * @param size
     * @return
     */
    public String getSubtituloGrafica(String unidadDeMedida, int size) {
        String subtituloDeLaGrafica;

        if (size == 1) {
            subtituloDeLaGrafica = activity.getResources().getString(R.string.factura_lbl_subtitulo_grafica_solo_un_mes);
            subtituloDeLaGrafica = subtituloDeLaGrafica + " (" + unidadDeMedida + ")";
        } else {
            String primerParteDelSubtitulo = activity.getResources().getString(R.string.factura_lbl_subtitulo_grafica_parte_1);
            String segundaParteDelSubtitulo = activity.getResources().getString(R.string.factura_lbl_subtitulo_grafica_parte_2);
            subtituloDeLaGrafica = primerParteDelSubtitulo + " " + Integer.toString(size) + " " + segundaParteDelSubtitulo + " (" + unidadDeMedida + ")";
        }

        return subtituloDeLaGrafica;
    }

    /**
     * Obtiene el título de la gráfica.
     * @param servicio
     * @return
     */
    public String getTituloGrafica(String servicio) {
        String tituloDeLaGrafica = activity.getResources().getString(R.string.factura_lbl_titulo_grafica);
        servicio = formatDateFactura.primeroLetraMayuscula(servicio);
        tituloDeLaGrafica = tituloDeLaGrafica + " " + servicio;
        return tituloDeLaGrafica;
    }

    /**
     * Obtiene el indice del menor consumo.
     * @param indiceDelMayorConsumo
     * @param indiceDelConsumoIntermedio
     * @param detalleServicioFacturaList
     * @return
     */
    public int getIndiceMenorConsumo(int indiceDelMayorConsumo, int indiceDelConsumoIntermedio, List<DetalleServicioFactura> detalleServicioFacturaList) {
        int indice = -1;
        double menorConsumo = -1;

        int numeroDeItemsEnLaLista = detalleServicioFacturaList.size();

        for (int i = 0; i < numeroDeItemsEnLaLista; i++) {
            if (i != indiceDelMayorConsumo && i != indiceDelConsumoIntermedio && detalleServicioFacturaList.get(i).getValor() > menorConsumo) {
                menorConsumo = detalleServicioFacturaList.get(i).getValor();
                indice = i;
            }
        }

        return indice;
    }

    /**
     * Obtiene el indice del consumo intermedio.
     * @param indiceDelMayorConsumo
     * @param detalleServicioFacturaList
     * @return
     */
    public int getIndiceConsumoIntermedio(int indiceDelMayorConsumo, List<DetalleServicioFactura> detalleServicioFacturaList) {
        int indice = -1;
        double consumoIntermedio = -1;

        int numeroDeItemsEnLaLista = detalleServicioFacturaList.size();

        for (int i = 0; i < numeroDeItemsEnLaLista; i++) {
            if (i != indiceDelMayorConsumo && detalleServicioFacturaList.get(i).getValor() > consumoIntermedio) {
                consumoIntermedio = detalleServicioFacturaList.get(i).getValor();
                indice = i;
            }
        }

        return indice;
    }

    /**
     * Obtiene el indice de mayor consumo.
     * @param detalleServicioFacturaList
     * @return
     */
    public int getIndiceMayorConsumo(List<DetalleServicioFactura> detalleServicioFacturaList) {
        double mayorConsumo = -1;
        int indice = -1;
        for (int i = 0; i < detalleServicioFacturaList.size(); i++) {
            if (detalleServicioFacturaList.get(i).getValor() > mayorConsumo) {
                mayorConsumo = detalleServicioFacturaList.get(i).getValor();
                indice = i;
            }
        }

        return indice;
    }

    /**
     * Obtiene el detalle del servicio con el tamaño de la barra y el color de menor consumo.
     * @param detalleServicioFacturaList
     * @param indiceDelMayorConsumo
     * @param indiceDelMenorConsumo
     * @return
     */
    public List<DetalleServicioFactura> getDetalleDeServicioConAnchoYColorDelMenorConsumo(List<DetalleServicioFactura> detalleServicioFacturaList, int indiceDelMayorConsumo, int indiceDelMenorConsumo) {

        DetalleServicioFactura detalleMenorConsumo = detalleServicioFacturaList.get(indiceDelMenorConsumo);

        detalleMenorConsumo.setColorDeLaBarra(R.color.factura_barra_gris_menor_consumo);

        int mayorConsumo = detalleServicioFacturaList.get(indiceDelMayorConsumo).getValor();

        int menorConsumo = detalleMenorConsumo.getValor();

        double porcentajeDelMenorConsumoRespectoAlMayorConsumo = obtenerPorcentaje(mayorConsumo, menorConsumo);

        detalleMenorConsumo.setTamanioDeLaBarra(obtenerAncho(porcentajeDelMenorConsumoRespectoAlMayorConsumo * this.tamanioMayorDeLaBarra));

        detalleServicioFacturaList.set(indiceDelMenorConsumo, detalleMenorConsumo);

        return detalleServicioFacturaList;
    }

    /**
     * Obtiene el detalle del servicio con el tamaño de la barra y el color del consumo intermedio.
     * @param detalleServicioFacturaList
     * @param indiceDelMayorConsumo
     * @param indiceDelConsumoIntermedio
     * @return
     */
    public List<DetalleServicioFactura> getDetalleDeServicioConAnchoYColorDelConsumoIntermedio(List<DetalleServicioFactura> detalleServicioFacturaList, int indiceDelMayorConsumo, int indiceDelConsumoIntermedio) {

        DetalleServicioFactura detalleConsumoIntermedio = detalleServicioFacturaList.get(indiceDelConsumoIntermedio);

        detalleConsumoIntermedio.setColorDeLaBarra(R.color.factura_barra_verde_consumo_medio);

        int mayorConsumo = detalleServicioFacturaList.get(indiceDelMayorConsumo).getValor();

        int consumoIntermdio = detalleConsumoIntermedio.getValor();

        double porcentajeDelConsumoIntermedioRespectoAlMayorConsumo = obtenerPorcentaje(mayorConsumo, consumoIntermdio);

        detalleConsumoIntermedio.setTamanioDeLaBarra(obtenerAncho(porcentajeDelConsumoIntermedioRespectoAlMayorConsumo * this.tamanioMayorDeLaBarra));

        detalleServicioFacturaList.set(indiceDelConsumoIntermedio, detalleConsumoIntermedio);

        return detalleServicioFacturaList;
    }

    /**
     * Obtiene el detalle del servicio con el tamaño de la barra y el color del mayor consumo.
     * @param detalleServicioFacturaList
     * @param indiceDelMayorConsumo
     * @return
     */
    public List<DetalleServicioFactura> getDetalleDeServicioConAnchoYColorDelMayor(List<DetalleServicioFactura> detalleServicioFacturaList, int indiceDelMayorConsumo) {

        DetalleServicioFactura detalleMayor = detalleServicioFacturaList.get(indiceDelMayorConsumo);

        detalleMayor.setColorDeLaBarra(R.color.factura_barra_verde_mayor_consumo);
        detalleMayor.setTamanioDeLaBarra(obtenerAncho(this.tamanioMayorDeLaBarra));

        detalleServicioFacturaList.set(indiceDelMayorConsumo, detalleMayor);

        return detalleServicioFacturaList;
    }

    /**
     * Obtiene el valor a pagar de un tipo de servicio en el formato moneda.
     * @param servicioFacturaResponse
     * @return
     */
    public String getValorAPagar(ServicioFacturaResponse servicioFacturaResponse) {
        Double valorAPagarDecimal = servicioFacturaResponse.getValorAPagar();
        return formatDateFactura.moneda(valorAPagarDecimal.intValue());
    }

    /**
     * Obtiene el identificador del icono correspondiente a un tipo de servicio.
     * @param tituloSelServicio
     * @return
     */
    public int getIconoTipoServicio(String tituloSelServicio) {
        switch (tituloSelServicio) {
            case Constants.DETALLE_FACTURA_ACUEDUCTO:
                return R.mipmap.icon_detalle_acueducto;

            case Constants.DETALLE_FACTURA_ALCANTARILLADO:
                return R.mipmap.icon_detalle_alcantarillado;

            case Constants.DETALLE_FACTURA_ENERGIA:
                return R.mipmap.icon_detalle_energia;

            case Constants.DETALLE_FACTURA_GAS:
                return R.mipmap.icon_detalle_gas;

            case Constants.DETALLE_FACTURA_OTROS:
                return R.mipmap.icon_detalle_otros;

            default:
                return 0;
        }
    }

    /**
     * Calcula el porcentaje correspondiente.
     * @param dividendo
     * @param divisor
     * @return
     */
    private double obtenerPorcentaje(double dividendo, double divisor) {

        if (dividendo != 0) {
            return divisor / dividendo;
        }

        return 0;
    }

    /**
     * Calcula el ancho de la barra.
     * @param ancho
     * @return
     */
    private int obtenerAncho(double ancho) {
        return (int) (ancho * densidad + 0.5f);
    }
}
