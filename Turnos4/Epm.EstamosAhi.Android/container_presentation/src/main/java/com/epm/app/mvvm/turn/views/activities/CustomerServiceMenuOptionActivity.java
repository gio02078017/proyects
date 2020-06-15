package com.epm.app.mvvm.turn.views.activities;

import android.arch.lifecycle.ViewModelProvider;
import android.arch.lifecycle.ViewModelProviders;
import android.content.Intent;
import android.databinding.DataBindingUtil;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.view.GravityCompat;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.Toolbar;

import com.epm.app.R;
import com.epm.app.databinding.ActivityCustomerServiceMenuOptionBinding;
import com.epm.app.mvvm.procedure.views.activities.GuideProceduresAndRequirementsActivity;
import com.epm.app.mvvm.turn.adapter.CustomerServiceMenuOptionRecyclerAdapter;
import com.epm.app.mvvm.turn.models.CustomerServiceMenu;
import com.epm.app.mvvm.turn.viewModel.CustomerServiceMenuOptionViewModel;
import com.epm.app.view.activities.LandingActivity;

import javax.inject.Inject;

import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;
import dagger.android.AndroidInjection;
import dagger.android.AndroidInjector;
import dagger.android.DispatchingAndroidInjector;
import dagger.android.support.HasSupportFragmentInjector;

public class CustomerServiceMenuOptionActivity extends BaseActivity implements HasSupportFragmentInjector, CustomerServiceMenuOptionRecyclerAdapter.OnCustomerServiceMenuOptionRecyclerListener {

    @Inject
    DispatchingAndroidInjector<Fragment> dispatchingAndroidInjector;
    @Inject
    ViewModelProvider.Factory viewModelFactory;

    ActivityCustomerServiceMenuOptionBinding binding;
    private Toolbar toolbarApp;
    CustomerServiceMenuOptionViewModel customerServiceMenuOptionViewModel;
    CustomerServiceMenuOptionRecyclerAdapter adapter;

    private final int TRAMITES_Y_REQUISITOS = 0;
    private final int CANALES_DE_ATENCION = 1;

    private boolean controlDoubleClick = false;
    private boolean controlDoubleClickBack = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        getCustomSharedPreferences().addString(Constants.EXIST_NOTIFICATION, Constants.EMPTY_STRING);
        binding =  DataBindingUtil.setContentView(this,R.layout.activity_customer_service_menu_option);
        this.configureDagger();
        customerServiceMenuOptionViewModel = ViewModelProviders.of(this,viewModelFactory).get(CustomerServiceMenuOptionViewModel.class);
        customerServiceMenuOptionViewModel.getCustomerServiceMenu();
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        toolbarApp = (Toolbar) binding.toolbarCustomerServiceMenuOption;
        loadToolbar();
        loadBinding();

    }

    private void loadBinding() {
        customerServiceMenuOptionViewModel.getListCustomerServiceMenu().observe(this, listCustomerServiceMenu -> {
            DrawMenu(listCustomerServiceMenu);
        });
    }

    private void DrawMenu(CustomerServiceMenu listCustomerServiceMenu){

          adapter = new CustomerServiceMenuOptionRecyclerAdapter(this,listCustomerServiceMenu.getCustomerServiceMenuItem(),this,getResources());
          LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false);
          binding.customerServiceMenuOptionRecyclerView.setNestedScrollingEnabled(false);
          binding.customerServiceMenuOptionRecyclerView.setHasFixedSize(true);
          binding.customerServiceMenuOptionRecyclerView.setAdapter(adapter);
          binding.customerServiceMenuOptionRecyclerView.setLayoutManager(linearLayoutManager);
          binding.customerServiceMenuOptionRecyclerView.getAdapter();
    }

    @Override
    protected void onResume() {
        super.onResume();
        controlDoubleClick = false;
        controlDoubleClickBack = false;
        binding.customerServiceMenuOptionRecyclerView.setAdapter(adapter);

    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        if (binding.generalDrawerLayout != null && binding.generalDrawerLayout.isDrawerOpen(GravityCompat.END)) {
            binding.generalDrawerLayout.closeDrawer(GravityCompat.END);
        }
    }

    private void actionBackToLanding(){
        if(!controlDoubleClickBack){
            Intent intent=new Intent(this, LandingActivity.class);
            startActivityForResult(intent,Constants.DEFAUL_REQUEST_CODE);
            controlDoubleClickBack = true;
        }
    }

    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        setSupportActionBar((Toolbar) toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
    }


    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }

    private void startActivityUpdate(int action){
        if(!controlDoubleClick) {
            switch (action) {
                case TRAMITES_Y_REQUISITOS: {
                    Intent intent = new Intent(CustomerServiceMenuOptionActivity.this, GuideProceduresAndRequirementsActivity.class);
                    startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
                    break;
                }
                case CANALES_DE_ATENCION: {
                    Intent intent = new Intent(CustomerServiceMenuOptionActivity.this, ChannelsOfAttentionActivity.class);
                    startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
                    break;

                }
            }
            controlDoubleClick = true;
        }
    }


    private void configureDagger() {
        AndroidInjection.inject(this);
    }

    @Override
    public AndroidInjector<Fragment> supportFragmentInjector() {
        return dispatchingAndroidInjector;
    }

    @Override
    public void onItemClick(int position) {
        startActivityUpdate(position);
    }
}
