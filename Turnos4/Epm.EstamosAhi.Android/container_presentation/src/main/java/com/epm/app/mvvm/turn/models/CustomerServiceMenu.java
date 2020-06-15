package com.epm.app.mvvm.turn.models;


import android.os.Parcel;
import android.os.Parcelable;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class CustomerServiceMenu implements Parcelable {

    @SerializedName("Menu")
    @Expose
    private List<CustomerServiceMenuItem> customerServiceMenuItem = null;

    public CustomerServiceMenu(List<CustomerServiceMenuItem> customerServiceMenuItem) {
        this.customerServiceMenuItem = customerServiceMenuItem;
    }

    public CustomerServiceMenu(Parcel in) {
        customerServiceMenuItem = in.createTypedArrayList(CustomerServiceMenuItem.CREATOR);
    }

    public static final Creator<CustomerServiceMenu> CREATOR = new Creator<CustomerServiceMenu>() {
        @Override
        public CustomerServiceMenu createFromParcel(Parcel in) {
            return new CustomerServiceMenu(in);
        }

        @Override
        public CustomerServiceMenu[] newArray(int size) {
            return new CustomerServiceMenu[size];
        }
    };

    public List<CustomerServiceMenuItem> getCustomerServiceMenuItem() {
        return customerServiceMenuItem;
    }

    public void setCustomerServiceMenuItem(List<CustomerServiceMenuItem> customerServiceMenuItem) {
        this.customerServiceMenuItem = customerServiceMenuItem;
    }


    @Override
    public int describeContents() {
        return 0;
    }

    @Override
    public void writeToParcel(Parcel parcel, int i) {
        parcel.writeTypedList(customerServiceMenuItem);
    }
}
