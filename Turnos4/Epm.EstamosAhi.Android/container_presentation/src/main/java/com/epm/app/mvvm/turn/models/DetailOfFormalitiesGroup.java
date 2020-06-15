package com.epm.app.mvvm.turn.models;

import android.graphics.drawable.Drawable;

import java.util.List;

public class DetailOfFormalitiesGroup implements ParentListItem {
    private String mName;
    private List<DetailOfFormalities> detailOfFormalities;
    private Drawable icon;

    public DetailOfFormalitiesGroup(String name, List<DetailOfFormalities> movies,Drawable icon) {
        mName = name;
        detailOfFormalities = movies;
        this.icon = icon;
    }

    public String getName() {
        return mName;
    }

    public Drawable getIcon() {
        return icon;
    }

    @Override
    public List<?> getChildItemList() {
        return detailOfFormalities;
    }

    @Override
    public boolean isInitiallyExpanded() {
        return false;
    }
}