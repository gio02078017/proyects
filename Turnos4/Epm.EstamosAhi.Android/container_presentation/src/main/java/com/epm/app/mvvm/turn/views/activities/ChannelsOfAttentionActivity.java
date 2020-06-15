package com.epm.app.mvvm.turn.views.activities;

import android.arch.lifecycle.ViewModelProvider;
import android.arch.lifecycle.ViewModelProviders;
import android.content.Intent;
import android.databinding.DataBindingUtil;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.Toolbar;

import com.epm.app.R;
import com.epm.app.databinding.ActivityChannelsOfAttentionBinding;
import com.epm.app.databinding.ActivityCustomerServiceMenuOptionBinding;
import com.epm.app.mvvm.turn.adapter.ChannelsOfAttentionMenuRecyclerAdapter;
import com.epm.app.mvvm.turn.adapter.CustomerServiceMenuOptionRecyclerAdapter;
import com.epm.app.mvvm.turn.models.ChannelsOfAttentionMenu;
import com.epm.app.mvvm.turn.models.CustomerServiceMenu;
import com.epm.app.mvvm.turn.viewModel.ChannelsOfAttentionViewModel;
import com.epm.app.mvvm.turn.viewModel.CustomerServiceMenuOptionViewModel;
import com.epm.app.view.activities.LandingActivity;
import com.epm.app.view.activities.LineasDeAtencionActivity;

import javax.inject.Inject;

import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;
import dagger.android.AndroidInjection;
import dagger.android.AndroidInjector;
import dagger.android.DispatchingAndroidInjector;
import dagger.android.support.HasSupportFragmentInjector;

public class ChannelsOfAttentionActivity extends BaseActivityWithDI implements  ChannelsOfAttentionMenuRecyclerAdapter.OnChannelsOfAttentionMenuRecyclerListener {


    ActivityChannelsOfAttentionBinding binding;
    private Toolbar toolbarApp;
    Intent intent;

    private final int CALL_US = 0;
    private final int VISIT_US = 1;

    ChannelsOfAttentionViewModel channelsOfAttentionViewModel;
    ChannelsOfAttentionMenuRecyclerAdapter adapter;

    private boolean controlDoubleClick = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding =  DataBindingUtil.setContentView(this,R.layout.activity_channels_of_attention);
        this.configureDagger();

        channelsOfAttentionViewModel = ViewModelProviders.of(this,viewModelFactory).get(ChannelsOfAttentionViewModel.class);

        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        toolbarApp = (Toolbar) binding.toolbarChannelsOfAttention;
        loadToolbar();
        loadBinding();

    }

    private void loadBinding() {
        channelsOfAttentionViewModel.getChannelsOfAttentionMenu();
        channelsOfAttentionViewModel.getListChannelsOfAttention().observe(this, listChannelsOfAttentionMenu -> {
            DrawMenu(listChannelsOfAttentionMenu);
        });
    }

    private void DrawMenu(ChannelsOfAttentionMenu listChannelsOfAttentionMenu){
          adapter = new ChannelsOfAttentionMenuRecyclerAdapter(this,listChannelsOfAttentionMenu.getChannelsOfAttentionMenuItems(),this,getResources());
          LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false);
          binding.channelsOfAttentioRecyclerView.setNestedScrollingEnabled(false);
          binding.channelsOfAttentioRecyclerView.setHasFixedSize(true);
          binding.channelsOfAttentioRecyclerView.setAdapter(adapter);
          binding.channelsOfAttentioRecyclerView.setLayoutManager(linearLayoutManager);
          binding.channelsOfAttentioRecyclerView.getAdapter();
    }

    @Override
    protected void onResume() {
        super.onResume();
        controlDoubleClick = false;
        binding.channelsOfAttentioRecyclerView.setAdapter(adapter);

    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
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
                case CALL_US: {
                    goToLineAtentionActivity();
                    break;
                }
                case VISIT_US: {
                    goToNearbyOfficesActivity();
                    break;

                }
            }
            controlDoubleClick = true;
        }
    }

    private void goToLineAtentionActivity(){
        intent = new Intent(ChannelsOfAttentionActivity.this, LineasDeAtencionActivity.class);
        intent.putExtra(Constants.INFORMATION_ATTENTION_LINES, true);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }

    private void goToNearbyOfficesActivity(){
        intent = new Intent(ChannelsOfAttentionActivity.this, NearbyOfficesActivity.class);
        startActivityForResult(intent, Constants.DEFAUL_REQUEST_CODE);
    }


    @Override
    public void onItemClick(int position) {
        startActivityUpdate(position);
    }
}
