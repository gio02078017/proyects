package com.epm.app.mvvm.turn.views.activities;

import androidx.lifecycle.ViewModelProviders;
import android.content.Intent;
import androidx.databinding.DataBindingUtil;
import android.os.Bundle;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.appcompat.widget.Toolbar;

import com.epm.app.R;
import com.epm.app.databinding.ActivityChannelsOfAttentionBinding;
import com.epm.app.mvvm.turn.adapter.ChannelsOfAttentionMenuRecyclerAdapter;
import com.epm.app.mvvm.turn.models.ChannelsOfAttentionMenu;
import com.epm.app.mvvm.turn.viewModel.ChannelsOfAttentionViewModel;
import com.epm.app.view.activities.LineasDeAtencionActivity;

import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivityWithDI;

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
        startActivityWithOutDoubleClick(intent);
    }

    private void goToNearbyOfficesActivity(){
        intent = new Intent(ChannelsOfAttentionActivity.this, NearbyOfficesActivity.class);
        startActivityWithOutDoubleClick(intent);
    }


    @Override
    public void onItemClick(int position) {
        startActivityUpdate(position);
    }
}
