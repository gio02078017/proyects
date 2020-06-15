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
import com.epm.app.databinding.ItemChannelsOfAttentionMenuBinding;
import com.epm.app.mvvm.turn.models.ChannelsOfAttentionMenuItem;
import com.epm.app.mvvm.turn.viewModel.ChannelsOfAttentionViewModel;

import java.util.List;

public class ChannelsOfAttentionMenuRecyclerAdapter extends RecyclerView.Adapter<ChannelsOfAttentionMenuRecyclerAdapter.CustomViewHolder> {

    private OnChannelsOfAttentionMenuRecyclerListener onChannelsOfAttentionMenuRecyclerListener;
    private List<ChannelsOfAttentionMenuItem> items;
    private Resources resources;
    private Context context;

    public ChannelsOfAttentionMenuRecyclerAdapter(Context context, List<ChannelsOfAttentionMenuItem> items, OnChannelsOfAttentionMenuRecyclerListener onChannelsOfAttentionMenuRecyclerListener, Resources resources) {
        this.context = context;
        this.onChannelsOfAttentionMenuRecyclerListener = onChannelsOfAttentionMenuRecyclerListener;
        this.items = items;
        this.resources =resources;
    }

    @NonNull
    @Override
    public CustomViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {


        ItemChannelsOfAttentionMenuBinding binding =
                DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()),
                        R.layout.item_channels_of_attention_menu, parent, false);


        return new CustomViewHolder(binding, onChannelsOfAttentionMenuRecyclerListener);


    }

    @Override
    public void onBindViewHolder(@NonNull CustomViewHolder viewHolder, int position) {

        ChannelsOfAttentionMenuItem channelsOfAttentionMenuItem = items.get(position);

        ItemChannelsOfAttentionMenuBinding item = viewHolder.binding;

        ChannelsOfAttentionViewModel channelsOfAttentionViewModel = new ChannelsOfAttentionViewModel(resources);

        item.setItemViewModel(channelsOfAttentionViewModel);

        channelsOfAttentionViewModel.setChannelsOfAttentionMenuItem(channelsOfAttentionMenuItem);
        channelsOfAttentionViewModel.drawInformation();

        item.viewDividerItemChannelsOfAttention.setVisibility(position == 0 ? View.INVISIBLE : View.VISIBLE);

        item.btnAccionChannelsOfAttention.setOnClickListener( v -> {
            onChannelsOfAttentionMenuRecyclerListener.onItemClick(position);
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

        private OnChannelsOfAttentionMenuRecyclerListener onChannelsOfAttentionMenuRecyclerListener;
        private ItemChannelsOfAttentionMenuBinding binding;

        public CustomViewHolder(@NonNull ItemChannelsOfAttentionMenuBinding binding, OnChannelsOfAttentionMenuRecyclerListener onChannelsOfAttentionMenuRecyclerListener) {
            super(binding.getRoot());
            this.binding = binding;
            this.onChannelsOfAttentionMenuRecyclerListener = onChannelsOfAttentionMenuRecyclerListener;
        }


    }

    public interface OnChannelsOfAttentionMenuRecyclerListener {
        void onItemClick(int position);
    }



}
