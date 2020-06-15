package app.epm.com.reporte_danios_presentation.repositories;

import android.content.Context;
import android.net.Uri;

import com.epm.app.business_models.dto.ParametrosCoberturaDTO;
import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.epm.app.business_models.business_models.ReportArcGIS;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.ServiciosMapa;
import com.epm.app.business_models.dto.DataDTO;
import com.epm.app.business_models.dto.MensajeDTO;
import com.epm.app.business_models.dto.ParametrosEnviarCorreoDTO;
import com.esri.arcgisruntime.geometry.Geometry;
import com.esri.arcgisruntime.geometry.Point;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.io.File;
import java.net.URI;
import java.util.ArrayList;
import java.util.List;

import com.epm.app.business_models.business_models.InformacionDeUbicacion;
import app.epm.com.reporte_danios_domain.danios.business_models.ParametrosEnviarCorreo;
import app.epm.com.reporte_danios_domain.danios.business_models.ReportDanio;
import app.epm.com.reporte_danios_domain.danios.danios.IDaniosRepository;

import com.epm.app.business_models.dto.ServicesMapDTO;

import com.epm.app.business_models.dto.AddressFromArcgisDTO;
import com.epm.app.business_models.dto.CityFromArcgisDTO;
import app.epm.com.reporte_danios_presentation.helpers.Mapper;
import app.epm.com.reporte_danios_presentation.services.IDaniosServices;
import app.epm.com.utilities.helpers.FileManager;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.UbicationHelper;
import app.epm.com.utilities.services.ServicesArcGIS;
import app.epm.com.utilities.services.ServicesFactory;
import app.epm.com.utilities.utils.Constants;
import retrofit.RetrofitError;

public class DaniosRepository implements IDaniosRepository {

    private final IDaniosServices daniosServices;
    private final Gson gson;
    private final ICustomSharedPreferences customSharedPreferences;

    public DaniosRepository(ICustomSharedPreferences customSharedPreferences) {
        this.customSharedPreferences = customSharedPreferences;
        ServicesFactory servicesFactory = new ServicesFactory(customSharedPreferences);
        daniosServices = (IDaniosServices) servicesFactory.getInstance(IDaniosServices.class);
        this.gson = new GsonBuilder().disableHtmlEscaping().create();
    }

    @Override
    public InformacionDeUbicacion getInformacionDeUbicacionWithAddress(String lat, String lon) throws RepositoryError {
        ServicesFactory servicesFactory = new ServicesFactory(customSharedPreferences, Constants.URL_GET_ADDRESS);
        IDaniosServices daniosServices = (IDaniosServices) servicesFactory.getInstance(IDaniosServices.class);
        String location = lat + "," + lon;
        try {
            AddressFromArcgisDTO addressFromArcgisDTO = daniosServices.getInformacionDeUbicacionWithAddress(location, "pjson");
            return Mapper.convertAddressFromArcgisDTOToInformacionDeUbicacionDomain(addressFromArcgisDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public InformacionDeUbicacion getBasicInformacionDeUbicacion(String lat, String lon) throws RepositoryError {
        ServicesFactory servicesFactory = new ServicesFactory(customSharedPreferences, Constants.URL_GET_CITY);
        IDaniosServices daniosServices = (IDaniosServices) servicesFactory.getInstance(IDaniosServices.class);
        String location = String.format(Constants.FORMAT_LOCATION_ARCGIS, lat, lon);
        location = "%7b".concat(location).concat("%7d");
        try {
            CityFromArcgisDTO cityFromArcgisDTO = daniosServices.getBasicInformacionDeUbicacion(location, "pjson", "esriGeometryPoint", "4326", "UBICACION", false);
            return Mapper.convertCityFromArcgisDTOToInformacionDeUbicacion(cityFromArcgisDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public List<String> getMunicipios(ParametrosCobertura parametrosCobertura) throws RepositoryError {
        try {
            ParametrosCoberturaDTO parametrosCoberturaDTO = Mapper.convertParametrosCoberturaDomainToDTO(parametrosCobertura);
            return daniosServices.getMunicipios(parametrosCoberturaDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public Mensaje sendEmail(ParametrosEnviarCorreo parametrosEnviarCorreo) throws RepositoryError {
        try {
            ParametrosEnviarCorreoDTO parametrosEnviarCorreoDTO = Mapper.convertParametrosEnviarCorreoDomainToDTO(parametrosEnviarCorreo);
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(parametrosEnviarCorreoDTO));
            MensajeDTO mensajeDTO = daniosServices.sendEmail(dataDTO);
            return Mapper.convertMensajeDTOToDomain(mensajeDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public ServiciosMapa getServicesKML() throws RepositoryError {
        try {
            ServicesMapDTO servicesMapDTO = daniosServices.getServicesKML();
            return Mapper.convertServicioKMLDTOToDomain(servicesMapDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public String sendReportDanioArcgis(final ReportDanio reportDanio, final ServicesArcGIS servicesArcGIS, final Context context) {
        final ReportArcGIS reportArcGIS = Mapper.convertReportDanioToReportArcGIS(reportDanio);
        reportArcGIS.setFileAttachments(getFilesAttachments(reportDanio, context));
        return servicesArcGIS.createGraphicReportArcGIS(reportArcGIS, UbicationHelper.fromJsonToPoint(reportDanio.getPointSelectedSerializable()), reportDanio.getTypeReporte());
    }

    private ArrayList<File> getFilesAttachments(ReportDanio reportDanio, Context context) {
        ArrayList<File> filesAttachments = new ArrayList<>();
        FileManager filemanager = new FileManager(context);
        for (String path : reportDanio.getArrayFiles()) {

            String realPath = filemanager.getRealPath(path);
            if(realPath == null || realPath.equalsIgnoreCase(Constants.EMPTY_STRING)){
                realPath  = filemanager.fileNotFound(path);
            }

            if(realPath != null && !realPath.equalsIgnoreCase(Constants.EMPTY_STRING)){
                File file = new File(realPath);
                filesAttachments.add(file);
            }
        }

        return filesAttachments;
    }


}



