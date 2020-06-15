package com.epm.app.dto;

import com.epm.app.business_models.dto.ItemGeneralDTO;

import java.util.List;

/**
 * Created by mateoquicenososa on 23/11/16.
 */

public class ListasGeneralesDTO {
    private List<ItemGeneralDTO> TiposIdentificacion;
    private List<ItemGeneralDTO> TiposPersonas;
    private List<ItemGeneralDTO> TiposVivienda;
    private List<ItemGeneralDTO> Generos;

    public List<ItemGeneralDTO> getTiposIdentificacion() {
        return TiposIdentificacion;
    }

    public void setTiposIdentificacion(List<ItemGeneralDTO> tiposIdentificacion) {
        TiposIdentificacion = tiposIdentificacion;
    }

    public List<ItemGeneralDTO> getTiposPersonas() {
        return TiposPersonas;
    }

    public void setTiposPersonas(List<ItemGeneralDTO> tiposPersonas) {
        TiposPersonas = tiposPersonas;
    }

    public List<ItemGeneralDTO> getTiposVivienda() {
        return TiposVivienda;
    }

    public void setTiposVivienda(List<ItemGeneralDTO> tiposVivienda) {
        TiposVivienda = tiposVivienda;
    }

    public List<ItemGeneralDTO> getGeneros() {
        return Generos;
    }

    public void setGeneros(List<ItemGeneralDTO> generos) {
        Generos = generos;
    }
}
