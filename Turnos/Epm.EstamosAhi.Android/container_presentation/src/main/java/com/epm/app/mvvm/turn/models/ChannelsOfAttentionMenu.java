package com.epm.app.mvvm.turn.models;


import android.os.Parcel;
import android.os.Parcelable;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class ChannelsOfAttentionMenu implements Parcelable {

    @SerializedName("Menu")
    @Expose
    private List<ChannelsOfAttentionMenuItem> channelsOfAttentionMenuItems = null;

    public ChannelsOfAttentionMenu(List<ChannelsOfAttentionMenuItem> channelsOfAttentionMenuItems) {
        this.channelsOfAttentionMenuItems = channelsOfAttentionMenuItems;
    }

    public ChannelsOfAttentionMenu(Parcel in) {
        channelsOfAttentionMenuItems = in.createTypedArrayList(ChannelsOfAttentionMenuItem.CREATOR);
    }

    public static final Creator<ChannelsOfAttentionMenu> CREATOR = new Creator<ChannelsOfAttentionMenu>() {
        @Override
        public ChannelsOfAttentionMenu createFromParcel(Parcel in) {
            return new ChannelsOfAttentionMenu(in);
        }

        @Override
        public ChannelsOfAttentionMenu[] newArray(int size) {
            return new ChannelsOfAttentionMenu[size];
        }
    };

    public List<ChannelsOfAttentionMenuItem> getChannelsOfAttentionMenuItems() {
        return channelsOfAttentionMenuItems;
    }

    public void setChannelsOfAttentionMenuItems(List<ChannelsOfAttentionMenuItem> channelsOfAttentionMenuItems) {
        this.channelsOfAttentionMenuItems = channelsOfAttentionMenuItems;
    }

    @Override
    public int describeContents() {
        return 0;
    }

    @Override
    public void writeToParcel(Parcel parcel, int i) {
        parcel.writeTypedList(channelsOfAttentionMenuItems);
    }
}
