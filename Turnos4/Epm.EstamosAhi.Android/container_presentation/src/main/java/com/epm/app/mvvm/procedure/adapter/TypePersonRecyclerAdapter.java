package com.epm.app.mvvm.procedure.adapter;

import android.content.Context;
import android.content.res.Resources;
import android.databinding.DataBindingUtil;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import com.epm.app.R;
import com.epm.app.databinding.ItemTypePersonBinding;
import com.epm.app.mvvm.procedure.network.response.TypePerson.TypePersonItem;
import com.epm.app.mvvm.procedure.viewModel.TypePersonViewModel;

import java.util.List;

public class TypePersonRecyclerAdapter extends RecyclerView.Adapter<TypePersonRecyclerAdapter.CustomViewHolder> {

    private OnTypePersonRecyclerListener onTypePersonRecyclerListener;
    private List<TypePersonItem> items;
    private Resources resources;
    private Context context;

    public TypePersonRecyclerAdapter(Context context, List<TypePersonItem> items, OnTypePersonRecyclerListener onTypePersonRecyclerListener, Resources resources) {
        this.context = context;
        this.onTypePersonRecyclerListener = onTypePersonRecyclerListener;
        this.items = items;
        this.resources = resources;
    }

    @NonNull
    @Override
    public CustomViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {

        ItemTypePersonBinding binding =
                DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()),
                        R.layout.item_type_person, parent, false);

        return new CustomViewHolder(binding);
    }

    @Override
    public void onBindViewHolder(@NonNull CustomViewHolder viewHolder, int position) {

        TypePersonItem typePersonItem = items.get(position);
        ItemTypePersonBinding item = viewHolder.binding;
        TypePersonViewModel itemTypePersonViewModel = new TypePersonViewModel(resources);
        item.setItemViewModelTypePerson(itemTypePersonViewModel);
        itemTypePersonViewModel.setTypePersonItem(typePersonItem);
        itemTypePersonViewModel.drawInformation();
        item.layoutTypePerson.setOnClickListener(v ->{
            if (typePersonItem.isActive())
                onTypePersonRecyclerListener.onItemClick(position);
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

        private ItemTypePersonBinding binding;

        public CustomViewHolder(@NonNull ItemTypePersonBinding binding) {
            super(binding.getRoot());
            this.binding = binding;
        }
    }

    public interface OnTypePersonRecyclerListener {
        void onItemClick(int position);
    }
}
