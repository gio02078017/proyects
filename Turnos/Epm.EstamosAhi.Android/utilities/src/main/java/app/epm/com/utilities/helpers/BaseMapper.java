package app.epm.com.utilities.helpers;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.drawable.Drawable;
import android.os.Build;
import androidx.core.content.ContextCompat;
import androidx.core.graphics.drawable.DrawableCompat;
import android.util.Log;

import com.epm.app.business_models.business_models.ETipoServicio;
import com.epm.app.business_models.business_models.FieldMapa;
import com.epm.app.business_models.business_models.InformacionDeUbicacion;
import com.epm.app.business_models.business_models.Mapa;
import com.epm.app.business_models.business_models.Mensaje;
import com.epm.app.business_models.business_models.RepositoryError;
import com.epm.app.business_models.business_models.ServiciosMapa;
import com.epm.app.business_models.dto.AddressFromArcgisDTO;
import com.epm.app.business_models.dto.CityFromArcgisDTO;
import com.epm.app.business_models.dto.DataDTO;
import com.epm.app.business_models.dto.ErrorDTO;
import com.epm.app.business_models.dto.FieldDTO;
import com.epm.app.business_models.dto.MapDTO;
import com.epm.app.business_models.dto.MensajeDTO;
import com.epm.app.business_models.dto.ServicesMapDTO;

import java.io.InterruptedIOException;
import java.net.SocketTimeoutException;
import java.util.ArrayList;
import java.util.List;

import app.epm.com.utilities.utils.Constants;
import retrofit.RetrofitError;
import retrofit.client.Response;

/**
 * Created by ocadavid on 9/02/2017.
 */

public class BaseMapper {

    public static Mensaje convertMensajeDTOToDomain(MensajeDTO mensajeDTO) {
        Mensaje mensaje = new Mensaje();
        mensaje.setCode(mensajeDTO.getCodigo());
        mensaje.setText(mensajeDTO.getTexto());
        return mensaje;
    }

    public static DataDTO getDataDTOWithModelCrypt(String data) {
        DataDTO dataDTO = new DataDTO();
        try {
            CryptLib cryptLib = new CryptLib();
            String cryptKey = Constants.ENCRYPT_AES_KEY;
            String cryptIv = Constants.ENCRYPT_AES_VECTOR;
            dataDTO.setData(cryptLib.encrypt(data.replace("\\\\", "\\"), cryptKey, cryptIv));
        } catch (Exception e) {
            Log.e(e.getMessage(), "encrypt called");
        }
        return dataDTO;
    }

    public static RepositoryError convertRetrofitErrorToRepositoryError(RetrofitError retrofitError) {
        RepositoryError repositoryError;

        repositoryError = valdiateTimeOutToGetRepositoryError(retrofitError);
        if (repositoryError != null) {
            return repositoryError;
        }

        repositoryError = validateTheBodyToGetRepositoryError(retrofitError);
        if (repositoryError != null) {
            return repositoryError;
        }

        return getDefaulError();
    }

    private static RepositoryError validateTheBodyToGetRepositoryError(RetrofitError retrofitError) {
        RepositoryError repositoryError = null;
        Response response = retrofitError.getResponse();
        if (response != null) {
            int errorId = response.getStatus();
            String mensaje = Constants.DEFAUL_ERROR;
            if (errorId == Constants.UNAUTHORIZED_ERROR_CODE || errorId == Constants.NOT_FOUND_ERROR_CODE) {
                try {
                    ErrorDTO errorDTO = (ErrorDTO) retrofitError.getBodyAs(ErrorDTO.class);
                    if (errorDTO != null) {
                        mensaje = errorDTO.getMessage();
                    }
                } catch (Exception exception) {
                    Log.e("Exception", exception.toString());
                }
            }
            repositoryError = new RepositoryError(mensaje);
            repositoryError.setIdError(errorId);
        }
        return repositoryError;
    }

    private static RepositoryError valdiateTimeOutToGetRepositoryError(RetrofitError retrofitError) {
        if (retrofitError.getCause() != null && retrofitError.getCause() instanceof SocketTimeoutException
                || retrofitError.getCause() instanceof InterruptedIOException) {
            RepositoryError repositoryError = new RepositoryError(Constants.REQUEST_TIMEOUT_ERROR_MESSAGE);
            repositoryError.setIdError(Constants.DEFAUL_ERROR_CODE);
            return repositoryError;
        }
        return null;
    }

