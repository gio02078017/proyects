package com.epm.app.mvvm.comunidad.views.activities;

import android.arch.lifecycle.ViewModelProviders;
import android.content.Intent;
import android.databinding.DataBindingUtil;
import android.os.Bundle;
import android.support.v4.view.GravityCompat;
import android.support.v4.view.MenuItemCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.widget.CardView;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.util.SparseBooleanArray;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.TextView;

import com.epm.app.R;
import com.epm.app.databinding.ActivityDashboardComunityAlertBinding;
import com.epm.app.mvvm.comunidad.adapter.DashboardComunityAlertAdapter;
import com.epm.app.mvvm.comunidad.viewModel.ItemsRecyclerViewModel;
import com.epm.app.view.activities.LandingActivity;

import java.util.ArrayList;
import java.util.List;

import javax.inject.Inject;

import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;

public class DashboardComunityAlertActivity extends BaseActivityWithDI implements  DashboardComunityAlertAdapter.OnDashboardComunityAlertListener {


    @Inject
    ItemsRecyclerViewModel itemsRecyclerViewModel;
    ActivityDashboardComunityAlertBinding binding;
    DashboardComunityAlertAdapter adapter;
    private SparseBooleanArray seleccionados;
    private Toolbar toolbarApp;
    private CardView cardView;
    private final int UPDATE_SUBSCRIPTION = 0;
    private final int INFORMATION = 1;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this,R.layout.activity_dashboard_comunity_alert);
        this.configureDagger();
        itemsRecyclerViewModel = ViewModelProviders.of(this,viewModelFactory).get(ItemsRecyclerViewModel.class);
        List<ItemsRecyclerViewModel> items = new ArrayList<>();
        items.add(itemsRecyclerViewModel);
        items.add(itemsRecyclerViewModel);
        adapter = new DashboardComunityAlertAdapter(DashboardComunityAlertActivity.this,items,this);
        binding.dashboardComunityAlertRecyclerView.setAdapter(adapter);
        binding.dashboardComunityAlertRecyclerView.setLayoutManager(new LinearLayoutManager(this));
        binding.dashboardComunityAlertRecyclerView.getAdapter();
        getCustomSharedPreferences().addString(Constants.EXIST_NOTIFICATION, "");
        seleccionados = new SparseBooleanArray();
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        toolbarApp = (Toolbar) binding.toolbarComunityAlert;
        loadToolbar();

    }


    @Override
    protected void onResume() {
        super.onResume();
        binding.dashboardComunityAlertRecyclerView.setAdapter(adapter);

    }

    @Override
    public void onBackPressed() {
        if (binding.generalDrawerLayout != null && binding.generalDrawerLayout.isDrawerOpen(GravityCompat.END)) {
            binding.generalDrawerLayout.closeDrawer(GravityCompat.END);
        } else {
            Intent intent=new Intent(this, LandingActivity.class);
            startActivityForResult(intent,Constants.DEFAUL_REQUEST_CODE);
        }

    }
    private void loadToolbar() {
        toolbarApp.setNavigationIcon(R.mipmap.right_arrow);
        setSupportActionBar((Toolbar) toolbarApp);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(app.epm.com.utilities.R.menu.menu_general_white, menu);
        final MenuItem menuItem = menu.findItem(app.epm.com.utilities.R.id.action_notifications);
        View actionView = MenuItemCompat.getActionView(menuItem);
        numberNotifications = (TextView) actionView.findViewById(app.epm.com.utilities.R.id.notification_badge);
        if(customSharedPreferences.getInt(Constants.NUMBER_NOTIFICATIONS) != null) {
            setNumberNotifications(customSharedPreferences.getInt(Constants.NUMBER_NOTIFICATIONS));
        }
        actionView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onOptionsItemSelected(menuItem);
            }
        });
        return true;
    }
    

    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }

    private void startActivityUpdate(int action){
        switch (action){
            case UPDATE_SUBSCRIPTION:{
                Intent intent = new Intent(DashboardComunityAlertActivity.this ,UpdateSubscriptionActivity.class);
                startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
                break;
            }
            case INFORMATION:{
                Intent intent = new Intent(DashboardComunityAlertActivity.this,InformationOfInterestActivity.class);
                startActivityForResult(intent,Constants.DEFAUL_REQUEST_CODE);
                break;
            }
        }

    }


    @Override
    public void onItemClick(int position) {
        startActivityUpdate(position);
    }
}
