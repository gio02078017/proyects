package com.epm.app.mvvm.turn.adapter;

import android.content.Context;
import android.content.res.Resources;
import android.databinding.DataBindingUtil;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.epm.app.R;
import com.epm.app.databinding.ItemCustomerServiceMenuBinding;
import com.epm.app.mvvm.turn.models.CustomerServiceMenuItem;
import com.epm.app.mvvm.turn.viewModel.CustomerServiceMenuOptionViewModel;
import java.util.List;

public class CustomerServiceMenuOptionRecyclerAdapter extends RecyclerView.Adapter<CustomerServiceMenuOptionRecyclerAdapter.CustomViewHolder> {

    private OnCustomerServiceMenuOptionRecyclerListener onCustomerServiceMenuOptionRecyclerListener;
    private List<CustomerServiceMenuItem> items;
    private Resources resources;
    private Context context;

    public CustomerServiceMenuOptionRecyclerAdapter(Context context, List<CustomerServiceMenuItem> items, OnCustomerServiceMenuOptionRecyclerListener onCustomerServiceMenuOptionRecyclerListener, Resources resources) {
        this.context = context;
        this.onCustomerServiceMenuOptionRecyclerListener = onCustomerServiceMenuOptionRecyclerListener;
        this.items = items;
        this.resources =resources;
    }

    @NonNull
    @Override
    public CustomViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {

        ItemCustomerServiceMenuBinding binding =
                DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()),
                        R.layout.item_customer_service_menu, parent, false);


        return new CustomViewHolder(binding, onCustomerServiceMenuOptionRecyclerListener);


    }

    @Override
    public void onBindViewHolder(@NonNull CustomViewHolder viewHolder, int position) {

        CustomerServiceMenuItem  customerServiceMenuItem = items.get(position);

        ItemCustomerServiceMenuBinding item = viewHolder.binding;

        CustomerServiceMenuOptionViewModel customerServiceMenuOptionViewModel = new CustomerServiceMenuOptionViewModel(resources);

        item.setItemViewModel(customerServiceMenuOptionViewModel);
        customerServiceMenuOptionViewModel.setCustomerServiceMenuItem(customerServiceMenuItem);
        customerServiceMenuOptionViewModel.drawInformation();

        item.viewDividerItemCustomerService.setVisibility(position == 0 ? View.INVISIBLE : View.VISIBLE);

        item.btnAccionCustomerService.setOnClickListener( v -> {
            onCustomerServiceMenuOptionRecyclerListener.onItemClick(position);
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

        private OnCustomerServiceMenuOptionRecyclerListener onCustomerServiceMenuOptionRecyclerListener;
        private ItemCustomerServiceMenuBinding binding;

        public CustomViewHolder(@NonNull ItemCustomerServiceMenuBinding binding, OnCustomerServiceMenuOptionRecyclerListener onCustomerServiceMenuOptionRecyclerListener) {
            super(binding.getRoot());
            this.binding = binding;
            this.onCustomerServiceMenuOptionRecyclerListener = onCustomerServiceMenuOptionRecyclerListener;
        }


    }

    public interface OnCustomerServiceMenuOptionRecyclerListener {
        void onItemClick(int position);
    }



}