    public static RepositoryError getDefaulError() {
        RepositoryError repositoryError = new RepositoryError(Constants.DEFAUL_ERROR);
        repositoryError.setIdError(Constants.DEFAUL_ERROR_CODE);
        return repositoryError;
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

    public static InformacionDeUbicacion convertAddressFromArcgisDTOToInformacionDeUbicacionDomain(AddressFromArcgisDTO addressFromArcgisDTO) {
        if (addressFromArcgisDTO != null && addressFromArcgisDTO.getAddress() != null) {
            InformacionDeUbicacion informacionDeUbicacion = new InformacionDeUbicacion();
            informacionDeUbicacion.setDireccion(addressFromArcgisDTO.getAddress().getMatch_addr());
            informacionDeUbicacion.setMunicipio(addressFromArcgisDTO.getAddress().getCity());
            informacionDeUbicacion.setDeparatamento(addressFromArcgisDTO.getAddress().getRegion());
            informacionDeUbicacion.setPais(addressFromArcgisDTO.getAddress().getCountryCode());
            return informacionDeUbicacion;
        }
        return null;
    }

    public static InformacionDeUbicacion convertCityFromArcgisDTOToInformacionDeUbicacion(CityFromArcgisDTO cityFromArcgisDTO) {
        if (cityFromArcgisDTO == null || cityFromArcgisDTO.getFeatures() == null || cityFromArcgisDTO.getFeatures().size() == 0 || cityFromArcgisDTO.getFeatures().get(0) == null
                || cityFromArcgisDTO.getFeatures().get(0).getAttributes() == null
                || cityFromArcgisDTO.getFeatures().get(0).getAttributes().getUBICACION() == null) {
            return null;
        } else {
            String ubicacion = cityFromArcgisDTO.getFeatures().get(0).getAttributes().getUBICACION();
            String[] municipioYDepartamento = ubicacion.split(",");
            if (municipioYDepartamento.length != 2) {
                return null;
            }
            String municipio = municipioYDepartamento[0].trim();
            String departamento = municipioYDepartamento[1].trim();
            InformacionDeUbicacion informacionDeUbicacion = new InformacionDeUbicacion();
            informacionDeUbicacion.setMunicipio(municipio);
            informacionDeUbicacion.setDeparatamento(departamento);
            informacionDeUbicacion.setDireccion(municipio.concat(", ").concat(departamento));
            return informacionDeUbicacion;
        }
    }

    public static ServiciosMapa convertServicioKMLDTOToDomain(ServicesMapDTO servicesMapDTO) {
        ServiciosMapa serviciosMapa = new ServiciosMapa();
        serviciosMapa.setMapa(convertMapDTOToDomain(servicesMapDTO.getMap()));
        return serviciosMapa;
    }

    private static List<Mapa> convertMapDTOToDomain(List<MapDTO> mapDTO) {
        List<Mapa> mapList = new ArrayList<>();
        for (MapDTO map : mapDTO) {
            Mapa mapDomian = new Mapa();
            mapDomian.setNombreServcio(map.getMenu());
            mapDomian.setUrlBase(map.getBaseMaps().getLayer().getUrl());
            mapDomian.setTipoServicio(map.getBaseMaps().getLayer().getType());
            mapDomian.setLabelServicio(map.getBaseMaps().getLayer().getLabel());
            mapDomian.setUrlServicio(map.getEditFeatureLayers().getLayer().getUrl());
            mapDomian.setFieldMapa(convertFieldDTOToDomain(map.getEditFeatureLayers().getLayer().getFields().getField()));
            mapList.add(mapDomian);
        }
        return mapList;
    }

    private static List<FieldMapa> convertFieldDTOToDomain(List<FieldDTO> fieldDTO) {
        List<FieldMapa> fieldMapaList = new ArrayList<>();
        for (FieldDTO fiel : fieldDTO) {
            FieldMapa fielMapa = new FieldMapa();
            fielMapa.setId(fiel.getId());
            fieldMapaList.add(fielMapa);
        }
        return fieldMapaList;
    }

    public static Bitmap getBitmapFromVectorDrawable(Context context, int drawableId) {
        Drawable drawable = ContextCompat.getDrawable(context, drawableId);
        if (Build.VERSION.SDK_INT < Build.VERSION_CODES.LOLLIPOP) {
            drawable = (DrawableCompat.wrap(drawable)).mutate();
        }

        Bitmap bitmap = Bitmap.createBitmap(drawable.getIntrinsicWidth(),
                drawable.getIntrinsicHeight(), Bitmap.Config.ARGB_8888);
        Canvas canvas = new Canvas(bitmap);
        drawable.setBounds(0, 0, canvas.getWidth(), canvas.getHeight());
        drawable.draw(canvas);

        return bitmap;
    }

   /*
    private Bitmap convertImageToPicture(int resId) {
        Bitmap bitmap = BitmapFactory.decodeResource(getResources(), resId);
        return bitmap;
    }*/


}
