package com.epm.app.adapters;

import android.content.Context;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.epm.app.R;
import com.epm.app.view.views_activities.ILineasDeAtencionView;

import java.util.List;

import app.epm.com.container_domain.business_models.LineaDeAtencion;

/**
 * Created by root on 30/03/17.
 */

public class LineasDeAtencionRecyclerViewAdapter extends RecyclerView.Adapter<RecyclerView.ViewHolder> {

    private Context context;
    private ILineasDeAtencionView lineasDeAtencionView;
    private List<LineaDeAtencion> lineaDeAtencionList;


    public LineasDeAtencionRecyclerViewAdapter(Context context, ILineasDeAtencionView lineasDeAtencionView,
                                               List<LineaDeAtencion> lineaDeAtencionList) {
        this.context = context;
        this.lineasDeAtencionView = lineasDeAtencionView;
        this.lineaDeAtencionList = lineaDeAtencionList;
    }

    @Override
    public RecyclerView.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(context).inflate(R.layout.list_lineas_de_atencion, parent, false);
        return new CustomViewHolder(view);
    }

    @Override
    public void onBindViewHolder(RecyclerView.ViewHolder holder, int position) {
        CustomViewHolder customViewHolder = (CustomViewHolder) holder;
        final LineaDeAtencion lineaDeAtencion = lineaDeAtencionList.get(position);
        customViewHolder.lineasDeAtencion_TextView_LineName.setText(lineaDeAtencion.getName());
        customViewHolder.lineasDeAtencion_TextView_LineNumber.setText(lineaDeAtencion.getNumber());
        customViewHolder.lineasDeAtencion_View.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                lineasDeAtencionView.showAlerDialogLineasDeAtencion(lineaDeAtencion);
            }
        });
    }

    @Override
    public int getItemCount() {
        return lineaDeAtencionList.size();
    }

    private class CustomViewHolder extends RecyclerView.ViewHolder {

        private TextView lineasDeAtencion_TextView_LineName;
        private TextView lineasDeAtencion_TextView_LineNumber;
        private View lineasDeAtencion_View;

        public CustomViewHolder(View view) {
            super(view);
            lineasDeAtencion_View = view;
            lineasDeAtencion_TextView_LineName = (TextView) view.findViewById(R.id.lineasDeAtencion_TextView_LineName);
            lineasDeAtencion_TextView_LineNumber = (TextView) view.findViewById(R.id.lineasDeAtencion_TextView_LineNumber);
        }
    }
}
