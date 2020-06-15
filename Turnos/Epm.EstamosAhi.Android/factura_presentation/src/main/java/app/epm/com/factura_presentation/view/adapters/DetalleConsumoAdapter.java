package app.epm.com.factura_presentation.view.adapters;

import android.app.Activity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;

import java.util.List;

import app.epm.com.facturadomain.business_models.DetalleServicioFactura;
import app.epm.com.facturadomain.business_models.ServicioFacturaResponse;
import app.epm.com.factura_presentation.R;
import app.epm.com.utilities.controls.ListViewExpandible;
import app.epm.com.utilities.helpers.FormatDateFactura;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by ocadavid on 10/02/2017.
 */
public class DetalleConsumoAdapter extends ListViewExpandible.AnimatedExpandableListAdapter {
    private Activity activity;
    ListViewExpandible listViewExpandible;
    private List<ServicioFacturaResponse> servicioFacturaResponseList;
    private LayoutInflater layoutInflater;
    private FormatDateFactura formatDateFactura;
    private FunctionDetalleConsumo function;

    public DetalleConsumoAdapter(Activity activity, ListViewExpandible listViewExpandible, List<ServicioFacturaResponse> servicioFacuraResponseList) {
        this.activity = activity;
        this.listViewExpandible = listViewExpandible;
        this.servicioFacturaResponseList = servicioFacuraResponseList;
        this.layoutInflater = LayoutInflater.from(activity);
        this.formatDateFactura = new FormatDateFactura();
        this.function = new FunctionDetalleConsumo(activity);
    }

    /**
     * Obtiene una vista que muestra los datos para el item dado dentro del grupo dado.
     *
     * @param groupPosition
     * @param childPosition
     * @param isLastChild
     * @param convertView
     * @param parent
     * @return
     */
    @Override
    public View getRealChildView(int groupPosition, int childPosition, boolean isLastChild, View convertView, ViewGroup parent) {
        View customView = convertView;
        if (this.servicioFacturaResponseList.get(groupPosition).getServicio().equals(Constants.DETALLE_FACTURA_OTROS)) {
            return customView = generateViewDetalleConsumoOtros(groupPosition, childPosition, parent);
        } else {
            return customView = generateViewGraficaConsumoTipoServicio(groupPosition, childPosition, parent);
        }
    }

    /**
     * Obtiene el número de items de un grupo especificado (Tipo Servicio).
     *
     * @param groupPosition
     * @return
     */
    @Override
    public int getRealChildrenCount(int groupPosition) {
        if (this.servicioFacturaResponseList.get(groupPosition).getServicio().equals(Constants.DETALLE_FACTURA_OTROS)) {
            return servicioFacturaResponseList.get(groupPosition).getDetalleServicio().size();
        }
        return 1;
    }

    /**
     * Obtiene el número de grupos (Tipos de servicio).
     *
     * @return
     */
    @Override
    public int getGroupCount() {
        return this.servicioFacturaResponseList.size();
    }

    /**
     * Obtiene los datos asociados al grupo seleccionado(Tipo de servicio).
     *
     * @param groupPosition
     * @return
     */
    @Override
    public ServicioFacturaResponse getGroup(int groupPosition) {
        return this.servicioFacturaResponseList.get(groupPosition);
    }

    /**
     * Obtiene los datos asociados al item dado dentro del grupo dado.
     *
     * @param groupPosition
     * @param childPosition
     * @return
     */
    @Override
    public DetalleServicioFactura getChild(int groupPosition, int childPosition) {
        return this.servicioFacturaResponseList.get(groupPosition).getDetalleServicio().get(childPosition);
    }

    /**
     * Obtiene el ID para el grupo en la posición dada.
     *
     * @param groupPosition
     * @return
     */
    @Override
    public long getGroupId(int groupPosition) {
        return groupPosition;
    }

    /**
     * Obtiene el ID para el item dado en el grupo dado.
     *
     * @param groupPosition
     * @param childPosition
     * @return
     */
    @Override
    public long getChildId(int groupPosition, int childPosition) {
        return childPosition;
    }

    /**
     * Indica que los identificadores de los items y del grupo son estables.
     *
     * @return
     */
    @Override
    public boolean hasStableIds() {
        return true;
    }

