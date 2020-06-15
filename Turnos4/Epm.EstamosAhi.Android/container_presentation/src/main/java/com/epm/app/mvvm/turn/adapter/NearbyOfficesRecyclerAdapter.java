package com.epm.app.mvvm.turn.adapter;

import android.content.Context;
import android.content.res.Resources;
import android.databinding.DataBindingUtil;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import com.epm.app.R;
import com.epm.app.databinding.ItemNearbyOfficesBinding;
import com.epm.app.mvvm.turn.network.response.nearbyOffices.NearbyOfficesItem;
import com.epm.app.mvvm.turn.viewModel.NearbyOfficesViewModel;

import java.util.List;

public class NearbyOfficesRecyclerAdapter extends RecyclerView.Adapter<NearbyOfficesRecyclerAdapter.CustomViewHolder> {

    private OnNearbyOfficesRecyclerListener onNearbyOfficesRecyclerListener;
    private List<NearbyOfficesItem> items;
    private Resources resources;
    private Context context;

    public NearbyOfficesRecyclerAdapter(Context context, List<NearbyOfficesItem> items, OnNearbyOfficesRecyclerListener onNearbyOfficesRecyclerListener, Resources resources) {
        this.context = context;
        this.onNearbyOfficesRecyclerListener = onNearbyOfficesRecyclerListener;
        this.items = items;
        this.resources =resources;
    }

    @NonNull
    @Override
    public CustomViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {

        ItemNearbyOfficesBinding binding =
                DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()),
                        R.layout.item_nearby_offices, parent, false);

        return new CustomViewHolder(binding, onNearbyOfficesRecyclerListener);
    }

    @Override
    public void onBindViewHolder(@NonNull CustomViewHolder viewHolder, int position) {

        NearbyOfficesItem nearbyOfficesItem = items.get(position);

        ItemNearbyOfficesBinding item = viewHolder.binding;
        NearbyOfficesViewModel itemNearbyOfficesViewModel = new NearbyOfficesViewModel(resources);
        item.setItemViewModel(itemNearbyOfficesViewModel);
        itemNearbyOfficesViewModel.setNearbyOfficesItem(nearbyOfficesItem);
        itemNearbyOfficesViewModel.drawInformation();
        item.layoutNearbyOfficess.setOnClickListener(v -> {
            onNearbyOfficesRecyclerListener.onItemClick(position, nearbyOfficesItem.getIdOficina());
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

        private OnNearbyOfficesRecyclerListener onNearbyOfficesRecyclerListener;
        private ItemNearbyOfficesBinding binding;

        public CustomViewHolder(@NonNull ItemNearbyOfficesBinding binding, OnNearbyOfficesRecyclerListener onNearbyOfficesRecyclerListener) {
            super(binding.getRoot());
            this.binding = binding;
            this.onNearbyOfficesRecyclerListener = onNearbyOfficesRecyclerListener;
        }
    }

    public interface OnNearbyOfficesRecyclerListener {
        void onItemClick(int position, int idOffice );
    }
}
