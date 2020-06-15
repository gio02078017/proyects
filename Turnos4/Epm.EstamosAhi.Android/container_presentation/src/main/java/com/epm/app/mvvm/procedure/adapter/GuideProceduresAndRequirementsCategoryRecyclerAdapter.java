package com.epm.app.mvvm.procedure.adapter;

import android.content.Context;
import android.content.res.Resources;
import android.databinding.DataBindingUtil;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import com.epm.app.R;
import com.epm.app.databinding.ItemGuideProceduresAndRequirementsCategoryBinding;
import com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory.GuideProceduresAndRequirementsCategoryItem;
import com.epm.app.mvvm.procedure.viewModel.GuideProceduresAndRequirementsViewModel;

import java.util.List;

public class GuideProceduresAndRequirementsCategoryRecyclerAdapter extends RecyclerView.Adapter<GuideProceduresAndRequirementsCategoryRecyclerAdapter.CustomViewHolder> {

    private OnGuideProceduresAndRequirementsCategoryRecyclerListener onGuideProceduresAndRequirementsCategoryRecyclerListener;
    private List<GuideProceduresAndRequirementsCategoryItem> items;
    private Resources resources;

    public GuideProceduresAndRequirementsCategoryRecyclerAdapter(List<GuideProceduresAndRequirementsCategoryItem> items, OnGuideProceduresAndRequirementsCategoryRecyclerListener onGuideProceduresAndRequirementsCategoryRecyclerListener, Resources resources) {
        this.onGuideProceduresAndRequirementsCategoryRecyclerListener = onGuideProceduresAndRequirementsCategoryRecyclerListener;
        this.items = items;
        this.resources = resources;
    }

    @NonNull
    @Override
    public CustomViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {

        ItemGuideProceduresAndRequirementsCategoryBinding binding =
                DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()),
                        R.layout.item_guide_procedures_and_requirements_category, parent, false);
        return new CustomViewHolder(binding);
    }

    @Override
    public void onBindViewHolder(@NonNull CustomViewHolder viewHolder, int position) {

        GuideProceduresAndRequirementsCategoryItem guideProceduresAndRequirementsCategoryItem = items.get(position);

        ItemGuideProceduresAndRequirementsCategoryBinding item = viewHolder.binding;
        GuideProceduresAndRequirementsViewModel itemGuideProceduresAndRequirementsViewModel = new GuideProceduresAndRequirementsViewModel(resources);
        item.setItemViewModelCategory(itemGuideProceduresAndRequirementsViewModel);
        itemGuideProceduresAndRequirementsViewModel.setGuideProceduresAndRequirementsCategoryItem(guideProceduresAndRequirementsCategoryItem);
        itemGuideProceduresAndRequirementsViewModel.drawInformation();
        item.layoutGuideProceduresAndRequirements.setOnClickListener(v -> {
            if (guideProceduresAndRequirementsCategoryItem.isState())
                onGuideProceduresAndRequirementsCategoryRecyclerListener.onItemClick(position);
        });

    }

    @Override
    public int getItemCount() {
        return items.size();
    }

    class CustomViewHolder extends RecyclerView.ViewHolder {

        private ItemGuideProceduresAndRequirementsCategoryBinding binding;

        public CustomViewHolder(@NonNull ItemGuideProceduresAndRequirementsCategoryBinding binding) {
            super(binding.getRoot());
            this.binding = binding;
        }
    }

    public interface OnGuideProceduresAndRequirementsCategoryRecyclerListener {
        void onItemClick(int position);
    }
}
