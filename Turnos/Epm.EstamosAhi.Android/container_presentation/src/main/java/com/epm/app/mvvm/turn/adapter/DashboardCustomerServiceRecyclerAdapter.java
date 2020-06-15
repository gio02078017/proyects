package com.epm.app.mvvm.turn.adapter;

import android.content.Context;
import android.content.res.Resources;
import androidx.databinding.DataBindingUtil;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.epm.app.R;
import com.epm.app.databinding.ItemCustomerServiceMenuBinding;
import com.epm.app.mvvm.turn.models.CustomerServiceMenuItem;
import com.epm.app.mvvm.turn.viewModel.DashboardCustomerServiceViewModel;
import java.util.List;

public class DashboardCustomerServiceRecyclerAdapter extends RecyclerView.Adapter<DashboardCustomerServiceRecyclerAdapter.CustomViewHolder> {

    private OnDashboardCustomerServiceRecyclerListener onDashboardCustomerServiceRecyclerListener;
    private List<CustomerServiceMenuItem> items;
    private Resources resources;
    private Context context;

    public DashboardCustomerServiceRecyclerAdapter(Context context, List<CustomerServiceMenuItem> items, OnDashboardCustomerServiceRecyclerListener onDashboardCustomerServiceRecyclerListener, Resources resources) {
        this.context = context;
        this.onDashboardCustomerServiceRecyclerListener = onDashboardCustomerServiceRecyclerListener;
        this.items = items;
        this.resources =resources;
    }

    @NonNull
    @Override
    public CustomViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {

        ItemCustomerServiceMenuBinding binding =
                DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()),
                        R.layout.item_customer_service_menu, parent, false);


        return new CustomViewHolder(binding, onDashboardCustomerServiceRecyclerListener);


    }

    @Override
    public void onBindViewHolder(@NonNull CustomViewHolder viewHolder, int position) {

        CustomerServiceMenuItem  customerServiceMenuItem = items.get(position);

        ItemCustomerServiceMenuBinding item = viewHolder.binding;

        DashboardCustomerServiceViewModel dashboardCustomerServiceViewModel = new DashboardCustomerServiceViewModel(resources);

        item.setItemViewModel(dashboardCustomerServiceViewModel);
        dashboardCustomerServiceViewModel.setCustomerServiceMenuItem(customerServiceMenuItem);
        dashboardCustomerServiceViewModel.drawInformation();

        item.viewDividerItemCustomerService.setVisibility(position == 0 ? View.INVISIBLE : View.VISIBLE);

        item.btnAccionCustomerService.setOnClickListener( v -> {
            onDashboardCustomerServiceRecyclerListener.onItemClick(position);
        });

    }

    public Context getContext() {
        return context;
    }

    @Override
    public int getItemCount() {
        return items.size();
    }

    class CustomViewHolder extends RecyclerView.ViewHolder {

        private OnDashboardCustomerServiceRecyclerListener onDashboardCustomerServiceRecyclerListener;
        private ItemCustomerServiceMenuBinding binding;

        public CustomViewHolder(@NonNull ItemCustomerServiceMenuBinding binding, OnDashboardCustomerServiceRecyclerListener onDashboardCustomerServiceRecyclerListener) {
            super(binding.getRoot());
            this.binding = binding;
            this.onDashboardCustomerServiceRecyclerListener = onDashboardCustomerServiceRecyclerListener;
        }


    }

    public interface OnDashboardCustomerServiceRecyclerListener {
        void onItemClick(int position);
    }



}
