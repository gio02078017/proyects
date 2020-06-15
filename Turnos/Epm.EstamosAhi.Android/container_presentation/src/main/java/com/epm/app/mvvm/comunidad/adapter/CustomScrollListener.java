package com.epm.app.mvvm.comunidad.adapter;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.epm.app.mvvm.comunidad.viewModel.NotificationCenterViewModel;
import com.epm.app.mvvm.comunidad.viewModel.iViewModel.INotificationCenterViewModel;

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