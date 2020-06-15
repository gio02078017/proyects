package app.epm.com.factura_presentation.dto;

import androidx.annotation.Nullable;

/**
 * Created by ocadavid on 22/12/2016.
 */
public class DetalleServicioFacturaDTO {

    private int Id;

    private String Descripcion;

    private
    @Nullable
    int Valor;

    public int getId() {
        return Id;
    }

    public void setId(int id) {
        Id = id;
    }

    public String getDescripcion() {
        return Descripcion;
    }

    public void setDescripcion(String descripcion) {
        Descripcion = descripcion;
    }

    @Nullable
    public int getValor() {
        return Valor;
    }

    public void setValor(@Nullable int valor) {
        Valor = valor;
    }
}
