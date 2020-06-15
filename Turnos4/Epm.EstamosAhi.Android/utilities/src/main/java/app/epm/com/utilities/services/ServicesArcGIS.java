package app.epm.com.utilities.services;

import android.content.Context;
import android.util.Log;
import com.epm.app.business_models.business_models.ReportArcGIS;
import com.esri.arcgisruntime.ArcGISRuntimeEnvironment;
import com.esri.arcgisruntime.concurrent.ListenableFuture;
import com.esri.arcgisruntime.data.ArcGISFeature;
import com.esri.arcgisruntime.data.Attachment;
import com.esri.arcgisruntime.data.Feature;
import com.esri.arcgisruntime.data.FeatureEditResult;
import com.esri.arcgisruntime.data.FeatureQueryResult;
import com.esri.arcgisruntime.data.FeatureTemplate;
import com.esri.arcgisruntime.data.FeatureType;
import com.esri.arcgisruntime.data.Field;
import com.esri.arcgisruntime.data.QueryParameters;
import com.esri.arcgisruntime.data.ServiceFeatureTable;
import com.esri.arcgisruntime.geometry.Point;
import com.esri.arcgisruntime.io.RequestConfiguration;
import com.esri.arcgisruntime.layers.FeatureLayer;
import com.esri.arcgisruntime.loadable.LoadStatus;
import com.esri.arcgisruntime.mapping.ArcGISMap;
import java.io.File;
import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ExecutionException;

import app.epm.com.utilities.helpers.FileManager;
import app.epm.com.utilities.helpers.IFileManager;
import app.epm.com.utilities.helpers.TypeDanioOrFraude;
import app.epm.com.utilities.utils.Constants;

public class ServicesArcGIS{
    private ReportArcGIS reportArcGIS;
    private Integer processingArcGIS;
    private Integer objectId = null;
    private ServiceFeatureTable serviceFeatureTable;
    private FeatureLayer featureLayer;
    private List<FeatureType> types;
    private IFileManager fileManager;

    public ServicesArcGIS(String urlServiceArcGIS,ArcGISMap map, Context context) {
        RequestConfiguration request =  new RequestConfiguration();
        request.getHeaders().put(Constants.ARCGIS_HEADER_NAME, Constants.ARCGIS_HEADER_VALUE);
        serviceFeatureTable = new ServiceFeatureTable(urlServiceArcGIS);
        serviceFeatureTable.setFeatureRequestMode(ServiceFeatureTable.FeatureRequestMode.MANUAL_CACHE);
        serviceFeatureTable.setRequestConfiguration(request);
        serviceFeatureTable.addDoneLoadingListener(() ->{
            ArcGISRuntimeEnvironment.setLicense(Constants.ARCGIS_LICENSEINFO);
            if (serviceFeatureTable.getLoadStatus() == LoadStatus.LOADED) {
                List <Field> fields = serviceFeatureTable.getFields();
                if (fields != null) {
                    types = serviceFeatureTable.getFeatureTypes();
                }
            }else if(serviceFeatureTable.getLoadStatus() == LoadStatus.FAILED_TO_LOAD){
                return;
            }

        });
        featureLayer = new FeatureLayer(serviceFeatureTable);
        map.getOperationalLayers().add(featureLayer);
        fileManager = new FileManager(context);
    }

    public String createGraphicReportArcGIS(ReportArcGIS reportArcGIS, Point pointSelect, String nameTypeTemplate) {
        this.reportArcGIS = reportArcGIS;
        processingArcGIS = 1;
        addFeature(pointSelect, nameTypeTemplate);

        while (true) {
            try {
                Thread.sleep(2000);
                if (processingArcGIS == 2) {
                    break;
                }
            } catch (InterruptedException ex) {
                Log.i("Arcgis", "Interrupted");
                Thread.currentThread().interrupt();
                break;
            }

        }
        if (objectId == null) {
            Log.i("Arcgis", "null");
            return null;

        } else {
            Log.i("Arcgis", objectId.toString());
            return objectId.toString();
        }

    }

    private void addFeature(Point pointSelect, String nameTypeTemplate){

        Map<String, Object> attributes = getAttributesGraphicPoint(nameTypeTemplate);
        Feature addedFeature = serviceFeatureTable.createFeature(attributes, pointSelect);

        final ListenableFuture<Void> addFeatureFuture = serviceFeatureTable.addFeatureAsync(addedFeature);
        addFeatureFuture.addDoneListener(() ->{
            try{
                if (serviceFeatureTable instanceof ServiceFeatureTable) {
                    applyEditsFuture();
                }
            }catch (Exception e) {
                Log.i("Arcgis", "Fue llamado el catch externo metodo addFeature" + e.getMessage());
            }
        });

    }

