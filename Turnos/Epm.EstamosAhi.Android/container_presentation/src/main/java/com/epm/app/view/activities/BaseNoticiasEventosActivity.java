package com.epm.app.view.activities;

import android.os.Bundle;
import androidx.appcompat.widget.Toolbar;
import android.view.View;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;

import com.epm.app.R;

import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by leidycarolinazuluaga on 29/03/2017.
 */

public abstract class BaseNoticiasEventosActivity<T extends BasePresenter> extends BaseActivity<T> {


    private Toolbar toolbarApp;
    private String modulo;
    private LinearLayout base_llHeader;
    private ImageView base_ivEvent;
    private ListView base_lvList;

    public BaseNoticiasEventosActivity(String modulo) {
        this.modulo = modulo;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_base_noticias_o_eventos);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        createProgressDialog();
        loadViews();
        loadViewsCustom();

    }

    private void loadViewsCustom() {
        switch (modulo) {
            case Constants.EVENTOS:
                loadViewEventos();
                break;
            case Constants.NOTICIAS:
                loadViewsNoticias();
                break;
            default:
                break;
        }
    }

    private void loadViewsNoticias() {
        base_llHeader.setBackgroundResource(R.mipmap.fondo_noticias);
        base_ivEvent.setImageResource(R.mipmap.icon_background_noticias);
    }

    private void loadViewEventos() {
        base_llHeader.setBackgroundResource(R.mipmap.fondo_eventos);
        base_ivEvent.setImageResource(R.mipmap.icon_background_eventos);
    }

    private void loadViews() {
        toolbarApp = (Toolbar) findViewById(R.id.toolbar);
        base_llHeader = (LinearLayout) findViewById(R.id.base_linearLayout_header);
        base_ivEvent = (ImageView) findViewById(R.id.base_imageView_event);
        base_lvList = (ListView) findViewById(R.id.base_listView_list);
        loadToolbar();
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar((Toolbar) toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
                this.toolbarApp.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                onBackPressed();
            }
        });
    }
}