    /**
     * Obtiene la  vista  del grupo del tipo de servicio seleccionado.
     *
     * @param groupPosition
     * @param isExpanded
     * @param convertView
     * @param parent
     * @return
     */
    @Override
    public View getGroupView(int groupPosition, boolean isExpanded, View convertView, ViewGroup parent) {
        View customView = convertView;
        ServicioFacturaResponse servicioFacturaResponse = getGroup(groupPosition);
        return customView = createViewTipoServicio(groupPosition, parent, servicioFacturaResponse);
    }

    /**
     * Indica que todos los items pertencientes a un grupo son seleccionables.
     *
     * @param groupPosition
     * @param childPosition
     * @return
     */
    @Override
    public boolean isChildSelectable(int groupPosition, int childPosition) {
        return true;
    }

    /**
     * Genera la vista que muestra los datos del grupo Tipo servicio: Otros.
     *
     * @param groupPosition
     * @param childPosition
     * @param parent
     * @return
     */
    private View generateViewDetalleConsumoOtros(int groupPosition, int childPosition, ViewGroup parent) {

        View convertView = createViewDetalleOtros(parent);

        DetalleServicioFactura detalleServicioFactura = getChild(groupPosition, childPosition);

        LinearLayout detalleFacturaOtros_llTitulo = (LinearLayout) convertView.findViewById(R.id.detalleFacturaOtros_llTitulo);

        if (childPosition == 0) {
            detalleFacturaOtros_llTitulo.setVisibility(View.VISIBLE);
        } else {
            detalleFacturaOtros_llTitulo.setVisibility(View.GONE);
        }

        ImageView detalleFacturaOtros_ivBarraSeparadora = (ImageView) convertView.findViewById(R.id.detalleFacturaOtros_ivBarraSeparadora);

        if (childPosition == getRealChildrenCount(groupPosition) - 1) {
            detalleFacturaOtros_ivBarraSeparadora.setVisibility(View.VISIBLE);
        } else {
            detalleFacturaOtros_ivBarraSeparadora.setVisibility(View.GONE);
        }

        showDescripcionOtros(convertView, detalleServicioFactura.getDescripcion());

        showValorOtros(convertView, detalleServicioFactura.getValor());

        return convertView;
    }

    /**
     * Muestra el valor del detalle del tipo de servicio: otros.
     *
     * @param convertView
     * @param valor
     */
    private void showValorOtros(View convertView, double valor) {
        TextView detalleFacturaOtros_tvValor =  convertView.findViewById(R.id.detalleFacturaOtros_tvValor);
        Double valorAFormatear = valor;
        String valorTotal = formatDateFactura.moneda(valorAFormatear.intValue());
        detalleFacturaOtros_tvValor.setText(valorTotal);
    }

    /**
     * Muestra la descripción del detalle del tipo de servicio: otros.
     *
     * @param convertView
     * @param descripcion
     */
    private void showDescripcionOtros(View convertView, String descripcion) {
        TextView detalleFacturaOtros_tvDescripcion =  convertView.findViewById(R.id.detalleFacturaOtros_tvDescripcion);
        descripcion = formatDateFactura.primeroLetraMayuscula(descripcion);
        detalleFacturaOtros_tvDescripcion.setText(descripcion);
    }

    /**
     * Crea la vista del detalla de tipo de servicio: Otros.
     *
     * @param parent
     * @return
     */
    private View createViewDetalleOtros(ViewGroup parent) {
        View view = this.layoutInflater.inflate(R.layout.template_lista_detalle_factura_otros, parent, false);
        return view;
    }

    /**
     * Genera la vista que muestra la gráfica de consumo de cualquier tipo servicios.
     *
     * @param groupPosition
     * @param childPosition
     * @param parent
     * @return
     */

    private View generateViewGraficaConsumoTipoServicio(int groupPosition, int childPosition, ViewGroup parent) {

        View convertView = createViewGrafica(parent);

        TextView grafica_tvConsumo =  convertView.findViewById(R.id.grafica_tvConsumo);
        TextView grafica_tvTitulo =  convertView.findViewById(R.id.grafica_tvTitulo);
        TextView grafica_tvSubtituloIndicador =  convertView.findViewById(R.id.grafica_tvSubtituloIndicador);

        ServicioFacturaResponse servicioFactura = getGroup(groupPosition);

        List<DetalleServicioFactura> detalleServicioFacturaList = servicioFactura.getDetalleServicio();

        int consumo = servicioFactura.getConsumo();
        String unidadDeMedida = servicioFactura.getUnidadDeMedida();

        setConsumoAlTextView(grafica_tvConsumo, consumo, unidadDeMedida);

        String servicio = servicioFactura.getServicio();

        grafica_tvTitulo.setText(this.function.getTituloGrafica(servicio));

        grafica_tvSubtituloIndicador.setText(this.function.getSubtituloGrafica(unidadDeMedida, servicioFactura.getDetalleServicio().size()));

        loadGrafica(convertView, detalleServicioFacturaList, unidadDeMedida);

        return convertView;
    }

