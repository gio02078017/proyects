package com.epm.app.mvvm.transactions.models;

import android.os.Parcel;
import android.os.Parcelable;

import com.google.gson.annotations.SerializedName;

import java.util.List;

public class Transaction implements Parcelable {

    @SerializedName("Id")
    private String id;
    @SerializedName("Nombre")
    private String name;
    @SerializedName("Descripcion")
    private String description;
    @SerializedName("Activo")
    private boolean active;
    @SerializedName("ListaServicios")
    private List<String> services = null;

    protected Transaction(Parcel in) {
        id = in.readString();
        name = in.readString();
        description = in.readString();
        active = in.readByte() != 0;
        services = in.createStringArrayList();
    }

    public Transaction(String id, String name, String description, boolean active, List<String> services) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.active = active;
        this.services = services;
    }

    @Override
    public void writeToParcel(Parcel dest, int flags) {
        dest.writeString(id);
        dest.writeString(name);
        dest.writeString(description);
        dest.writeByte((byte) (active ? 1 : 0));
        dest.writeStringList(services);
    }

    @Override
    public int describeContents() {
        return 0;
    }

    public static final Creator<Transaction> CREATOR = new Creator<Transaction>() {
        @Override
        public Transaction createFromParcel(Parcel in) {
            return new Transaction(in);
        }

        @Override
        public Transaction[] newArray(int size) {
            return new Transaction[size];
        }
    };

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getDescription() {
        return description;
    }


    public boolean isActive() {
        return active;
    }

    public void setActive(boolean active) {
        this.active = active;
    }

    public List<String> getServices() {
        return services;
    }

    public void setServices(List<String> services) {
        this.services = services;
    }

}
