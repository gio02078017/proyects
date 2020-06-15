package com.epm.app.mvvm.turn.views.activities;

import android.arch.lifecycle.ViewModelProviders;
import android.databinding.DataBindingUtil;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.Toolbar;
import android.widget.ExpandableListAdapter;
import android.widget.ExpandableListView;

import com.epm.app.R;
import com.epm.app.databinding.ActivityDetailsOfTheTransactionBinding;
import com.epm.app.mvvm.turn.adapter.DetailsOfFormalitiesGroupAdapter;
import com.epm.app.mvvm.turn.models.DetailOfFormalities;
import com.epm.app.mvvm.turn.models.DetailOfFormalitiesGroup;
import com.epm.app.mvvm.turn.viewModel.DetailsOfTheTransactionViewModel;
import com.epm.app.mvvm.turn.viewModel.iViewModel.IDetailsOfTheTransactionViewModel;
import com.epm.app.mvvm.utilAdapter.ExpandableRecyclerAdapter;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;

import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class DetailsOfTheTransactionActivity extends BaseActivityWithDI {

    ActivityDetailsOfTheTransactionBinding binding;
    IDetailsOfTheTransactionViewModel detailsOfTheTransactionViewModel;
    private Toolbar toolbarApp;
    private DetailsOfFormalitiesGroupAdapter mAdapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this,R.layout.activity_details_of_the_transaction);
        this.configureDagger();
        detailsOfTheTransactionViewModel = ViewModelProviders.of(this,viewModelFactory).get(DetailsOfTheTransactionViewModel.class);
        binding.setViewModel((DetailsOfTheTransactionViewModel) detailsOfTheTransactionViewModel);
        toolbarApp = (Toolbar) binding.toolbarDetails;
        loadToolbar();
        loadDrawerLayout(R.id.generalDrawerLayout);
        //loadSwipeDrawerLayout();
        loadRecycler();
    }

    private void loadRecycler(){
        DetailOfFormalities  movie_two = new DetailOfFormalities("Presencial \t \nNombre completo y número de documento de identidad del solicitante \n extranjeros: Cédula de Extranjería \n Informar la dirección del inmueble o número del contrato o factura del inmueble. \n Para el cambio de categoría No residencial (Comercial, industrial u oficial) a Residencial, se debe presentar el certificado de estrato. \n Al momento de realizar la revisión el inmueble debe estar ocupado, para poder verificar las condiciones reales de uso del servicio en el inmueble \t\n" +
                "Presencial \t \nNombre completo y número de documento de identidad del solicitante \n extranjeros: Cédula de Extranjería \n Informar la dirección del inmueble o número del identidad del solicitante \n extranjeros: Cédula de Extranjería \n Informar la dirección del inmueble o número del contrato identidad del solicitante \n extranjeros: Cédula de Extranjería \n Informar la dirección del inmueble o número del contrato contrato o factura del inmueble. \n Para el cambio de categoría No residencial (Comercial, industrial u oficial) a Residencial, se debe presentar el certificado de estrato. \n Al momento de realizar la revisión el inmueble debe estar ocupado, para poder verificar las condiciones reales de uso del servicio en el inmueble \t\nPresencial \t \nNombre completo y número de documento de identidad del solicitante \n extranjeros: Cédula de Extranjería \n Informar la dirección del inmueble o número del contrato o factura del inmueble. \n Para el cambio de categoría No residencial (Comercial, industrial u oficial) a Residencial, se debe presentar el certificado de estrato. \n Al momento de realizar la revisión el inmueble debe estar ocupado, para poder verificar las condiciones reales de uso del servicio en el inmueble \t\n"
        );
        DetailOfFormalitiesGroup molvie_category_two = new DetailOfFormalitiesGroup("Thriller", Arrays.asList(movie_two),getResources().getDrawable(R.drawable.ic_question));
        DetailOfFormalities  movie_one = new DetailOfFormalities("inmueble debe estar ocupado, para poder verificar las condiciones reales de uso del servicio en el inmueble \t\n"
        );
        DetailOfFormalitiesGroup molvie_category_four = new DetailOfFormalitiesGroup("Thriller", Arrays.asList(movie_one),getResources().getDrawable(R.drawable.ic_icon_need_list));
        LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false){
            @Override
            public boolean canScrollVertically() {
                return false;
            }
        };
        final List<DetailOfFormalitiesGroup> movieCategories = Arrays.asList(molvie_category_two,molvie_category_four);
        mAdapter = new DetailsOfFormalitiesGroupAdapter(this,movieCategories);
        stateListView();
        binding.listView.setAdapter(mAdapter);
        binding.listView.setLayoutManager(linearLayoutManager);
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar(toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
    }


    private void stateListView(){
        mAdapter.setExpandCollapseListener(new ExpandableRecyclerAdapter.ExpandCollapseListener() {
            @Override
            public void onListItemExpanded(int position) {

            }

            @Override
            public void onListItemCollapsed(int position) {

            }
        });
    }


    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }


}