    /**
     * Crea la vista de un tipo de servicio.
     *
     * @param groupPosition
     * @param parent
     * @param servicioFacturaResponse
     * @return
     */
    private View createViewTipoServicio(final int groupPosition, ViewGroup parent, ServicioFacturaResponse servicioFacturaResponse) {
        View convertViewfinal = getViewTipoDeServicio(parent);
        TextView templatePadre_tvServicio =  convertViewfinal.findViewById(R.id.templatePadre_tvServicio);
        ImageView templatePadre_ivIconoServicio =  convertViewfinal.findViewById(R.id.templatePadre_ivIconoServicio);
        TextView templatePadre_tvValor =  convertViewfinal.findViewById(R.id.templatePadre_tvValor);
        ImageView templatePadre_ivMasMenos =  convertViewfinal.findViewById(R.id.templatePadre_ivMasMenos);
        ImageView templatePadre_ivSeparador =  convertViewfinal.findViewById(R.id.templatePadre_ivSeparador);

        if (this.listViewExpandible.isGroupExpanded(groupPosition)) {
            templatePadre_ivMasMenos.setImageResource(R.mipmap.icon_menos_detalle);
        } else {
            templatePadre_ivMasMenos.setImageResource(R.mipmap.icon_mas_detalle);
        }

        LinearLayout templatePadre_llContenedor =  convertViewfinal.findViewById(R.id.templatePadre_llContenedor);
        templatePadre_llContenedor.setOnClickListener(v -> {
            if (listViewExpandible.isGroupExpanded(groupPosition)) {
                listViewExpandible.collapseGroupWithAnimation(groupPosition);
            } else {
                listViewExpandible.expandGroupWithAnimation(groupPosition);
            }
        });

        if (groupPosition == 0) {
            templatePadre_ivSeparador.setVisibility(View.GONE);
        } else {
            templatePadre_ivSeparador.setVisibility(View.VISIBLE);
        }

        String tituloSelServicio = servicioFacturaResponse.getServicio();
        int iconoDelServicio = this.function.getIconoTipoServicio(tituloSelServicio);

        tituloSelServicio = formatDateFactura.primeroLetraMayuscula(tituloSelServicio);
        templatePadre_tvServicio.setText(tituloSelServicio);

        if (iconoDelServicio != 0) {
            templatePadre_ivIconoServicio.setImageResource(iconoDelServicio);
        }

        String valorAPagar = this.function.getValorAPagar(servicioFacturaResponse);
        templatePadre_tvValor.setText(valorAPagar);

        return convertViewfinal;
    }

    /**
     * Crea la vista de la gráfica.
     *
     * @param parent
     * @return
     */
    private View createViewGrafica(ViewGroup parent) {
        View convertView = this.layoutInflater.inflate(R.layout.template_grafica_de_un_servicio, parent, false);
        return convertView;
    }

