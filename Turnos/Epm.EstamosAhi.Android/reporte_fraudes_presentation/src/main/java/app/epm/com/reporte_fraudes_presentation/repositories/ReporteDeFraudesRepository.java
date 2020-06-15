package app.epm.com.reporte_fraudes_presentation.repositories;

import android.content.Context;

import com.epm.app.business_models.dto.AddressFromArcgisDTO;
import com.epm.app.business_models.dto.CityFromArcgisDTO;
import com.epm.app.business_models.business_models.InformacionDeUbicacion;
import com.epm.app.business_models.dto.ParametrosCoberturaDTO;
import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.ParametrosCobertura;
import com.epm.app.business_models.business_models.ReportArcGIS;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.ServiciosMapa;
import com.epm.app.business_models.dto.DataDTO;
import com.epm.app.business_models.dto.MensajeDTO;
import com.epm.app.business_models.dto.ServicesMapDTO;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import app.epm.com.reporte_fraudes_domain.business_models.ParametrossReporteDeFraudes;
import app.epm.com.reporte_fraudes_domain.business_models.ReporteDeFraude;
import app.epm.com.reporte_fraudes_domain.reporte_fraudes.IReporteDeFraudesRepository;
import app.epm.com.reporte_fraudes_presentation.dto.ParametersReporteDeFraudesDTO;
import app.epm.com.reporte_fraudes_presentation.helpers.Mapper;
import app.epm.com.reporte_fraudes_presentation.services.IReporteDeFraudesServices;
import app.epm.com.utilities.helpers.FileManager;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.UbicationHelper;
import app.epm.com.utilities.services.ServicesArcGIS;
import app.epm.com.utilities.services.ServicesFactory;
import app.epm.com.utilities.utils.Constants;
import retrofit.RetrofitError;

public class ReporteDeFraudesRepository implements IReporteDeFraudesRepository {

    private final IReporteDeFraudesServices reporteDeFraudesServices;
    private final ICustomSharedPreferences customSharedPreferences;
    private final Gson gson;

    public ReporteDeFraudesRepository(ICustomSharedPreferences customSharedPreferences) {
        this.customSharedPreferences = customSharedPreferences;
        ServicesFactory servicesFactory = new ServicesFactory(customSharedPreferences);
        reporteDeFraudesServices = (IReporteDeFraudesServices) servicesFactory.getInstance(IReporteDeFraudesServices.class);
        gson = new GsonBuilder().disableHtmlEscaping().create();
    }

    @Override
    public List<String> getMunicipalitiesWithCoverage(ParametrosCobertura parametrosCobertura) throws RepositoryError {
        try {
            ParametrosCoberturaDTO parametrosCoberturaDTO = Mapper.convertParametrosCoberturaDomainToDTO(parametrosCobertura);
            return  reporteDeFraudesServices.getMunicipalitiesWithCoverage(parametrosCoberturaDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public Mensaje sendEmailTheRegister(ParametrossReporteDeFraudes parametrossReporteDeFraudes) throws RepositoryError {
        try {
            ParametersReporteDeFraudesDTO parametersReporteDeFraudesDTO = Mapper.
                    convertParametersReporteDeFraudesDomainToDTO(parametrossReporteDeFraudes);
            DataDTO dataDTO = Mapper.getDataDTOWithModelCrypt(gson.toJson(parametersReporteDeFraudesDTO));
            MensajeDTO mensajeDTO = reporteDeFraudesServices.sendEmailTheRegister(dataDTO);
            return Mapper.convertMensajeDTOToDomain(mensajeDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public String sendReportFraudeArcgis(ReporteDeFraude reporteDeFraude, ServicesArcGIS servicesArcGIS, Context context) {
        final ReportArcGIS reportArcGIS = Mapper.convertReportFraudeToReportArcGIS(reporteDeFraude);
        reportArcGIS.setFileAttachments(getFilesAttachments(reporteDeFraude, context));
        return servicesArcGIS.createGraphicReportArcGIS(reportArcGIS, UbicationHelper.fromJsonToPoint(reporteDeFraude.getPointSelectedSerializable()), reporteDeFraude.getTypeReport());
    }

    public ServiciosMapa getServicioKML() throws RepositoryError {
        try {
            ServicesMapDTO servicesMapDTO = reporteDeFraudesServices.getServicioKML();
            return Mapper.convertServicioKMLDTOToDomain(servicesMapDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    private ArrayList<File> getFilesAttachments(ReporteDeFraude reporteDeFraude, Context context) {
        ArrayList<File> filesAttachments = new ArrayList<>();
        FileManager filemanager = new FileManager(context);
        for (String path : reporteDeFraude.getArrayFiles()) {
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

    @Override
    public InformacionDeUbicacion getInformacionDeUbicacionWithAddress(String lat, String lon) throws RepositoryError {
        ServicesFactory servicesFactory = new ServicesFactory(customSharedPreferences, Constants.URL_GET_ADDRESS);
        IReporteDeFraudesServices fraudesServices = (IReporteDeFraudesServices) servicesFactory.getInstance(IReporteDeFraudesServices.class);
        String location = lat + "," + lon;
        try {
            AddressFromArcgisDTO addressFromArcgisDTO = fraudesServices.getInformacionDeUbicacionWithAddress(location, "pjson");
            return Mapper.convertAddressFromArcgisDTOToInformacionDeUbicacionDomain(addressFromArcgisDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }

    @Override
    public InformacionDeUbicacion getBasicInformacionDeUbicacion(String lat, String lon) throws RepositoryError {
        ServicesFactory servicesFactory = new ServicesFactory(customSharedPreferences, Constants.URL_GET_CITY);
        IReporteDeFraudesServices daniosServices = (IReporteDeFraudesServices) servicesFactory.getInstance(IReporteDeFraudesServices.class);
        String location = String.format(Constants.FORMAT_LOCATION_ARCGIS, lat, lon);
        location = "%7b".concat(location).concat("%7d");
        try {
            CityFromArcgisDTO cityFromArcgisDTO = daniosServices.getBasicInformacionDeUbicacion(location, "pjson", "esriGeometryPoint", "4326", "UBICACION", false);
            return Mapper.convertCityFromArcgisDTOToInformacionDeUbicacion(cityFromArcgisDTO);
        } catch (RetrofitError retrofitError) {
            throw Mapper.convertRetrofitErrorToRepositoryError(retrofitError);
        }
    }
}