package com.epm.app.view.adapters;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.drawable.Drawable;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.bumptech.glide.load.resource.drawable.GlideDrawable;
import com.bumptech.glide.request.animation.GlideAnimation;
import com.bumptech.glide.request.target.GlideDrawableImageViewTarget;
import com.epm.app.R;
import com.epm.app.view.activities.DetalleNoticiasEventosActivity;
import java.util.List;

import app.epm.com.container_domain.business_models.NoticiasEventos;
import app.epm.com.utilities.helpers.Validations;
import app.epm.com.utilities.utils.Constants;

/**
 * Created by leidycarolinazuluaga on 29/03/2017.
 */

public class NoticiaOEventoAdapter extends ArrayAdapter {

    private Activity context;
    private List<NoticiasEventos> noticiasEventosList;
    private LayoutInflater layoutInflater;
    private Validations validation;

    public NoticiaOEventoAdapter(Activity context, int resource, List<NoticiasEventos> noticiasEventosList) {
        super(context, resource, noticiasEventosList);
        this.context = context;
        this.noticiasEventosList = noticiasEventosList;
        layoutInflater = context.getLayoutInflater();
        validation = new Validations();
    }

    public static class ViewHolder {
        TextView listNewsEvents_tvDay;
        TextView listNewsEvents_tvMonth;
        TextView listNewsEvents_tvYear;
        TextView listNewsEvents_tvDescriptionBottom;
        ProgressBar listNewsEvents_pgLoading;
        ImageView listNewsEvents_imgContainer;
        LinearLayout listNewsEvents_llOpenDetail;
    }

    @Override
    public int getCount() {
        return this.noticiasEventosList.size();
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        View rowView = convertView;
        final ViewHolder viewHolder;
        if (rowView == null) {
            rowView = layoutInflater.inflate(R.layout.list_noticias_o_eventos, null, true);
            viewHolder = new ViewHolder();
            viewHolder.listNewsEvents_tvDay =  rowView.findViewById(R.id.listNewsEvents_tvDay);
            viewHolder.listNewsEvents_tvMonth =  rowView.findViewById(R.id.listNewsEvents_tvMonth);
            viewHolder.listNewsEvents_tvYear =  rowView.findViewById(R.id.listNewsEvents_tvYear);
            viewHolder.listNewsEvents_tvDescriptionBottom = rowView.findViewById(R.id.listNewsEvents_tvDescriptionBottom);
            viewHolder.listNewsEvents_pgLoading =  rowView.findViewById(R.id.listNewsEvents_pgLoading);
            viewHolder.listNewsEvents_imgContainer =  rowView.findViewById(R.id.listNewsEvents_imgContainer);
            viewHolder.listNewsEvents_llOpenDetail =  rowView.findViewById(R.id.listNewsEvents_llOpenDetail);

            rowView.setTag(viewHolder);
        } else {
            viewHolder = (ViewHolder) rowView.getTag();
        }

        viewHolder.listNewsEvents_pgLoading.setVisibility(View.VISIBLE);
        final NoticiasEventos noticiasEventos = noticiasEventosList.get(position);

        String[] arrayDate = noticiasEventos.getFecha().split("/");

        viewHolder.listNewsEvents_tvDay.setText(arrayDate[2]);

        viewHolder.listNewsEvents_tvMonth.setText(arrayDate[1]);

        viewHolder.listNewsEvents_tvYear.setText(arrayDate[0]);

        viewHolder.listNewsEvents_tvDescriptionBottom.setText(noticiasEventos.getTitulo());

        Glide.with(context).load(noticiasEventos.getImagen()).centerCrop().into(new GlideDrawableImageViewTarget(viewHolder.listNewsEvents_imgContainer) {
            @Override
            public void onResourceReady(GlideDrawable resource, GlideAnimation<? super GlideDrawable> animation) {

                viewHolder.listNewsEvents_imgContainer.setImageDrawable(resource);

                viewHolder.listNewsEvents_imgContainer.setBackground(resource);

                viewHolder.listNewsEvents_pgLoading.setVisibility(View.GONE);
            }

            @Override
            public void onLoadFailed(Exception e, Drawable errorDrawable) {
                viewHolder.listNewsEvents_pgLoading.setVisibility(View.GONE);
            }
        });

        viewHolder.listNewsEvents_llOpenDetail.setOnClickListener(view -> {
            Bitmap image = viewHolder.listNewsEvents_imgContainer.getDrawingCache();
            Intent intent = new Intent(context, DetalleNoticiasEventosActivity.class);
            intent.putExtra(Constants.NEWS_EVENTS, noticiasEventos);
            intent.putExtra(Constants.IMAGE_NEWS_EVENTS, image);
            context.startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
        });

        return rowView;
    }
}