package app.epm.com.factura_presentation.view.adapters;

import android.app.Activity;
import android.content.Context;

import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.TextView;

import java.util.List;

import app.epm.com.facturadomain.business_models.DataContratos;
import app.epm.com.facturadomain.business_models.FacturaResponse;
import app.epm.com.factura_presentation.R;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by leidycarolinazuluagabastidas on 23/01/17.
 */

public class ContratoAdapter extends ArrayAdapter<DataContratos> {

    private Activity context;
    private List<DataContratos> dataContratos;
    private List<FacturaResponse> facturaResponses;


    int positionactual;
    int seleccionado;
    boolean recycleView = false;


    public void setDescriptionContrato(int position, String description) {
        dataContratos.get(position).setDescripcion(description);
        String a = dataContratos.get(position).getDescripcion();
    }

    public String getDescriptionContrato(int position) {
        return dataContratos.get(position).getDescripcion();
    }

    public ContratoAdapter(Activity context, int resource, List<DataContratos> dataContratos, List<FacturaResponse> facturaResponses) {
        super(context, resource, dataContratos);
        this.context = context;
        this.dataContratos = dataContratos;
        this.facturaResponses = facturaResponses;
    }


    @Override
    public int getCount() {
        return this.dataContratos.size();
    }

    public List<DataContratos> getDataContratosList() {
        return this.dataContratos;
    }

    public static class Holder {
        private EditText gestionarContrato_etNombre;
        private TextView gestionarContrato_tvNumero;
        private CheckBox gestionarContrato_checDigital;
        private CheckBox gestionarContrato_checkEliminar;
        int positionA;
    }


    @Override
    public View getView(final int position, View convertView, ViewGroup parent) {
        final Holder holder;

        positionactual = position;


        if (convertView == null) {
            final LayoutInflater inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            convertView = inflater.inflate(R.layout.list_contrato_inscrito_item, parent, false);
            holder = new Holder();
            setData(holder, positionactual, convertView);
            loadListener(position, holder);

            holder.gestionarContrato_etNombre.addTextChangedListener(new TextWatcher() {
                @Override
                public void beforeTextChanged(CharSequence charSequence, int i, int i1, int i2) {
                    if (recycleView) {
                        recycleView = false;
                        return;
                    }
                }

                @Override
                public void onTextChanged(CharSequence charSequence, int i, int i1, int i2) {

                    if (!dataContratos.get(positionactual).getDescripcion().equals(charSequence.toString().trim())) {
                        dataContratos.get(seleccionado).setDescripcion(charSequence.toString().trim());
                        dataContratos.get(seleccionado).setOperacion(Constants.OPERACION_ACTUALIZAR);
                        setOperation(dataContratos.get(seleccionado), dataContratos.get(holder.positionA).getOperacion());
                    }
                }

                @Override
                public void afterTextChanged(Editable editable) {
                    //The method is not used.
                }
            });


            convertView.setTag(holder);

        } else {
            holder = (Holder) convertView.getTag();
            recycleView = true;
            setData(holder, positionactual, convertView);
        }
        holder.positionA = position;
        setFacturaResponses(this.facturaResponses);

        holder.gestionarContrato_etNombre.setOnFocusChangeListener((view, b) -> seleccionado = position);

        return convertView;
    }

    private void setData(Holder holder, int position, View convertView) {

        if (convertView != null) {
            holder.gestionarContrato_etNombre = convertView.findViewById(R.id.gestionarContrato_etNombre);
            holder.gestionarContrato_etNombre.clearFocus();
            holder.gestionarContrato_checDigital =  convertView.findViewById(R.id.gestionarContrato_checDigital);
            holder.gestionarContrato_checkEliminar =  convertView.findViewById(R.id.gestionarContrato_checkEliminar);
            holder.gestionarContrato_tvNumero =  convertView.findViewById(R.id.gestionarContrato_tvNumero);
        }

        holder.gestionarContrato_etNombre.setText(this.dataContratos.get(position).getDescripcion());
        holder.gestionarContrato_tvNumero.setText(this.dataContratos.get(position).getNumero());
        holder.gestionarContrato_checDigital.setChecked(this.dataContratos.get(position).isRecibirFacturaDigital());
        holder.gestionarContrato_checkEliminar.setChecked(this.dataContratos.get(position).isEliminar());
        holder.gestionarContrato_etNombre.clearFocus();
    }


    public List<FacturaResponse> getFacturaResponses() {
        return facturaResponses;
    }

    public void setFacturaResponses(List<FacturaResponse> facturaResponses) {
        this.facturaResponses = facturaResponses;
    }

    private void loadListener(final int position, final Holder holder) {


        holder.gestionarContrato_checDigital.setOnClickListener(view -> {
            dataContratos.get(holder.positionA).setRecibirFacturaDigital(!dataContratos.get(holder.positionA).isRecibirFacturaDigital());
            dataContratos.get(holder.positionA).setOperacion(Constants.OPERACION_ACTUALIZAR);
            setOperation(dataContratos.get(holder.positionA), dataContratos.get(holder.positionA).getOperacion());
        });

        holder.gestionarContrato_checkEliminar.setOnClickListener(view -> {
            dataContratos.get(holder.positionA).setEliminar(!dataContratos.get(holder.positionA).isEliminar());
            dataContratos.get(holder.positionA).setOperacion(Constants.OPERACION_ACTUALIZAR);
            setOperation(dataContratos.get(holder.positionA), dataContratos.get(holder.positionA).getOperacion());
            setInfoFacturaResponse(dataContratos.get(holder.positionA).getNumero(), getFacturaResponses(), dataContratos.get(holder.positionA).isEliminar());
        });
    }

    private void setOperation(DataContratos dataContratos, int operation) {

        if (dataContratos.isEliminar()) {
            operation = Constants.OPERACION_ELIMINAR;
        } else if (dataContratos.isRecibirFacturaDigital()) {
            operation = Constants.OPERACION_ACTUALIZAR;
        }
        dataContratos.setOperacion(operation);
    }

    private void setInfoFacturaResponse(String numeroContrato, List<FacturaResponse> facturaResponse, boolean eliminar) {
        for (FacturaResponse item : facturaResponse) {
            if (item.getNumeroContrato().equals(numeroContrato)) {
                item.setEliminar(eliminar);
                setFacturaResponses(facturaResponse);
            }
        }
    }
}
