package app.epm.com.facturadomain.business_models;

import android.support.annotation.Nullable;

/**
 * Created by ocadavid on 22/12/2016.
 */
public class DetalleServicioFactura {

    private int id;

    private String descripcion;

    private @Nullable
    int valor;

    private int colorDeLaBarra;

    private int tamanioDeLaBarra;


    public int getTamanioDeLaBarra() {
        return tamanioDeLaBarra;
    }

    public void setTamanioDeLaBarra(int tamanioDeLaBarra) {
        this.tamanioDeLaBarra = tamanioDeLaBarra;
    }

    public int getColorDeLaBarra() {
        return colorDeLaBarra;
    }

    public void setColorDeLaBarra(int colorDeLaBarra) {
        this.colorDeLaBarra = colorDeLaBarra;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getDescripcion() {
        return descripcion;
    }

    public void setDescripcion(String descripcion) {
        this.descripcion = descripcion;
    }

    @Nullable
    public int getValor() {
        return valor;
    }

    public void setValor(@Nullable int valor) {
        this.valor = valor;
    }
}