    /**
     * Carga la gráfica con su información.
     *
     * @param convertView
     * @param detalleServicioFacturaList
     * @param unidadDeMedida
     */
    private void loadGrafica(View convertView, List<DetalleServicioFactura> detalleServicioFacturaList, String unidadDeMedida) {

        TextView[] textViewsFechasDeLasBarras = getTextViewsFechasDeLasBarras(convertView);
        TextView[] textViewsConsumoDeLasBarras = getTextViewsConsumoDeLasBarras(convertView);
        RelativeLayout[] barrasDeLasGraficas = getBarrasDeLasGraficas(convertView);
        LinearLayout[] linearLayoutsQueContienenLosComponentesDelConsumo = getLinearLayoutsQueContienenLosComponentesDelConsumo(convertView);
        RelativeLayout grafica_rlSinConsumo =  convertView.findViewById(R.id.grafica_rlSinConsumo);
        LinearLayout grafica_llGrafica =  convertView.findViewById(R.id.grafica_llGrafica);

        showBarrasDeLaGrafica(linearLayoutsQueContienenLosComponentesDelConsumo, detalleServicioFacturaList.size());

        if (detalleServicioFacturaList.size() == 0) {
            grafica_llGrafica.setVisibility(View.GONE);
            grafica_rlSinConsumo.setVisibility(View.VISIBLE);
            return;
        } else {
            grafica_rlSinConsumo.setVisibility(View.GONE);
            grafica_llGrafica.setVisibility(View.VISIBLE);
        }

        int indiceDelMayorConsumo = this.function.getIndiceMayorConsumo(detalleServicioFacturaList);

        int indiceDelConsumoIntermedio = this.function.getIndiceConsumoIntermedio(indiceDelMayorConsumo, detalleServicioFacturaList);

        int indiceDelMenorConsumo = this.function.getIndiceMenorConsumo(indiceDelMayorConsumo, indiceDelConsumoIntermedio, detalleServicioFacturaList);

        detalleServicioFacturaList = this.function.getDetalleDeServicioConAnchoYColorDelMayor(detalleServicioFacturaList, indiceDelMayorConsumo);

        if (indiceDelConsumoIntermedio != -1) {
            detalleServicioFacturaList = this.function.getDetalleDeServicioConAnchoYColorDelConsumoIntermedio(detalleServicioFacturaList, indiceDelMayorConsumo, indiceDelConsumoIntermedio);
        }

        if (indiceDelMenorConsumo != -1) {
            detalleServicioFacturaList = this.function.getDetalleDeServicioConAnchoYColorDelMenorConsumo(detalleServicioFacturaList, indiceDelMayorConsumo, indiceDelMenorConsumo);
        }

        showMesesDelConsumo(textViewsFechasDeLasBarras, detalleServicioFacturaList);

        showConsumoDeLosMeses(textViewsConsumoDeLasBarras, detalleServicioFacturaList, unidadDeMedida);

        changeTamanioDeLasBarras(barrasDeLasGraficas, detalleServicioFacturaList);

    }

    /**
     * Obtiene un arreglo con los componentes del consumo.
     *
     * @param convertView
     * @return
     */
    private LinearLayout[] getLinearLayoutsQueContienenLosComponentesDelConsumo(View convertView) {

        LinearLayout[] linearLayoutsQueContienenLosComponentesDelConsumo = new LinearLayout[3];
        linearLayoutsQueContienenLosComponentesDelConsumo[0] =  convertView.findViewById(R.id.grafica_llPrimerBarra);
        linearLayoutsQueContienenLosComponentesDelConsumo[1] =  convertView.findViewById(R.id.grafica_llSegundaBarra);
        linearLayoutsQueContienenLosComponentesDelConsumo[2] =  convertView.findViewById(R.id.grafica_llTercerBarra);

        return linearLayoutsQueContienenLosComponentesDelConsumo;
    }

    /**
     * Obtiene un arreglo con las barras de las gráficas.
     *
     * @param convertView
     * @return
     */
    private RelativeLayout[] getBarrasDeLasGraficas(View convertView) {
        RelativeLayout[] barrasDeLasGraficas = new RelativeLayout[3];
        barrasDeLasGraficas[0] =  convertView.findViewById(R.id.grafica_rlPrimerBarra);
        barrasDeLasGraficas[1] =  convertView.findViewById(R.id.grafica_rlSegundaBarra);
        barrasDeLasGraficas[2] =  convertView.findViewById(R.id.grafica_rlTercerBarra);

        return barrasDeLasGraficas;
    }

    /**
     * Obtiene un arreglo con los tex views del consumo de las barras.
     *
     * @param convertView
     * @return
     */
    private TextView[] getTextViewsConsumoDeLasBarras(View convertView) {

        TextView[] textViewsConsumoDeLasBarras = new TextView[3];
        textViewsConsumoDeLasBarras[0] =  convertView.findViewById(R.id.grafica_tvPrimerConsumo);
        textViewsConsumoDeLasBarras[1] =  convertView.findViewById(R.id.grafica_tvSegundoConsumo);
        textViewsConsumoDeLasBarras[2] =  convertView.findViewById(R.id.grafica_tvTercerConsumo);

        return textViewsConsumoDeLasBarras;
    }

