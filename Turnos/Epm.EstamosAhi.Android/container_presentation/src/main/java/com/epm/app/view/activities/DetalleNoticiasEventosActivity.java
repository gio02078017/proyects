package com.epm.app.view.activities;

import android.graphics.drawable.Drawable;
import android.os.Bundle;
import androidx.appcompat.widget.Toolbar;
import android.view.View;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.bumptech.glide.load.resource.drawable.GlideDrawable;
import com.bumptech.glide.request.animation.GlideAnimation;
import com.bumptech.glide.request.target.GlideDrawableImageViewTarget;
import com.epm.app.R;

import app.epm.com.container_domain.business_models.NoticiasEventos;
import app.epm.com.utilities.helpers.Validations;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by leidycarolinazuluagabastidas on 29/03/2017.
 */

public class DetalleNoticiasEventosActivity extends BaseActivity {

    private ImageView newsEventsDetal_imgNewsEvents;
    private TextView newsEventsDetail_tvTitle;
    private TextView newsEventsDetail_tvDescription;
    private TextView newsEventsDetal_tvDay;
    private TextView newsEventsDetal_tvMonth;
    private TextView newsEventsDetal_tvYear;
    private ProgressBar newsEventsDetail_pbLoadImage;
    private Toolbar toolbarApp;
    private NoticiasEventos noticiasEventos;
    private Validations validations;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_noticias_o_eventos_detalle);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        validations = new Validations();
        loadViews();
        noticiasEventos = (NoticiasEventos) getIntent().getSerializableExtra(Constants.NEWS_EVENTS);
        showDetailNoticiasEventos(noticiasEventos);
    }

    private void showDetailNoticiasEventos(final NoticiasEventos noticiasEventos) {
        Glide.with(this).load(noticiasEventos.getImagen()).centerCrop().into(new GlideDrawableImageViewTarget(newsEventsDetal_imgNewsEvents) {
            @Override
            public void onResourceReady(GlideDrawable resource, GlideAnimation<? super GlideDrawable> animation) {

                newsEventsDetal_imgNewsEvents.setImageDrawable(resource);

                newsEventsDetal_imgNewsEvents.setBackground(resource);

                newsEventsDetail_pbLoadImage.setVisibility(View.GONE);
            }

            @Override
            public void onLoadFailed(Exception e, Drawable errorDrawable) {
                newsEventsDetail_pbLoadImage.setVisibility(View.GONE);
            }
        });
        newsEventsDetail_tvTitle.setText(noticiasEventos.getTitulo());
        newsEventsDetail_tvDescription.setText(noticiasEventos.getResumen());
        String[] arrayDate = noticiasEventos.getFecha().split("/");
        newsEventsDetal_tvDay.setText(arrayDate[2]);
        newsEventsDetal_tvMonth.setText(arrayDate[1]);
        newsEventsDetal_tvYear.setText(arrayDate[0]);
    }

    private void loadViews() {

        toolbarApp = (Toolbar) findViewById(R.id.toolbar);
        newsEventsDetal_imgNewsEvents = (ImageView) findViewById(R.id.newsEventsDetal_imgNewsEvents);
        newsEventsDetail_tvTitle = (TextView) findViewById(R.id.newsEventsDetail_tvTitle);
        newsEventsDetail_tvDescription = (TextView) findViewById(R.id.newsEventsDetail_tvDescription);
        newsEventsDetal_tvDay = (TextView) findViewById(R.id.newsEventsDetal_tvDay);
        newsEventsDetal_tvMonth = (TextView) findViewById(R.id.newsEventsDetal_tvMonth);
        newsEventsDetal_tvYear = (TextView) findViewById(R.id.newsEventsDetal_tvYear);
        newsEventsDetail_pbLoadImage = (ProgressBar) findViewById(R.id.newsEventsDetail_pbLoadImage);
        loadToolbar();
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar((Toolbar) toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        toolbarApp.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onBackPressed();
            }
        });
    }
}