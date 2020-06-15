package com.epm.app.mvvm.turn.models;

import android.graphics.drawable.Drawable;

import java.util.List;

public class DetailOfFormalitiesGroup implements ParentListItem {
    private String mName;
    private List<String> detailOfFormalities;
    private Drawable icon;

    public DetailOfFormalitiesGroup(String name, List<String> detail,Drawable icon) {
        mName = name;
        detailOfFormalities = detail;
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