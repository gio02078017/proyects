package com.epm.app.mvvm.turn.models;

import android.os.Parcel;
import android.os.Parcelable;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class CustomerServiceMenuItem implements Parcelable{

    @SerializedName("TextItemCustomerServiceDescription")
    @Expose
    private String textItemCustomerServiceDescription;
    @SerializedName("ImageItemCustomerService")
    @Expose
    private String imageItemCustomerService;
    @SerializedName("TextButtonCustomerService")
    @Expose
    private String textButtonCustomerService;

    public CustomerServiceMenuItem() {
    }

    protected CustomerServiceMenuItem(Parcel in) {
        textItemCustomerServiceDescription = in.readString();
        imageItemCustomerService = in.readString();
        textButtonCustomerService = in.readString();
    }

    public static final Creator<CustomerServiceMenuItem> CREATOR = new Creator<CustomerServiceMenuItem>() {
        @Override
        public CustomerServiceMenuItem createFromParcel(Parcel in) {
            return new CustomerServiceMenuItem(in);
        }

        @Override
        public CustomerServiceMenuItem[] newArray(int size) {
            return new CustomerServiceMenuItem[size];
        }
    };

    public String getTextItemCustomerServiceDescription() {
        return textItemCustomerServiceDescription;
    }

    public void setTextItemCustomerServiceDescription(String textItemCustomerServiceDescription) {
        this.textItemCustomerServiceDescription = textItemCustomerServiceDescription;
    }

    public String getImageItemCustomerService() {
        return imageItemCustomerService;
    }

    public void setImageItemCustomerService(String imageItemCustomerService) {
        this.imageItemCustomerService = imageItemCustomerService;
    }

    public String getTextButtonCustomerService() {
        return textButtonCustomerService;
    }

    public void setTextButtonCustomerService(String textButtonCustomerService) {
        this.textButtonCustomerService = textButtonCustomerService;
    }

    @Override
    public int describeContents() {
        return 0;
    }

    @Override
    public void writeToParcel(Parcel parcel, int i) {
        parcel.writeString(textItemCustomerServiceDescription);
        parcel.writeString(imageItemCustomerService);
        parcel.writeString(textButtonCustomerService);
    }
}