    /**
     * Establece el tamaño de las barras.
     *
     * @param barrasDeLasGraficas
     * @param detalleServicioFacturaList
     */
    private void changeTamanioDeLasBarras(RelativeLayout[] barrasDeLasGraficas, List<DetalleServicioFactura> detalleServicioFacturaList) {

        int numeroDeItemsEnLaLista = detalleServicioFacturaList.size();

        for (int i = 0; i < numeroDeItemsEnLaLista; i++) {
            barrasDeLasGraficas[i].getLayoutParams().width = detalleServicioFacturaList.get(i).getTamanioDeLaBarra();
            barrasDeLasGraficas[i].setBackgroundResource(detalleServicioFacturaList.get(i).getColorDeLaBarra());
        }
    }

    /**
     * Muestra el valor del consumo de los meses en las barras.
     *
     * @param textViewsConsumoDeLasBarras
     * @param detalleServicioFacturaList
     * @param unidadDeMedida
     */
    private void showConsumoDeLosMeses(TextView[] textViewsConsumoDeLasBarras, List<DetalleServicioFactura> detalleServicioFacturaList, String unidadDeMedida) {

        int numeroDeItemsEnLaLista = detalleServicioFacturaList.size();

        int consumo;
        String consumoConUnidadDeMedida;

        for (int i = 0; i < numeroDeItemsEnLaLista; i++) {
            consumo = detalleServicioFacturaList.get(i).getValor();
            consumoConUnidadDeMedida = consumo + " " + unidadDeMedida;
            textViewsConsumoDeLasBarras[i].setText(consumoConUnidadDeMedida);
            textViewsConsumoDeLasBarras[i].setTextColor(activity.getResources().getColor(detalleServicioFacturaList.get(i).getColorDeLaBarra()));
        }
    }

    /**
     * Muestra la información de los meses en el detalle del consumo.
     *
     * @param textViewsFechasDeLasBarras
     * @param detalleServicioFacturaList
     */
    private void showMesesDelConsumo(TextView[] textViewsFechasDeLasBarras, List<DetalleServicioFactura> detalleServicioFacturaList) {

        int numeroDeItemsEnLaLista = detalleServicioFacturaList.size();

        for (int i = 0; i < numeroDeItemsEnLaLista; i++) {
            textViewsFechasDeLasBarras[i].setText(detalleServicioFacturaList.get(i).getDescripcion());
            textViewsFechasDeLasBarras[i].setTextColor(activity.getResources().getColor(detalleServicioFacturaList.get(i).getColorDeLaBarra()));
        }
    }

    /**
     * Obtiene un arreglo con los text views de las fechas de las barras.
     *
     * @param convertView
     * @return
     */
    private TextView[] getTextViewsFechasDeLasBarras(View convertView) {

        TextView[] textViewsFechasDeLasBarras = new TextView[3];
        textViewsFechasDeLasBarras[0] =  convertView.findViewById(R.id.grafica_tvPrimerMes);
        textViewsFechasDeLasBarras[1] =  convertView.findViewById(R.id.grafica_tvSegundoMes);
        textViewsFechasDeLasBarras[2] =  convertView.findViewById(R.id.grafica_tvTercerMes);

        return textViewsFechasDeLasBarras;
    }

    /**
     * Muestra las barras de las gráficas.
     *
     * @param linearLayoutsQueContienenLosComponentesDelConsumo
     * @param size
     */
    private void showBarrasDeLaGrafica(LinearLayout[] linearLayoutsQueContienenLosComponentesDelConsumo, int size) {

        for (int i = 0; i < size; i++) {
            linearLayoutsQueContienenLosComponentesDelConsumo[i].setVisibility(View.VISIBLE);
        }
    }

    /**
     * Establece la información del consumo + unidad de medida en el control textview.
     *
     * @param grafica_tvConsumo
     * @param consumo
     * @param unidadDeMedida
     */
    private void setConsumoAlTextView(TextView grafica_tvConsumo, int consumo, String unidadDeMedida) {
        String consumoConUnidadDeMedida = Integer.toString(consumo) + " " + unidadDeMedida;
        grafica_tvConsumo.setText(consumoConUnidadDeMedida);
    }

    /**
     * Obtiene la vista del grupo especifico.
     *
     * @param parent
     * @return
     */
    private View getViewTipoDeServicio(ViewGroup parent) {
        View view = this.layoutInflater.inflate(R.layout.template_lista_padre_detalle_consumo, parent, false);
        return view;
    }
}
