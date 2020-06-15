package app.epm.com.reporte_danios_presentation.helpers;


import com.epm.app.business_models.dto.ParametrosCoberturaDTO;
import com.epm.app.business_models.business_models.ETipoServicio;
import com.epm.app.business_models.business_models.FieldMapa;
import com.epm.app.business_models.business_models.Mapa;
import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.epm.app.business_models.business_models.ReportArcGIS;
import com.epm.app.business_models.business_models.ServiciosMapa;
import com.epm.app.business_models.dto.ParametrosEnviarCorreoDTO;

import java.util.ArrayList;
import java.util.List;

import com.epm.app.business_models.business_models.InformacionDeUbicacion;

import app.epm.com.reporte_danios_domain.danios.business_models.ParametrosEnviarCorreo;
import app.epm.com.reporte_danios_domain.danios.business_models.ReportDanio;

import com.epm.app.business_models.dto.AddressFromArcgisDTO;
import com.epm.app.business_models.dto.CityFromArcgisDTO;
import com.epm.app.business_models.dto.FieldDTO;
import com.epm.app.business_models.dto.MapDTO;
import com.epm.app.business_models.dto.ServicesMapDTO;

import app.epm.com.utilities.helpers.BaseMapper;


/**
 * Created by josetabaresramirez on 2/02/17.
 */

public class Mapper extends BaseMapper {

    public static ParametrosCoberturaDTO convertParametrosCoberturaDomainToDTO(ParametrosCobertura parametrosCobertura) {
        ParametrosCoberturaDTO parametrosCoberturaDTO = new ParametrosCoberturaDTO();
        parametrosCoberturaDTO.setTipoServicio(parametrosCobertura.getTipoServicio());
        parametrosCoberturaDTO.setMunicipio(parametrosCobertura.getMunicipio());
        parametrosCoberturaDTO.setDepartamento(parametrosCobertura.getDepartamento());
        return parametrosCoberturaDTO;
    }


    public static ParametrosEnviarCorreoDTO convertParametrosEnviarCorreoDomainToDTO(ParametrosEnviarCorreo parametrosEnviarCorreo) {
        ParametrosEnviarCorreoDTO parametrosEnviarCorreoDTO = new ParametrosEnviarCorreoDTO();

        parametrosEnviarCorreoDTO.setCorreoElectronico(parametrosEnviarCorreo.getCorreoElectronico());
        parametrosEnviarCorreoDTO.setNombreServicio(parametrosEnviarCorreo.getNombreServicio());
        parametrosEnviarCorreoDTO.setNumeroRadicado(parametrosEnviarCorreo.getNumeroRadicado());

        return parametrosEnviarCorreoDTO;
    }

    public static ReportArcGIS convertReportDanioToReportArcGIS(ReportDanio reportDanio) {
        ReportArcGIS reportArcGIS = new ReportArcGIS();
        reportArcGIS.setIdType(reportDanio.getIdTypeReporte());
        reportArcGIS.setDescription(reportDanio.getDescribeReport());
        reportArcGIS.setTelephone(reportDanio.getTelephoneUserReport());
        reportArcGIS.setAddress(reportDanio.getAddress());
        reportArcGIS.setName(reportDanio.getUserName());
        reportArcGIS.setEmail(reportDanio.getUserEmail());
        reportArcGIS.setLocationReference(reportDanio.getLugarReferencia());
        reportArcGIS.setAffectedService(getServicioAfectado(reportDanio.getTipoServicio().getValue()));
        return reportArcGIS;
    }

    public static int getServicioAfectado(int idTipoServicio) {
        if (idTipoServicio == ETipoServicio.Energia.getValue()) {
            return 2;
        } else if (idTipoServicio == ETipoServicio.Agua.getValue()) {
            return 1;
        } else if (idTipoServicio == ETipoServicio.Gas.getValue()) {
            return 3;
        }

        return 0;
    }
}
