package app.epm.com.utilities.utils;

import java.util.Locale;
import java.util.StringTokenizer;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public final class Constants {

    public static final boolean IS_DEBUG = false;
    public static final boolean IS_AUTOMATION = false;

    /**
     * Numbers
     */
    public static final int ZERO = 0;
    public static final int ONE = 1;
    public static final int TWO = 2;
    public static final int THREE = 3;
    public static final int FOUR = 4;
    public static final int SEVEN = 7;
    public static final int EIGHT=8;
    public static final int TEN = 10;
    public static final int TWELVE = 12;
    public static final int ONE_THOUSAND=1000;
    /**
     * URLs
     */
    public static final String URL_PRODUCCION_EPM = "https://appmovil.epm.com.co/WSAppMovil/V2_0/api";
    public static final String URL_DLLO_EPM = "https://appmovildllo.epm.com.co/WSAppMovil/V2_0/api";
    public static final String URL_DLLO_EPM_MOCK = "https://apim-epm-dev.azure-api.net/ServicioAlCliente/api";
    public static final String URL_UAT_EPM = "https://appmoviluat.epm.com.co/WSAppMovil/V2_0/api";
    public static final String URL_GET_ADDRESS = "http://geocode.arcgis.com/arcgis/rest/services/World";
    public static final String URL_GET_CITY = "http://services1.arcgis.com/lHSB5M3vxFT82S7P/arcgis/rest/services";
    public static final String URL_UAT_ARCGIS = "http://grupoepm.maps.arcgis.com";
    public static final String URL_VIRTUALIZACION = "http://svazcadtstv.cloudapp.net:8001/WSAppMovil";

    /**
     * URLs App
     */
    public static final String URL_TERMS_AND_CONDITIONS = "http://www.epm.com.co/site/portals/aplicaciones/app/terminos_y_condiciones/index.html";
    public static final String URL_APP_EPM_SHARE = "http://onelink.to/9arqwt";

    /**
     * Version App
     */
    public static final String URL_APP_PLAY_STORE = "https://play.google.com/store/apps/details?id=com.epm.app";
    public static final String URL_MARKET_APP_PLAY_STORE = "market://details?id=com.epm.app";
    public static final String DIV_HTML_APP_PLAY_STORE = "div[itemprop=softwareVersion]";

    /**
     * Qualify App
     */
    public static final String DATE_QUALIFY_APP = "dateQualifyApp";
    public static final String OPEN_QUALIFY_APP = "openQualifyApp";
    public static final String SHOW_ALERT_RATE = "openQualifyApp";

    /**
     * Fonts
     */
    public static final String PATH_NORMAL_FONT = "fonts/VAG-Rounded-lt-normal.ttf";
    public static final String PATH_BOLD_FONT = "fonts/VAG-Rounded-BT.ttf";

    /**
     * Keys
     */
    public static final String UTF_EIGHT = "UTF-8";
    public static final String EXCEPTION_STRING = "Exception";
    public static final String EXCEPTION_FILE_TO_BYTE = "ExceptionFromFileToByte";
    public static final String MY_PREFERENCES = "myPreferences";
    public static final String USUARIO = "usuario";
    public static final String TOKEN = "authToken";
    public static final String INVITADO = "invitado";
    public static final String LOGIN_BOOLEAN = "false";
    public static final String TRUE = "true";
    public static final String FALSE = "false";
    public static final String LOGUEADO = "logueado";
    public static final String EPM = "epm";
    public static final String tutorialPreferenceAlerts = "validateTutorialAlertas";
    public static final String SUSCRIPTION_ALERTAS = "subscribed";
    public static final String NUMBER_NOTIFICATIONS = "notifications";
    public static final String SHOW_BELL = "showBell";
    public static final String ASSIGNED_TRUN = "assignedTurn";

    /**
     * Constants
     */
    public static final String EMPTY_STRING = "";
    public static final int EMPTY_INT = -1;
    public static final String MENU_ACTION = "menuAction";
    public static final String MENU_ACTION_REGISTER = "menuActionRegister";
    public static final String MENU_ACTION_SIGN_IN = "menuActionSignIn";
    public static final String SHOW_LOGIN = "showLogin";
    public static final String CLOSE_SESION = "closeSesion";
    public static final String ID_USER = "defaultUser";

    /**
     * Numbers
     */
    public static final long THIRTY = 30;
    public static final long SIXTY = 60;
    public static final String REGULAR_EXPRESSION_CORRECT_EMAIL = "^(([\\w-]+\\.)+[\\w-]+|([a-zA-Z]{1}|[\\w-]{2,}))@"
            + "((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?"
            + "[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\."
            + "([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?"
            + "[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + "([a-zA-Z]+[\\w-]+\\.)+[a-zA-Z]{2,4})$";
    public static final String FORMAT_TO_SAVE_GENERAL_LIST = "%1$s/.%2$s/.%3$s";
    public static final String FORMAT_LOCATION_ARCGIS = "x:%1$s,y:%2$s";
    public static final int REQUEST_CODE_SHARE = 101;

    /**
     * Error Messages
     */
    public static final String NULL_PARAMETERS = "No se permiten parámetros nulos";
    public static final String EMPTY_PARAMETERS = "No se permiten parámetros vacíos";
    public static final String INVALID_EMAIL = "correo invalido";
    public static final String NOT_FOUND_EMAIL = "correo no encontrado";
    public static final String DEFAUL_ERROR = "Ha ocurrido un error, inténtalo nuevamente.";
    public static final int DEFAUL_ERROR_CODE = 0;
    public static final String REQUEST_TIMEOUT_ERROR_MESSAGE = "La solicitud está tardando demasiado. Por favor inténtalo nuevamente.";
    public static final int UNAUTHORIZED_ERROR_CODE = 401;
    public static final int NOT_FOUND_ERROR_CODE = 404;
    public static final int FORBIDEN_ERROR_CODE = 403;

    /**
     * Messages
     */
    public static final String MESSAGE_SHARE = "Quiero compartir contigo la aplicación EPM estamos ahí, podría ser una herramienta útil para ti.";

    /**
     * Encrypt
     */
    public static final String ENCRYPT_AES_KEY = "bREGsVfovp03uMZe9MXTvOHiN399Na1o";
    public static final String ENCRYPT_AES_VECTOR = "9C479S39eu5PSD34";
    public static final String TIPOS_DOCUMENTO_LIST = "tiposDocumentoList";
    public static final String TIPOS_PERSONA_LIST = "tiposPersonaList";
    public static final String TIPOS_VIVIENDA_LIST = "tiposViviendaList";
    public static final String GENEROS_LIST = "generosList";
    public static final String CURRENT_DATE_SAVED_LIST = "currentDateSavedList";
    public static final String APP_VERSION = "appVersion";

    /**
     * Class Names
     */
    public static final String LANDING_CLASS = "landingClass";
    public static final String REGISTER_LOGIN_CLASS = "registerLoginActivity";
    public static final String CHANGE_PSWORD_CLASS = "changePasswordActivity";
    public static final String MY_PROFILE_CLASS = "myProfileActivity";
    public static final String FACTURA_CLASS = "facturasConsultadasActivity";
    public static final String SPLASH_CLASS = "splashAppActivity";
    public static final String SHIFT_INFORMATION_ACTIVITY = "ShiftInformationActivity";
    public static final String GAS_STATIONS = "EstacionesDeGasActivity";
    public static final String HIDROITUANGO_ALERS = "SplashActivity";
    public static final String ATTENDED_SHIFT = "CustomerServiceMenuOptionActivity";

    public static final String EVENTS_ACTIVITY = "EventosActivity";
    public static final String FRAUD_REPORT = "ServicesDeFraudesActivity";
    public static final String TRANSPARENT_CONTACT = "HomeContactoTransparenteActivity";
    public static final String NEWS_ACTIVITY = "NoticiasActivity";
    public static final String ATENTION_LINES_ACTIVITY = "LineasDeAtencionActivity";
    public static final String CUSTOMER_SERVICE = "CustomerServiceMenuOptionActivity";
    public static final String DAMAGE_REPORT = "ServiciosDanioActivity";
    public static final String CHECK_INVOCE = "ConsultFacturaActivity";
    public static final String ATENTION_CHANNEL = "ChannelsOfAttentionActivity";

    public static final String NOTIFICATION_CENTER_CLASS = "notificationCenterActivity";
    public static final long POST_DELAY_SPLASH_TIME = 3000;

    /**
     * Actions result intent.
     */
    public static final int DEFAUL_REQUEST_CODE = 1;
    public static final int ACTION_START = 2;
    public static final int LOGIN = 3;
    public static final int ACTION_LOG_OUT = 4;
    public static final int FACTURA = 5;
    public static final int SESSION = 6;
    public static final int MIS_FACTURAS = 7;
    public static final int RATE_APP = 8;
    public static final int SERVICES_DANIO = 9;
    public static final int ACTION_TO_EXECUTE_FROM_ONE_SIGNAL = 10;
    public static final int DESCRIBE_DANIO = 11;
    public static final int ATTACH = 12;
    public static final int FINISH = 13;
    public static final int GALLERY_JELLY_BEAN_AND_LESS = 14;
    public static final int GALLERY_KITKAT_AND_HIGHER = 15;
    public static final int CAMERA_CAPTURE = 16;
    public static final int REGISTER_LOGIN_BACK = 17;
    public static final int SERVICIOS_FRAUDE = 19;
    public static final int ALERT = 20;
    public static final int UNAUTHORIZED = 21;
    public static final int ACTION_TO_EXECUTE_NOTICIAS = 23;
    public static final int ACTION_TO_EXECUTE_LINEAS = 24;
    public static final int ACTION_TO_EXECUTE_SERVICIO_AL_CLIENTE = 25;
    public static final int ACTION_TO_EXECUTE_REPORTE_FRAUDES = 26;
    public static final int ACTION_TO_EXECUTE_CONTACTO_TRANSPARENTE = 28;
    public static final int ACTION_TO_EXECUTE_DANIOS = 29;
    public static final int ACTION_TO_EVENTOS = 30;
    public static final int ACTION_TO_ESTACIONES_DE_GAS = 31;
    public static final int ACTION_TO_ALERTAS_HIDROITUANGO = 32;
    public static final int ACTION_TO_RED_ALERTAS_HIDROITUANGO = 33;
    public static final int RECOVERY_SUBSCRIPTION = 34;
    public static final int ACTION_TO_TURN_ON_GPS = 35;
    public static final int BACKPRESSED = 36;
    public static final int ACTION_TO_TURN_ON_PERMISSION = 37;
    public static final int ACTION_TO_ATTENDED_SHIFT = 38;
    public static final int ACTION_TO_TURN_ADVANCE = 39;

    /**
     * Factura
     */
    public static final int CODIGO_FACTURA_NO_EXISTENTE = 18;
    public static final int CODIGO_OPERACION = 2;
    public static final String POSICION_LISTA_MIS_FACTURAS = "posicionListaMisFacturas";
    public static final String LISTA_MIS_FACTURAS = "listaMisFacturas";
    public static final String LISTA_MIS_FACTURAS_POR_PAGO = "listaMisFacturas";
    public static final String LISTA_HISTORICO_FACTURAS = "listaHistoricoFacturas";
    public static final String FORMATYMD = "yyyy-MM-dd";
    public static final String FORMATMD = "MMMM dd";
    public static final String FORMATMY = "MMMM / yyyy";
    public static final String FORMATDMY = "dd/MM/yyyy";
    public static final String FORMAT_YMD_HMS = "yyyy-MM-dd'T'HH:mm:ss";
    public static final String FORMAT_DMY_HMS = "dd-MM-yyyy HH:mm:ss";
    public static final String FORMAT_DATE_YMDHMS = "yyyy-MM-dd-hh-mm-ss";
    public static final String NAME_CONTRATO = "nameContrato";
    public static final String NUMBER_FACTURA = "numberFactura";
    public static final String NUMBER_CONTRATO = "numberContrato";
    public static final String ENTITYFINANCIAL = "entityFinancial";
    public static final String INFORMACION_PSE = "informacionPSE";
    public static final String NOMBRE_ENTIDAD_FINANCIERA = "entidadFinanciera";
    public static final String VALOR_TOTAL_PAGAR = "valorTotalPagar";
    public static final String ID_ENTIDAD_FINANCIERA = "idEntidadFinanciera";
    public static final String TRANSACCION_PSE_RESPONSE = "transaccionPSEResponse";
    public static final String EMP_FACTURA = "epmfactura";
    public static final String ARG_ANIO = "anio";
    public static final String ARG_MES = "mes";
    public static final String ARG_FECHA_DE_PAGO_RESULTADO = "fechaDePagoResultado";
    public static final String ARG_PAGADA_EN_RESULTADO = "pagadaEnResultado";
    public static final String ARG_VALOR_PAGADO_RESULTADO = "valorPagadoResultado";
    public static final String ARG_FACTURA_PAGADA = "facturaPagadaResultado";
    public static final String ARG_URL_PDF = "urlPDF";
    public static final int OPERACION_ACTUALIZAR = 2;
    public static final int OPERACION_ELIMINAR = 3;
    public static final int CONTRATO_INSCRITO = 74;
    public static final String CUSTOMER_ID_ARCGIS = "oSPc8ziJY5htmmW2";
    public static final int REQUEST_CODE_PERMISSION = 2;
    public static final int REQUEST_CODE_GPS_SETTINGS = 3;
    public static final String REPORT_DANIOS = "reportDanios";
    public static final int MIN_LENGTH_PHONE = 7;
    public static final String HISTORICO_MESSAGE = "No existe histórico de facturas disponible para el contrato seleccionado.";
    public static final String DETALLE_MESSAGE = "No existe detalle de consumo disponible para el contrato seleccionado.";

    /**
     * Detalle de factura.
     */
    public static final String DETALLE_FACTURA_ACUEDUCTO = "ACUEDUCTO";
    public static final String DETALLE_FACTURA_ALCANTARILLADO = "ALCANTARILLADO";
    public static final String DETALLE_FACTURA_ENERGIA = "ENERGÍA";
    public static final String DETALLE_FACTURA_GAS = "GAS";
    public static final String DETALLE_FACTURA_OTROS = "OTROS";


    /**
     * Adjuntos
     */
    public static final int AUDIO_CAPTURE = 23;
    public static final int MAX_DURATION = 30500;
    public static final int FACTOR_CONVERSION_MB = 1048576;
    public static final int FACTOR_CONVERSION_KB = 1024;
    public static final String FORMAT_DATE_FILE = "yyyyMMdd_HHmmss";
    public static final String ARRAY_WITH_PATH = "arrayAttachFiles";
    public static final String SUFFIX_FILE_IMAGE = ".jpg";
    public static final String SUFFIX_FILE_AUDIO = ".mp3";
    public static final String PREFIX_FILE_IMAGE = "JPEG_";
    public static final String PREFIX_FILE_AUDIO = "AUDIORECORD_";
    public static final String DATA_CAMERA = "data";
    public static final String PATH_ATTACH_AUDIO = "pathAttachAudio";
    public static final String FORMAT_MB = "0.00";
    public static final String MEGABYTE = " MB";
    public static final String KILOBYTE = " KB";

    /**
     * Danios
     */
    public static final String SERVICES = "Services";
    public static final String AGUA = "Aguas";
    public static final String ENERGIA = "Energia";
    public static final String ENERGIAS = "Energía";
    public static final String GAS = "Gas";
    public static final String TIPO_SERVICIO = "tipoServicio";
    public static final String DATA_ERROR_MESSAGE = "Ha ocurrido un problema al tratar de cargar el servicio sobre el cual se va a reportar el Daño. Por favor intente de nuevo.";
    public static final String LIST_TYPE_DANIO = "listsTypeDanio";
    public static final String INFORMACION_UBICACION = "informacionUbicacion";
    public static final String ADDRESS = "address";
    public static final String COUNTRYCODE = "COL";

    /**
     * Formatos de mensajes de cobertura.
     */
    public static final String FORMAT_MESSAGE_WITHOUT_COBERTURA_AND_ON_ANTIOQUIA = "¡Ten en cuenta! EPM no es el operador del servicio de %1$s en %2$s, %3$s. Consulta en tu alcaldía cuál es el operador.";
    public static final String FORMAT_MESSAGE_WITHOUT_COBERTURA_AND_NOT_ON_ANTIOQUIA = "¡Ten en cuenta! Este servicio no está disponible en %1$s, %2$s. Comunícate con la línea de atención al cliente 01 8000 415 115 para realizar tu reporte.";
    public static final String FORMAT_MESSAGE_WITHOUT_COBERTURA_ILUMINARIA_AND_ON_ANTIOQUIA = "¡Ten en cuenta! EPM no es el operador del servicio de Alumbrado Público en %1$s, %2$s. Consulta en tu alcaldía cuál es el operador.";
    public static final String FORMAT_MESSAGE_WITHOUT_COBERTURA_ENERGIA = "¡Ten en cuenta! En %1$s, %2$s. Solo puedes reportar daños de Alumbrado Público.";
    public static final String FORMAT_MESSAGE_WITHOUT_COBERTURA_ILUMINARIA_AND_NOT_ON_ANTIOQUIA = "¡Ten en cuenta! En %1$s, %2$s. No puedes reportar daños de Alumbrado Público. Comunícate con la línea de atención al cliente 01 8000 415 115 para realizar tu reporte.";
    public static final String ANTIOQUIA = "ANTIOQUIA";
    public static final String ACTION_TO_EXECUTE = "accionAEjecutar";
    public static final String TYPE_FACTURA_NOTIFICATION = "factura";
    public static final String TYPE_NOTIFICATION = "tipeNotification";
    public static final String NOTIFICATION = "notification";

    /**
     * Contacto transparente.
     */
    public static final String STATE_ANONYMOUS = "anonymous";
    public static final String LISTA_GRUPO_INTERES = "listaGrupoInteres";
    public static final int MAX_LENGTH_PHONE = 50;
    public static final int MAX_LENGTH_PHONE_FRAUDE = 10;
    public static final String CONTACTO = "contacto";
    public static final String INCIDENT = "incident";
    public static final String ATTACHLIST = "AttachList";
    public static final int ATTACHACTIVITY = 1;
    public static final String CODE_INCIDENT = "codeIncident";
    public static final String TITLE_FRAGMENT_STATUS = "ESTADO";
    public static final String TITLE_FRAGMENT_DATA = "DETALLE";
    public static final String MINIMUM_DATE = "01/01/1950";
    public static final String FORMAT_DATE = "MM/dd/yyyy";
    public static final String FORMAT_DATE_ATTACH = "yyyyMMdd_HHmmssSSS";
    public static final String MP3 = "mp3";
    public static final String AUDIO = "AudioContactoTransparente";
    public static final String IMAGE = "ImagenContactoTransparente";
    public static final String NOT_FOUND = "No se encontró una denuncia";
    public static final String ALERT_HOME = "alertHome";

    /**
     * Constantes para google analytics.
     */
    public static final String LANDING_INVITADO = "dashboard_guest";
    public static final String LANDING_REGISTRADO = "dashboard_loggedUser";
    public static final String FACTURA_INVITADO = "factura_guest";
    public static final String FACTURA_REGISTRADO = "factura_loggedUser";
    public static final String DANIOS_INVITADO = "reporteDanos_guest";
    public static final String DANIOS_REGISTRADO = "reporteDanos_loggedUser";
    public static final String CONTACTO_TRASPARENTE_INVITADO = "contactoTransparente_guest";
    public static final String CONTACTO_TRASPARENTE_REGISTRADO = "contactoTransparente_loggedUser";
    public static final String GOOGLE_ANALYTICS_CATEGORY = "Action";
    public static final String ONE_SIGNAL_ID = "oneSignalId";
    public static final String ONE_SIGNAL_IS_EMPTY = "0000-0000-0000-0000-0000";

    /**
     * Base noticias y eventos
     */
    public static final String EVENTOS = "eventos";
    public static final String NOTICIAS = "noticias";
    public static final String NEWS_EVENTS = "newsEvents";
    public static final String IMAGE_NEWS_EVENTS = "imageNewsEvents";

    /**
     * Constantes ArcGIS.
     */
    public static final String ARCGIS_HEADER_NAME = "referer";
    public static final String ARCGIS_HEADER_VALUE = "appepmmobile";
    public static final String ARCGIS_DESCRIPCION = "Descripcion";
    public static final String ARCGIS_TELEFONO = "Telefono";
    public static final String ARCGIS_DIRECCION = "Direccion";
    public static final String ARCGIS_NOMBRE = "Nombre";
    public static final String ARCGIS_CORREO = "Correo";
    public static final String ARCGIS_LUGAR = "Lugar";
    public static final String ARCGIS_HORARIO = "Horario";
    public static final String ARCGIS_ESTADO = "Estado";
    public static final String ARCGIS_SERVICIO_AFECTADO = "Servicio";
    public static final String ARCGIS_SERVICIO_TIPO = "Tipo";
    public static final String ARCGIS_LICENSEINFO = "runtimelite,1000,rud6628759669,none,FA0RJAY3FPB9NTNE9189*-";

    /**
     * Oficinas de atención y estaciones de gas
     */
    public static final String CUSTOMERID = "oSPc8ziJY5htmmW2";
    public static final String URL_BASE_MAP = "http://grupoepm.maps.arcgis.com";
    public static final String URL_MAP_OFICINAS_DE_ATENCION = "http://grupoepm.maps.arcgis.com/home/webmap/viewer.html?webmap=d8f905de1ff44db8a87623a0642ad05e";
    public static final String URL_MAP_ALERTAS = "http://grupoepm.maps.arcgis.com/home/webmap/viewer.html?webmap=d8f905de1ff44db8a87623a0642ad05e";
    //public static final String ID_MAP_OFICINAS_DE_ATENCION = "d8f905de1ff44db8a87623a0642ad05e";

    public static final String ID_MAP_OFICINAS_DE_ATENCION = "d5c36ac8baf8413e953e8e67cdd2b33e";
    //public static final String ID_MAP_OFICINAS_DE_ATENCION = "d5c36ac8baf8413e953e8e67cdd2b33e";
    public static final String URL_MAP_ESTACIONES_DE_GAS = "http://grupoepm.maps.arcgis.com/home/webmap/viewer.html?webmap=a8a9182c328d4317b730f5b76a58a518";
    public static final String ID_MAP_ESTACIONES_DE_GAS = "a8a9182c328d4317b730f5b76a58a518";
    public static final String ID_MAP_ALERTA_ROJA = "13d91b3bfb7b44619f1aa835a94d61d8";
    public static final String ESTACIONES_DE_GAS = "estacionesDeGas";
    public static final String ALERTA_ROJA = "alertaroja";
    public static final String OFICINAS_DE_ATENCION = "oficinasDeAtencion";
    public static final String TITLE_OFICINAS_DE_ATENCION = "Nombre";
    public static final String ADDRESS_OFICINAS_DE_ATENCION = "Direccion";
    public static final String IMAGE_OFICINAS_DE_ATENCION = "Imagen";
    public static final String APPLY_EASY_POINT = "AplicaPuntoFacil";
    public static final String SCHEDULE_OFICINAS_DE_ATENCION = "Horario";
    public static final String ID_OFICINAS_DE_ATENCION = "OBJECTID";
    public static final String ID_SENTRY_OFICINAS_DE_ATENCION = "GlobalID";
    public static final String LATITUD_OFICINAS_DE_ATENCION = "Y";
    public static final String LONGITUD_OFICINAS_DE_ATENCION = "X";
    public static final String TITLE_ESTACIONES_DE_GAS = "NOMBRE_OPE";
    public static final String TITLE_ALERTA_ROJA = "NOMBRE_PUN";
    public static final String ADDRESS_ESTACIONES_DE_GAS = "UBICACION";
    public static final String IMAGE_ESTACIONES_DE_GAS = "Foto";
    public static final String IT_IS_EASY_POINT = "AplicaPuntoFacil";
    public static final String kEY_REPLACE_TITLE_EASY_POINT_START = "ácil ";
    public static final String kEY_REPLACE_TITLE_EASY_POINT_END = "ácil \n";
    /**
     * Fraudes
     */
    public static final String REPORT_FRAUDES = "reportFraudes";
    public static final String LIST_MUNICIPIES = "listMunicipies";
    public static final int REPORTE_FRAUDE = 1;
    public static final String LIST_TYPE_FRAUDE = "listTypeFraude";
    public static final String ADDRESS_FRAUDE = "addressFraude";
    public static final String DATA_ERROR_MESSAGE_FRAUDE = "Ha ocurrido un problema al tratar de cargar el servicio sobre el cual se va a reportar el Fraude. Por favor intente de nuevo.";
    public static final String REPORT_FRAUDE = "reportFraude";

    /**
     * Meses del año
     */
    public static final String JANUARY = "January";
    public static final String FEBRUARY = "February";
    public static final String MARCH = "March";
    public static final String APRIL = "April";
    public static final String MAY = "May";
    public static final String JUNE = "June";
    public static final String JULY = "July";
    public static final String AUGUST = "August";
    public static final String SEPTEMBER = "September";
    public static final String OCTOBER = "October";
    public static final String NOVEMBER = "November";
    public static final String DECEMBER = "December";
    public static final String ENERO = "Enero";
    public static final String FEBRERO = "Febrero";
    public static final String MARZO = "Marzo";
    public static final String ABRIL = "Abril";
    public static final String MAYO = "Mayo";
    public static final String JUNIO = "Junio";
    public static final String JULIO = "Julio";
    public static final String AGOSTO = "Agosto";
    public static final String SEPTIEMBRE = "Septiembre";
    public static final String OCTUBRE = "Octubre";
    public static final String NOVIEMBRE = "Noviembre";
    public static final String DICIEMBRE = "Diciembre";


    /**
     * Espacio promocional
     */
    public static final String DATE_WITHOUT_HOURS = "dateWithoutHours";
    public static final String OPEN_PROMOTIONAL_SPACE = "openPromotionalSpace";
    public static final String IMAGE_PROMOTIONAL_SPACE = "imagenEspacioPromocional";
    public static final String DATE_ACTIVITY_PROMOTIONAL_SPACE = "dateActivityPromotionalSpace";
    public static final String DATE_PROMOTIONAL_SPACE = "datePromotionalSpace";
    public static final String NAME_MODULE_PROMOTIONAL_SPACE = "nameModulePromotionalSpace";
    public static final int NOT_MODULE = 0;
    public static final int MODULE_CODE_NOTICIAS = 1;
    public static final int MODULE_CODE_LINEAS_DE_ATENCION = 2;
    public static final int MODULE_CODE_OFICINAS = 3;
    public static final int MODULE_CODE_REPORTE_FRAUDES = 4;
    public static final int MODULE_CODE_FACTURA = 5;
    public static final int MODULE_CODE_CONTACTO_TRANSPARENTE = 6;
    public static final int MODULE_CODE_REPORTE_DANIOS = 7;
    public static final int MODULE_CODE_EVENTOS = 8;
    public static final int MODULE_CODE_ESTACIONES_DE_GAS = 9;
    public static final String EXIST_NOTIFICATION = "existNotification";

    /**
     * Name Modules
     */
    public static final String MODULE_NOTICIAS = "noticias";
    public static final String MODULE_LINEAS_DE_ATENCION = "lineasDeAtencion";
    public static final String MODULE_SERVICIO_AL_CLIENTE = "servicioAlCliente";
    public static final String MODULE_REPORTE_FRAUDES = "reporteFraudes";
    public static final String MODULE_FACTURA = "facturaWeb";
    public static final String MODULE_TURNO_ABANDONADO = "turnoAbandonado";
    public static final String MODULE_TURNO_ATENDIDO = "turnoAtendido";
    public static final String MODULE_TURNO_AVANCE = "avanceTurno";
    public static final String MODULE_CONTACTO_TRANSPARENTE = "contactoTransparente";
    public static final String MODULE_REPORTE_DANIOS = "reporteDaños";
    public static final String MODULE_EVENTOS = "eventos";
    public static final String MODULE_ESTACIONES_DE_GAS = "estaciones";
    public static final String MODULE_DE_ALERTASHIDROITUANGO = "alertaRoja";

    public static final String CALLED_FROM_ANOTHER_MODULE = "calledFromAnotherModule";

    /**
     * TypeSuscription
     */
    public static final int TYPE_SUSCRIPTION_ALERTAS = 1;
    public static final int TYPE_SUBSCRIPTION_FACTURE = 2;
    public static final int TYPE_SUBSCRIPTION_CUSTOMER_SERVICE = 11;
    public static final int ID_APPLICATION = 1;

    /**
     * IdentificatorMessage
     */
    public static final int UPDATE_SUCCESFUL = 24;
    /**
     * Alertas
     */
    public static final String MUNICIPALITIES = "listMunicipalities";
    public static final String IMAGE_RESERVED404 = "file:///android_asset/Imagen3.png";
    public static final String EMAILSUBSCRIPTION = "UsuarioInvitado";
    public static final int YOUR_WIDTH_IN_PIXEL_PER_BUTTON = 200;
    /**
     * Notifications
     */
    public static final String SWITCH_COLOR_OFF= "#AAAAAA";
    public static final String SWITCH_COLOR_DISABLED = "#797979";
    public static final String MORE_THAN_99_NOTIFICATIONS = "99+";
    public static final String TITTLE_MESSAGE_APILEVEL_TOOLOW = "¡Actualiza tu sistema operativo!";
    public static final String BODY_MESSAGE_APILEVEL_TOOLOW = "Para que puedas acceder a esta sección de la app, es necesario que actualices tu equipo a la versión Android 5.0 o superior.";
    public static final int SUBSCRIPTION_WENT_RECOVERY = 35;

    /**
     * STATE_SHIFT
     */

    public static final String STATE_SHIFT_ABANDONED = "Abandonado";
    public static final String STATE_SHIFT_ATTENDED = "Atendido";
    public static final String STATE_SHIFT_IN_ATTENTION = "en Atención";
    public static final String STATE_SHIFT_CANCELED = "Cancelado";

    /**
     * turn
     */
    public static final String TYPE_RESOURCE_DRAWABLE= "drawable";
    public static final String EXTRAS_LOCATION= "extrasLocation";
    public static final String INFORMATION_OFFICE= "informationOffice";
    public static final String INFORMATION_OFFICE_JSON= "informationOfficeJson";
    public static final String SHIFT_INFORMATION= "shiftInformation";
    public static final String SHIFT_INFORMATION_JSON= "shiftInformationJson";
    public static final String ASSIGNED_SHIFT= "assignedShift";
    public static final String SHIFT_AND_OFFICE_INFORMATION= "shiftAndOfficeInformation";
    public static final int TIME_OUT_GPS= 4000;
    public static final int DELAY_TIME_OUT_GPS= 1000;
    public static final int DELAY_TIME_DETAIL_OFFICE= 300000;
    public static final String SYSTEM_OPERATIVE= "Android";
    public static final String OFFICE_STATED_CERRADA= "Cerrada";
    public static final int DETAIL_OFFICE_VIEW_MODEL = 0;
    public static final int ASK_TURN_VIEW_MODEL = 1;
    public static final String PACKAGE_GOOGLE_MAPS = "com.google.android.apps.maps";
    public static final String PACKAGE_WAZE = "com.waze";
    public static final String URL_WAZE = "https://waze.com/ul?ll=%s%s%s&navigate=yes&zoom=17";
    public static final String URL_HALF_WAZE ="%2c";
    public static final String URL_DEFAULT_WAZE = "market://details?id=com.waze";
    public static final String URL_GOOGLE_MAPS = "google.navigation:q=%s,%s";
    public static final String NAME_BACKPRESSED = "BACKPRESSED";
    public static final String TEXT_CONCATENATE_DISTANCE = "A %s km |";
    public static final String TEXT_CONCATENAT_TURN_IN_WAIT = "%s turnos en espera";
    public static final String TEXT_CONCATENAT_TURN_ASSIGNED = "Turno asignado %s";
    public static final String CONFIRM = "Si";

    public static final String ITEM_FEATURE_POINT_MAP = "item";
    public static final String ITEM_NAME_FEATURE_POINT_MAP = "name";
    public static final String ITEM_SEARCH_FEATURE_POINT_MAP = "itemSearch";
    public static final String REQUIRE_SEARCH_FEATURE_POINT_MAP = "require";
    public static final String EMPTY_SEARCH_FEATURE_POINT_MAP = "empty";
    public static final String NAME_FEATURE_POINT_MAP = "feature_office_detail";
    public static final String NAME_FEATURE_POINT_MAP_GAS_STATION = "feature_gas_station";
    public static final String NAME_FEATURE_ATTENTION_OFFICES = "feature_attention_offices";
    public static final String NAME_ICON_SOON_MORE_CATEGORY = "soon_more_category";
    public static final String TEXT_SOON_TOU_WILL_SEE_MORE_INFORMATION_ABOUT_PROCEDURES = "Pronto verás aquí la \n información de más trámites";

    public static final String NUMBER_LOCAL_MEDELLIN_CITY = "(034) 4444 115";
    public static final String INFORMATION_ATTENTION_LINES= "attentionLines";
    public static final String PROCEDURE_INFORMATION = "procedureInformation";
}