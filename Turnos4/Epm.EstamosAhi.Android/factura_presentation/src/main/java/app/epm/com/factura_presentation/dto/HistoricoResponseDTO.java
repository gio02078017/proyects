package app.epm.com.factura_presentation.dto;


import com.epm.app.business_models.dto.MensajeDTO;

import java.util.List;

/**
 * Created by ocadavid on 5/01/2017.
 */
public class HistoricoResponseDTO {
    private MensajeDTO Mensaje;
    private List<HistoricoFacturaResponseDTO> ListaHistoricoFacturaResponse;

    public MensajeDTO getMensaje() {
        return Mensaje;
    }

    public void setMensaje(MensajeDTO mensaje) {
        Mensaje = mensaje;
    }

    public List<HistoricoFacturaResponseDTO> getListaHistoricoFacturaResponse(){ return ListaHistoricoFacturaResponse; }

    public void setListaHistoricoFacturaResponse(List<HistoricoFacturaResponseDTO> listaHistoricoFacturaResponse) { ListaHistoricoFacturaResponse = listaHistoricoFacturaResponse; }
}
