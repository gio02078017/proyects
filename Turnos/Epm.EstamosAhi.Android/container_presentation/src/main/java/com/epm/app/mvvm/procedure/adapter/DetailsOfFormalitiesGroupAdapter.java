package com.epm.app.mvvm.procedure.adapter;

import androidx.databinding.DataBindingUtil;
import android.text.method.ScrollingMovementMethod;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import com.epm.app.R;
import com.epm.app.databinding.ListChildItemBinding;
import com.epm.app.databinding.ListGruopItemBinding;

import com.epm.app.mvvm.turn.models.DetailOfFormalitiesGroup;
import com.epm.app.mvvm.turn.models.ParentListItem;
import com.epm.app.mvvm.utilAdapter.ExpandableRecyclerAdapter;

import java.util.List;

import app.epm.com.utilities.utils.Constants;

public class DetailsOfFormalitiesGroupAdapter extends ExpandableRecyclerAdapter<DetailsOfFormalitiesGroupViewHolder, DetailsOfFormalitiesViewHolder> {



    public DetailsOfFormalitiesGroupAdapter(List<? extends ParentListItem> parentItemList) {
        super(parentItemList);

    }

    @Override
    public DetailsOfFormalitiesGroupViewHolder onCreateParentViewHolder(ViewGroup parentViewGroup) {
        ListGruopItemBinding binding = DataBindingUtil.inflate(LayoutInflater.from(parentViewGroup.getContext()),R.layout.list_gruop_item,parentViewGroup,false);
        return new DetailsOfFormalitiesGroupViewHolder(binding.getRoot());
    }

    @Override
    public DetailsOfFormalitiesViewHolder onCreateChildViewHolder(ViewGroup childViewGroup) {
        ListChildItemBinding binding = DataBindingUtil.inflate(LayoutInflater.from(childViewGroup.getContext()),R.layout.list_child_item,childViewGroup,false);
        binding.textDescriptionItem.setMovementMethod(new ScrollingMovementMethod());
        return new DetailsOfFormalitiesViewHolder(binding.getRoot());
    }

    @Override
    public void onBindParentViewHolder(DetailsOfFormalitiesGroupViewHolder movieCategoryViewHolder, int position, ParentListItem parentListItem) {
        DetailOfFormalitiesGroup movieCategory = (DetailOfFormalitiesGroup) parentListItem;
        movieCategoryViewHolder.bind(movieCategory);
    }

    public void setItems(List<? extends ParentListItem> items, int position) {
        updateList(items);
        expandParent(items.get(position));
    }

    @Override
    public void onBindChildViewHolder(DetailsOfFormalitiesViewHolder moviesViewHolder, int position, Object childListItem) {
        String description = (String) childListItem;
        moviesViewHolder.bind(description);
    }
}