package com.epm.app.view.activities;

import android.content.DialogInterface;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.epm.app.R;
import com.epm.app.dependency_injection.DomainModule;
import com.epm.app.presenters.NoticiasPresenter;
import com.epm.app.view.adapters.NoticiaOEventoAdapter;
import com.epm.app.view.views_activities.INoticiasView;

import java.util.List;

import app.epm.com.container_domain.business_models.NoticiasEventos;
import app.epm.com.utilities.controls.CustomTextViewNormal;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */

public class NoticiasActivity extends BaseNoticiasEventosActivity<NoticiasPresenter> implements INoticiasView {

    private ProgressBar base_progressbar_load;
    private ListView base_listView_list;


    public NoticiasActivity() {
        super(Constants.NOTICIAS);
    }


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setPresenter(new NoticiasPresenter(DomainModule.getNoticiasBussinesLogicInstance(getCustomSharedPreferences())));
        getPresenter().inject(this, getValidateInternet());
        loadView();
        getPresenter().validateInternetToGetNoticias();

    }

    private void loadView() {
        base_progressbar_load = (ProgressBar) findViewById(R.id.base_progressbar_load);
        base_listView_list = (ListView) findViewById(R.id.base_listView_list);
    }


    @Override
    public void showInformationNoticias(final List<NoticiasEventos> noticiasEventos) {
        runOnUiThread(() -> {
            base_progressbar_load.setVisibility(View.GONE);
            loadNoticias(noticiasEventos);
        });
    }

    private void loadNoticias(List<NoticiasEventos> noticiasEventos) {
        if (noticiasEventos != null && noticiasEventos.size() > 0) {
            NoticiaOEventoAdapter adapter = new NoticiaOEventoAdapter(NoticiasActivity.this, R.id.base_listView_list, noticiasEventos);
            base_listView_list.setAdapter(adapter);
        }
    }

    @Override
    public void showAlertDialogToLoadAgain(int title, String message) {
       showAlertDialog(getString(title), message);
    }

    @Override
    public void showAlertDialogToLoadAgain(String title, int message) {
        showAlertDialog(title, getString(message));
    }

    private void showAlertDialog(final String title, final String message) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(title, message, false, R.string.text_intentar, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                getPresenter().validateInternetToGetNoticias();
                dialogInterface.dismiss();
            }
        }, R.string.text_cancelar, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                onBackPressed();
            }
        }, null));

    }
}
