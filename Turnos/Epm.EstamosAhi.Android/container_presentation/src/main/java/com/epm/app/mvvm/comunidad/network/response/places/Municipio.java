package com.epm.app.mvvm.comunidad.network.response.places;

import android.os.Parcel;
import android.os.Parcelable;

import com.google.gson.annotations.SerializedName;

public class Municipio implements Parcelable {

    @SerializedName("IdMunicipio")
    public int idMunicipio;
    @SerializedName("Descripcion")
    public String descripcion;
    @SerializedName("Estado")
    public boolean estado;
    @SerializedName("IdDepartamento")
    public int idDepartamento;
    @SerializedName("OrdenGeograficoMunicipio")
    public int ordenGeograficoMunicipio;

    public Municipio() {
    }

    public Municipio(int idMunicipio, String descripcion, boolean estado, int idDepartamento, int ordenGeograficoMunicipio) {
        this.idMunicipio = idMunicipio;
        this.descripcion = descripcion;
        this.estado = estado;
        this.idDepartamento = idDepartamento;
        this.ordenGeograficoMunicipio = ordenGeograficoMunicipio;
    }


    protected Municipio(Parcel in) {
        idMunicipio = in.readInt();
        descripcion = in.readString();
        estado = in.readByte() != 0;
        idDepartamento = in.readInt();
        ordenGeograficoMunicipio = in.readInt();
    }

    public static final Creator<Municipio> CREATOR = new Creator<Municipio>() {
        @Override
        public Municipio createFromParcel(Parcel in) {
            return new Municipio(in);
        }

        @Override
        public Municipio[] newArray(int size) {
            return new Municipio[size];
        }
    };

    public int getIdMunicipio() {
        return idMunicipio;
    }

    public void setIdMunicipio(int idMunicipio) {
        this.idMunicipio = idMunicipio;
    }

    public String getDescripcion() {
        return descripcion;
    }

    public void setDescripcion(String descripcion) {
        this.descripcion = descripcion;
    }

    public boolean isEstado() {
        return estado;
    }

    public void setEstado(boolean estado) {
        this.estado = estado;
    }

    public int getIdDepartamento() {
        return idDepartamento;
    }

    public void setIdDepartamento(int idDepartamento) {
        this.idDepartamento = idDepartamento;
    }

    public int getOrdenGeograficoMunicipio() {
        return ordenGeograficoMunicipio;
    }

    public void setOrdenGeograficoMunicipio(int ordenGeograficoMunicipio) {
        this.ordenGeograficoMunicipio = ordenGeograficoMunicipio;
    }


    @Override
    public int describeContents() {
        return 0;
    }

    @Override
    public void writeToParcel(Parcel dest, int flags) {
        dest.writeInt(idMunicipio);
        dest.writeString(descripcion);
        dest.writeByte((byte) (estado ? 1 : 0));
        dest.writeInt(idDepartamento);
        dest.writeInt(ordenGeograficoMunicipio);
    }
}
