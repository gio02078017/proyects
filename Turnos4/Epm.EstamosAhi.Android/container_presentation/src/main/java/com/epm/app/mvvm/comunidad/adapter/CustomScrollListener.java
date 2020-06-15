package com.epm.app.mvvm.comunidad.adapter;

import android.support.annotation.NonNull;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;

import com.epm.app.mvvm.comunidad.viewModel.NotificationCenterViewModel;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.INotificationCenterViewModel;

import app.epm.com.utilities.utils.Constants;

public class CustomScrollListener extends RecyclerView.OnScrollListener {

    private INotificationCenterViewModel notificationCenterViewModel;
    private LinearLayoutManager linearLayoutManager;

    public CustomScrollListener(NotificationCenterViewModel notificationCenterViewModel, LinearLayoutManager linearLayoutManager) {
        this.notificationCenterViewModel = notificationCenterViewModel;
        this.linearLayoutManager = linearLayoutManager;
    }

    public void onScrollStateChanged(RecyclerView recyclerView, int newState) {
        switch (newState) {
            case RecyclerView.SCROLL_STATE_DRAGGING:
                System.out.println("The RecyclerView is not scrolling");
                notificationCenterViewModel.loadNotifications();
                break;

        }

    }


}