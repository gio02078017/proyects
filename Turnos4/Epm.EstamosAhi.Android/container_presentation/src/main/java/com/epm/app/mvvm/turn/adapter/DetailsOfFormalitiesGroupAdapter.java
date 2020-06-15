package com.epm.app.mvvm.turn.adapter;

import android.content.Context;
import android.databinding.DataBindingUtil;
import android.text.method.ScrollingMovementMethod;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.epm.app.R;
import com.epm.app.databinding.ListChildItemBinding;
import com.epm.app.databinding.ListGruopItemBinding;
import com.epm.app.mvvm.turn.models.DetailOfFormalities;
import com.epm.app.mvvm.turn.models.DetailOfFormalitiesGroup;
import com.epm.app.mvvm.turn.models.ParentListItem;
import com.epm.app.mvvm.utilAdapter.ExpandableRecyclerAdapter;

import java.util.List;

public class DetailsOfFormalitiesGroupAdapter extends ExpandableRecyclerAdapter<DetailsOfFormalitiesGroupViewHolder, DetailsOfFormalitiesViewHolder> {

    private LayoutInflater mInflator;

    public DetailsOfFormalitiesGroupAdapter(Context context, List<? extends ParentListItem> parentItemList) {
        super(parentItemList);
        mInflator = LayoutInflater.from(context);
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

    @Override
    public void onBindChildViewHolder(DetailsOfFormalitiesViewHolder moviesViewHolder, int position, Object childListItem) {
        DetailOfFormalities movies = (DetailOfFormalities) childListItem;
        moviesViewHolder.bind(movies);
    }
}