    public void applyEditsFuture(){
        final ListenableFuture<List<FeatureEditResult>> applyEditsFuture = serviceFeatureTable.applyEditsAsync();
        applyEditsFuture.addDoneListener(() -> {
            try {
                final List<FeatureEditResult> featureEditResults = applyEditsFuture.get();
                if(featureEditResults != null && !featureEditResults.isEmpty()){
                    if (!featureEditResults.get(0).hasCompletedWithErrors()) {
                        Log.i("Arcgis", "fue exitosa la creación del punto" + new BigDecimal(featureEditResults.get(0).getObjectId()).intValueExact() + "" + processingArcGIS);
                        objectId = new BigDecimal(featureEditResults.get(0).getObjectId()).intValueExact();
                        processingArcGIS = 1;
                        //applyEditsFuture.cancel(true);
                        Log.i("Arcgis", "fue exitosa la creación del punto" + processingArcGIS);
                        addAttachmentGraphicReportArcGIS();
                    }
                }else {
                    Log.i("Arcgis", "Fue llamado para crear el punto else " + processingArcGIS);
                    //TODO Cambio nuevo para validar proceso de envío Arcgis
                    processingArcGIS = 2;
                    Log.i("Arcgis", "Fue llamado para crear el punto else " + processingArcGIS);
                }

            }catch (InterruptedException e){
                Thread.currentThread().interrupt();
                Log.i("Arcgis", "Fue llamado el catch interno metodo addFeature" + e.getMessage());
            }catch (ExecutionException ex){
                Log.i("Arcgis", "Fue llamado el catch interno metodo addFeature" + ex.getMessage());
            }
        });
    }

    private Map<String, Object> getAttributesGraphicPoint(String nameTypeTemplate) {
        Map<String, Object> attributesGraphicPoint = new HashMap<String, Object>();
        attributesGraphicPoint.put(Constants.ARCGIS_DESCRIPCION, this.reportArcGIS.getDescription());
        attributesGraphicPoint.put(Constants.ARCGIS_TELEFONO, this.reportArcGIS.getTelephone());
        attributesGraphicPoint.put(Constants.ARCGIS_DIRECCION, this.reportArcGIS.getAddress());
        attributesGraphicPoint.put(Constants.ARCGIS_NOMBRE, this.reportArcGIS.getName());
        attributesGraphicPoint.put(Constants.ARCGIS_CORREO, this.reportArcGIS.getEmail());
        attributesGraphicPoint.put(Constants.ARCGIS_LUGAR, this.reportArcGIS.getLocationReference());
        if (this.reportArcGIS.getHour() != null) {
            attributesGraphicPoint.put(Constants.ARCGIS_HORARIO, this.reportArcGIS.getHour());
        }
        attributesGraphicPoint.put(Constants.ARCGIS_SERVICIO_TIPO, Integer.parseInt(this.reportArcGIS.getIdType()));
        attributesGraphicPoint.put(Constants.ARCGIS_SERVICIO_AFECTADO,  this.reportArcGIS.getAffectedService());
        attributesGraphicPoint.put(Constants.ARCGIS_ESTADO, this.reportArcGIS.getState());
        return attributesGraphicPoint;
    }


    private void addAttachmentGraphicReportArcGIS() {
        if (this.reportArcGIS.getFileAttachments() == null || this.reportArcGIS.getFileAttachments().size() == 0) {
            Log.i("Arcgis", "No se adjunto evidencias" + processingArcGIS);
            processingArcGIS = 2;
            Log.i("Arcgis", "No se adjunto evidencias" + processingArcGIS);
        } else {
            if(this.reportArcGIS.getFileAttachments().size() > 0){
                fetchAttachmentsFromServer(objectId.toString());
            }
        }
    }

    private void fetchAttachmentsFromServer(String objectID) {

        QueryParameters query = new QueryParameters();
        query.setWhereClause("OBJECTID = " + objectID);

        final ListenableFuture<FeatureQueryResult> resultQuery = serviceFeatureTable.queryFeaturesAsync(query);
        resultQuery.addDoneListener(() -> {
            try {
                FeatureQueryResult result = resultQuery.get();
                Feature feature = result.iterator().next();
                ArcGISFeature mSelectedArcGISFeature = (ArcGISFeature) feature;
                fetchAttachments(mSelectedArcGISFeature);
            } catch (Exception e) {
                Log.i("Arcgis", "error query catch externo metodo fetchAttachmentsFromServer " + e.getMessage());
            }
        });
    }

    private void fetchAttachments(ArcGISFeature mSelectedArcGISFeature){
        final ListenableFuture<List<Attachment>> attachmentResults = mSelectedArcGISFeature.fetchAttachmentsAsync();
        attachmentResults.addDoneListener(() -> {
            try {
                 sendAttachment(mSelectedArcGISFeature);
            } catch (Exception e) {
                Log.i("Arcgis", "error query catch interno metodo fetchAttachmentsFromServer " + e.getMessage());
            }
        });

    }

