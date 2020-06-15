package com.epm.app.mvvm.comunidad.adapter;

import android.content.Context;
import android.content.res.Resources;
import android.databinding.DataBindingUtil;
import android.databinding.ViewDataBinding;
import android.support.annotation.NonNull;
import android.support.design.widget.Snackbar;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.helper.ItemTouchHelper;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.epm.app.R;
import com.epm.app.databinding.ItemInformationOfInterestBinding;
import com.epm.app.databinding.ItemNotificationsBinding;
import com.epm.app.mvvm.comunidad.network.response.notifications.ReceivePushNotification;
import com.epm.app.mvvm.comunidad.viewModel.ItemsRecyclerViewModel;

import java.util.List;

import javax.inject.Inject;

public class NotificationsRecyclerAdapter extends RecyclerView.Adapter<NotificationsRecyclerAdapter.CustomViewHolder> {

    private OnNotificationRecyclerListener onNotificationRecyclerListener;
    private List<ReceivePushNotification> items;
    private final int layoutNotifications = R.layout.item_notifications;
    private Resources resources;
    private Context context;
    private ReceivePushNotification mRecentlyDeletedItem;
    private ItemsRecyclerViewModel itemsRecyclerViewModel;
    View view;

    public NotificationsRecyclerAdapter(Context context, List<ReceivePushNotification> items, OnNotificationRecyclerListener onNotificationRecyclerListener, Resources resources,View view) {
        this.context = context;
        this.onNotificationRecyclerListener = onNotificationRecyclerListener;
        this.items = items;
        this.resources =resources;
        this.view = view;
    }

    @NonNull
    @Override
    public CustomViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater layoutInflater = LayoutInflater.from(parent.getContext());
        ViewDataBinding binding;
        ItemNotificationsBinding bindingUpdate;
        ItemInformationOfInterestBinding bindinInformation;
        bindingUpdate = DataBindingUtil.inflate(layoutInflater, viewType, parent, false);
        return new CustomViewHolder(bindingUpdate, onNotificationRecyclerListener);


    }

    @Override
    public void onBindViewHolder(@NonNull CustomViewHolder viewHolder, int position) {
        switch (viewHolder.getItemViewType()) {
            case layoutNotifications:
                ItemNotificationsBinding itemUpdateSubscriptionBinding = viewHolder.bindingSubscription;
                ItemsRecyclerViewModel itemsRecyclerViewModel = new ItemsRecyclerViewModel(this.resources);
                itemsRecyclerViewModel.setReceivePushNotification(items.get(position));
                itemsRecyclerViewModel.typeNotification();
                itemUpdateSubscriptionBinding.setItemViewModel(itemsRecyclerViewModel);
                itemUpdateSubscriptionBinding.cardAlert.setEnabled(true);
                itemUpdateSubscriptionBinding.cardAlert.setOnClickListener(v -> {
                    setItemsRecyclerViewModel(itemsRecyclerViewModel);
                    onNotificationRecyclerListener.onItemClick(position);
                });
                break;
        }

    }

    private void setItemsRecyclerViewModel(ItemsRecyclerViewModel itemsRecyclerViewModel){
        this.itemsRecyclerViewModel = itemsRecyclerViewModel;
    }

    public void updateNotification(){
        itemsRecyclerViewModel.updateState();
    }


    public void deleteItem(int position){
        items.remove(position);
        notifyItemRemoved(position);
    }

    public void clearList(){
        if(items != null && items.size() > 0) {
            int size = this.items.size();
            this.items.clear();
            notifyItemRangeRemoved(0, size);
        }
    }

    public void setItems(List<ReceivePushNotification> items) {
        this.items = items;
        notifyDataSetChanged();
    }

    @Override
    public int getItemViewType(int position) {
        return getLayoutIdForPosition(position);
    }

    private int getLayoutIdForPosition(int position) {
        return layoutNotifications;

    }

    public Context getContext() {
        return context;
    }

    @Override
    public int getItemCount() {
        return items.size();
    }

    class CustomViewHolder extends RecyclerView.ViewHolder {

        private OnNotificationRecyclerListener onDashboardComunityAlertListener;
        private ItemNotificationsBinding bindingSubscription;

        public CustomViewHolder(@NonNull ItemNotificationsBinding bindingUpdate, OnNotificationRecyclerListener onNotificationRecyclerListener) {
            super(bindingUpdate.getRoot());
            this.bindingSubscription = bindingUpdate;
            this.onDashboardComunityAlertListener = onNotificationRecyclerListener;
        }


    }

    public interface OnNotificationRecyclerListener {
        void onItemClick(int position);
    }

}
