package com.epm.app.mvvm.turn.models;

import android.os.Parcel;
import android.os.Parcelable;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class ChannelsOfAttentionMenuItem implements Parcelable{

    @SerializedName("TextItemChannelsOfAttentionDescription")
    @Expose
    private String textItemChannelsOfAttentionDescription;
    @SerializedName("ImageItemChannelsOfAttention")
    @Expose
    private String imageItemChannelsOfAttention;
    @SerializedName("TextButtonChannelsOfAttention")
    @Expose
    private String textButtonChannelsOfAttention;

    public ChannelsOfAttentionMenuItem() {
    }

    protected ChannelsOfAttentionMenuItem(Parcel in) {
        textItemChannelsOfAttentionDescription = in.readString();
        imageItemChannelsOfAttention = in.readString();
        textButtonChannelsOfAttention = in.readString();
    }

    public static final Creator<ChannelsOfAttentionMenuItem> CREATOR = new Creator<ChannelsOfAttentionMenuItem>() {
        @Override
        public ChannelsOfAttentionMenuItem createFromParcel(Parcel in) {
            return new ChannelsOfAttentionMenuItem(in);
        }

        @Override
        public ChannelsOfAttentionMenuItem[] newArray(int size) {
            return new ChannelsOfAttentionMenuItem[size];
        }
    };

    public String getTextItemChannelsOfAttentionDescription() {
        return textItemChannelsOfAttentionDescription;
    }

    public void setTextItemChannelsOfAttentionDescription(String textItemChannelsOfAttentionDescription) {
        this.textItemChannelsOfAttentionDescription = textItemChannelsOfAttentionDescription;
    }

    public String getImageItemChannelsOfAttention() {
        return imageItemChannelsOfAttention;
    }

    public void setImageItemChannelsOfAttention(String imageItemChannelsOfAttention) {
        this.imageItemChannelsOfAttention = imageItemChannelsOfAttention;
    }

    public String getTextButtonChannelsOfAttention() {
        return textButtonChannelsOfAttention;
    }

    public void setTextButtonChannelsOfAttention(String textButtonChannelsOfAttention) {
        this.textButtonChannelsOfAttention = textButtonChannelsOfAttention;
    }

    @Override
    public int describeContents() {
        return 0;
    }

    @Override
    public void writeToParcel(Parcel parcel, int i) {
        parcel.writeString(textItemChannelsOfAttentionDescription);
        parcel.writeString(imageItemChannelsOfAttention);
        parcel.writeString(textButtonChannelsOfAttention);
    }
}