    private void sendAttachment(ArcGISFeature mSelectedArcGISFeature){

        for (int element = 0; element < reportArcGIS.getFileAttachments().size(); element++) {
            File file = reportArcGIS.getFileAttachments().get(element);
            addAttachmentGraphicReportArcGISCallBack(file, mSelectedArcGISFeature, (element == reportArcGIS.getFileAttachments().size() - 1) ? true : false);
        }
    }

    private void addAttachmentGraphicReportArcGISCallBack(File file,ArcGISFeature mSelectedArcGISFeature, boolean lastFile ){

        final ListenableFuture<Attachment> resultAddAttachment = mSelectedArcGISFeature.addAttachmentAsync(fileManager.fileToBytes(file), fileManager.getExtensionFile(file.getPath()), file.getName());

        resultAddAttachment.addDoneListener(() -> {

                updateFeature(mSelectedArcGISFeature, lastFile);
        });
    }

    private void updateFeature(ArcGISFeature mSelectedArcGISFeature, boolean lastFile){

        final ListenableFuture<Void> resultUpdateFeature = serviceFeatureTable.updateFeatureAsync(mSelectedArcGISFeature);
        resultUpdateFeature.addDoneListener(() ->{
                if(lastFile){
                    applyServerEdits(lastFile);
                }else{
                    processingArcGIS = 1;
                }
        });
    }

    private void applyServerEdits(boolean lastFile) {

        final ListenableFuture<List<FeatureEditResult>> updatedServerResult = serviceFeatureTable.applyEditsAsync();
        updatedServerResult.addDoneListener(() -> {
            try {
                List<FeatureEditResult> edits = updatedServerResult.get();
                if (edits.size() > 0) {
                    if (!edits.get(0).hasCompletedWithErrors()) {
                        Log.i("Arcgis", "set atributos del reporte" + processingArcGIS);
                        processingArcGIS = lastFile ? 2 : 1;
                        Log.i("Arcgis", "set atributos del reporte" + processingArcGIS);
                    } else {
                        Log.i("Arcgis", "error al set atributos del reporte" + processingArcGIS);
                        processingArcGIS = 2;
                        Log.i("Arcgis", "error al set atributos del reporte" + processingArcGIS);

                    }
                } else {
                    Log.i("Arcgis", "error al set atributos del reporte" + processingArcGIS);
                    processingArcGIS = 2;
                    Log.i("Arcgis", "error al set atributos del reporte" + processingArcGIS);
                }
            } catch (Exception e) {
                Log.i("Arcgis", "error al set catch interno atributos del reporte metodo applyServerEdits " + e.getMessage());
            }
        });
    }

    public ArrayList<TypeDanioOrFraude> getListNameTypeDanio(boolean hasCoberturEnergia, boolean hasCoberturaIluminaria) {

        ArrayList<TypeDanioOrFraude> listNameTypeDanio = new ArrayList<>();
        if (serviceFeatureTable.getFields() != null) {
            if (types != null && types.size() > 0){
                for (FeatureType featureType : types) {
                    if (verifyFeatureTypeDanios(hasCoberturEnergia, hasCoberturaIluminaria, featureType))
                        for (FeatureTemplate featureTemplate : featureType.getTemplates()) {
                            TypeDanioOrFraude typeDanioOrFraude = new TypeDanioOrFraude();
                            typeDanioOrFraude.setId(featureType.getId().toString());
                            typeDanioOrFraude.setNameType(featureTemplate.getName());
                            listNameTypeDanio.add(typeDanioOrFraude);
                        }
                }
            }
        }
        return listNameTypeDanio;
    }

    public ArrayList<TypeDanioOrFraude> getListNameTypeFraude() {
        ArrayList<TypeDanioOrFraude> listNameTypeDanio = new ArrayList<>();
        if (serviceFeatureTable.getFields() != null) {
            if (types != null && types.size() > 0) {
                for (FeatureType featureType : types) {
                    for (FeatureTemplate featureTemplate : featureType.getTemplates()) {
                        TypeDanioOrFraude typeDanioOrFraude = new TypeDanioOrFraude();
                        typeDanioOrFraude.setId(featureType.getId().toString());
                        typeDanioOrFraude.setNameType(featureTemplate.getName());
                        listNameTypeDanio.add(typeDanioOrFraude);
                    }
                }
            }
        }

        return listNameTypeDanio;
    }

    private boolean verifyFeatureTypeDanios(boolean hasCoberturEnergia, boolean hasCoberturaIluminaria, FeatureType featureType) {
        if (!hasCoberturEnergia && hasCoberturaIluminaria && featureType.getName().charAt(0) == 'A') {
            return true;
        }

        if (hasCoberturEnergia && !hasCoberturaIluminaria && featureType.getName().charAt(0) == 'E') {
            return true;
        }

        if (!hasCoberturEnergia && !hasCoberturaIluminaria) {
            return true;
        }

        if (hasCoberturEnergia && hasCoberturaIluminaria) {
            return true;
        }

        return false;
    }
}