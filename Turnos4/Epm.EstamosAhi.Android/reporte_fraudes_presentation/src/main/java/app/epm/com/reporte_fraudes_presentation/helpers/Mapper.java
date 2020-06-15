package app.epm.com.reporte_fraudes_presentation.helpers;

import com.epm.app.business_models.dto.ParametrosCoberturaDTO;
import com.epm.app.business_models.business_models.FieldMapa;
import com.epm.app.business_models.business_models.Mapa;
import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.epm.app.business_models.business_models.ReportArcGIS;
import com.epm.app.business_models.business_models.ServiciosMapa;
import com.epm.app.business_models.dto.FieldDTO;
import com.epm.app.business_models.dto.MapDTO;

import java.util.ArrayList;
import java.util.List;

import app.epm.com.reporte_fraudes_domain.business_models.ParametrossReporteDeFraudes;
import app.epm.com.reporte_fraudes_domain.business_models.ReporteDeFraude;
import app.epm.com.reporte_fraudes_presentation.dto.ParametersReporteDeFraudesDTO;
import app.epm.com.utilities.helpers.BaseMapper;

/**
 * Created by mateoquicenososa on 10/04/17.
 */

public class Mapper extends BaseMapper {

    public static ParametersReporteDeFraudesDTO convertParametersReporteDeFraudesDomainToDTO(ParametrossReporteDeFraudes parametrossReporteDeFraudes) {
        ParametersReporteDeFraudesDTO parametersReporteDeFraudesDTO = new ParametersReporteDeFraudesDTO();
        parametersReporteDeFraudesDTO.setCorreoElectronico(parametrossReporteDeFraudes.getCorreoElectronico());
        parametersReporteDeFraudesDTO.setNombre(parametrossReporteDeFraudes.getNombre());
        parametersReporteDeFraudesDTO.setNumeroRadicado(parametrossReporteDeFraudes.getNumeroRadicado());
        parametersReporteDeFraudesDTO.setTipoServicio(parametrossReporteDeFraudes.getTipoServicio());
        return parametersReporteDeFraudesDTO;
    }

    public static ReportArcGIS convertReportFraudeToReportArcGIS(ReporteDeFraude reporteDeFraude) {
        ReportArcGIS reportArcGIS = new ReportArcGIS();
        reportArcGIS.setDescription(reporteDeFraude.getDescribeReport());
        reportArcGIS.setTelephone(reporteDeFraude.getTelephone());
        reportArcGIS.setAddress(reporteDeFraude.getAddress());
        reportArcGIS.setName(reporteDeFraude.getUserName());
        reportArcGIS.setEmail(reporteDeFraude.getUserEmail());
        reportArcGIS.setLocationReference(reporteDeFraude.getReferencePlace());
        reportArcGIS.setAffectedService(getServicioAfectado(reporteDeFraude.getTypeService().getValue()));
        reportArcGIS.setHour(reporteDeFraude.getHorary());
        reportArcGIS.setIdType(reporteDeFraude.getIdType());
        return reportArcGIS;
    }

    public static ParametrosCoberturaDTO convertParametrosCoberturaDomainToDTO(ParametrosCobertura parametrosCobertura) {
        ParametrosCoberturaDTO parametrosCoberturaDTO = new ParametrosCoberturaDTO();
        parametrosCoberturaDTO.setTipoServicio(parametrosCobertura.getTipoServicio());
        parametrosCoberturaDTO.setMunicipio(parametrosCobertura.getMunicipio());
        parametrosCoberturaDTO.setDepartamento(parametrosCobertura.getDepartamento());
        return parametrosCoberturaDTO;
    }
}
