package co.gov.ins.guardianes.util

object Constants {

    object Key {
        const val PLACE = "place"
        const val TIP = "tip"
        const val EMPTY_STRING = ""
        const val LAST_UPDATE_TIP = "last_update_tip"
        const val LAST_UPDATE_PARTICIPANTS = "last_update_participants"
        const val LAST_UPDATE_SCHEDULE = "last_update_schedule"
        const val LAST_UPDATE_QUESTION = "last_update_question"
        const val SYMPTOM = "síntomas"
        const val FACTOR = "factores"
        const val DIAGNOSTIC = "diagnostic"
        const val ALERT = "ALERTA"
        const val NORMAL = "NORMAL"
        const val WARNING = "ADVERTENCIA"
        const val EXCEPTION = "EXCEPTION"
        const val CODE_EXCEPTION = "CODE_EXCEPTION"
        const val VALUE_EXCEPTION = "VALUE_EXCEPTION"
        const val UNAUTHORIZED = "UNAUTHORIZED"
        const val MESSAGE = "message"
    }

    object Persistence {
        const val COACH = "coach"
        const val PEMISSION = "permission"
        const val PREFERENCES_API = "preference_api"
        const val PREFERENCES_USER = "preference_user"
        const val USER = "user"
        const val TOKEN = "token"
    }

    object EVENT {
        const val MENU_ATTENTION_LINES = "menu_lineas_atencion"
        const val MENU_HEALTH_CENTERS = "menu_centros_salud"
        const val MENU_LAST_NEWS = "menu_ultimas_noticias"
        const val MENU_PRIVACITY = "menu_permisos_privacidad"
        const val MENU_SHARE = "menu_compartir"
        const val MENU_TRACES = "menu_traces"

        /*
           Quarantine home
       */
        const val HOME_CARE_TIPS = "cuidado_casa"
        const val PLATFORMS_TO_LEARN = "plataformas_aprender"
        const val ECONOMIC_ALTERNATIVES = "alternativas_economicas"

        /*
          Info
         */
        const val CALL_BUTTON = "llamar_192"

        /*
            How is Colombia doing
         */
        const val SHARE_IT_APP = "compartir_en_cifras"
        const val SEE_INTERACTIVE_MAP = "mapa_interactivo"

        /*
            How you feel?
         */
        const val REPORT_SYMPTOMS_BUTTON = "reportar_sintomas"
        const val I_FEEL_GOOD = "Me siento bien"
        const val I_FEEL_BAD = "Me siento mal"
        const val ROAD_REPORT_SAVE_PROFILE = "agregar_familiar"

        /*
        Diagnostic result
         */
        const val DIAGNOSTIC_NORMAL = "diag_normal"
        const val DIAGNOSTIC_WARNING = "diag_advertencia"
        const val DIAGNOSTIC_ALERT = "diag_alerta"

        /*
        Mobility status
         */
        const val MOBILITY_STATUS = "estatus_movilidad"
    }

    object Regex {
        const val IS_NUMBER = "^[0-9]{6,30}$"
        const val NUMBER_DONT_START_WITH_0 = "^(?!(0))[0-9]{6,30}$"
        const val IS_NUMBER_TWO = "^([A-Za-z0-9_]{6,30})+"
        const val IS_VALID_NAME = "[a-zA-ZÀ-ÿ\\sá-ú]+"
    }

    object DocumentType {
        const val CC = "CC"
        const val TI = "TI"
        const val CE = "CE"
        const val VISA = "Visa"
        const val PEP = "PEP"
        const val PAS = "Pasaporte"
        const val RC = "Registro Civil"
    }

    object Risk {
        const val RULES_MOTOR_1 = 2
        const val FORM_BACKGROUND = 3
        const val FORM_SYMPTOM_2 = 4
        const val RULES_MOTOR_2 = 5
        const val FORM_SYMPTOM_3 = 6
        const val FORM_FACTOR = 7
        const val RULES_MOTOR_3 = 7
        const val ERROR = 4
    }

    object RULES {
        const val RULES_1 = 1
        const val RULES_2 = 2
        const val RULES_3 = 3
        const val RULES_4 = 4
    }

    object AnswerRisk {
        const val GOOD = 1
        const val WARNING_1 = 21
        const val WARNING_2 = 22
        const val WARNING_3 = 23
        const val WARNING_4 = 24
        const val ALERT_1 = 11
        const val ALERT_2 = 12
        const val ALERT_3 = 13
        const val ALERT_4 = 14
        const val ALERT_5 = 15
    }

    object Symptom {
        const val NONE = "NINGUNO_DE_LOS_ANTERIORES"
        const val NONE2 = "NINGUNA_DE_LOS_ANTERIORES"
        const val NONE3 = "NINGUNA_DE_LAS_ANTERIORES"
        const val FEVER = "FIEBRE"
        const val FEVER_HIGH = "FIEBRE_MAYOR_A_37_5"
        const val DIFFICULTY_BREATHING = "DIFICULTAD_PARA_RESPIRAR"
    }

    object Factor {
        const val WORKER_HEALTH = "SOY_TRABAJADOR_DE_LA_SALUD"
        const val HAVE_RECEIVED_MEDICAL_ATTENTION_ONCE = "HE_RECIBIDO_ATENCION_MEDICA_UNA_VEZ"
        const val HAVE_VISITED_THE_DOCTOR_MORE_AT_ONCE = "HE_VISITADO_AL_MEDICO_MAS_DE_UNA_VEZ"
        const val NO_HAVE_RECEIVED_MEDICAL_ATTENTION_ONCE = "NO_HE_RECIBIDO_ATENCION_MEDICA"
    }

    object DiagnosticResult {
        const val NORMAL = "NORMAL"
        const val WARNING = "ADVERTENCIA"
        const val ALERT = "ALERTA"
    }

    object Diagnostic {
        const val NORMAL_RULE_1 = "cb388ab6-9bf3-4033-ab06-da16d5c2d819"
        const val WARNING_RULE_2_1 = "e66ded72-c55a-4ec2-8d88-df2e6db844cb"
        const val WARNING_RULE_2_2 = "3abd2ea9-74e3-4945-aac7-454498198203"
        const val WARNING_RULE_2_3 = "bb01f096-0ced-4672-b50f-7e30c4a06d54"
        const val WARNING_RULE_2_4 = "7fa462f5-5b8b-46e1-a574-b308607c207f"
        const val ALERT_RULE_3_1 = "74c80274-9b1b-493f-9c69-653ab72d0dd9"
        const val ALERT_RULE_3_2 = "165389f5-161b-47f2-a7f5-20bed053262d"
        const val ERROR = "e77750a4-879b-4429-8efe-d20cb9fa38d2"
    }

    object QrType {
        const val YELLOW = "yellow"
        const val RED = "red"
        const val GREEN = "green"
    }

    object CodeQR {
        const val TXT_TOOLBAR = "txt_toolbar"
        const val TXT_ACTIVITY = "txt_activity"
        const val TXT_CONTENT = "txt_content"
        const val PDF_URL = "pdf_url"
        const val NOT_USER_QR = "Nunca ha generado un QR, porfavor genere uno"
        const val LIST_EXCEPT = "listExcept"
    }

    object Parents {
        val parentsList = listOf("Padre", "Madre", "Hija", "Hijo", "Cónyuge", "Suegro", "Suegra",
                "Yerno", "Nuera", "Abuelo", "Abuela", "Hermano", "Hermana", "Nieto", "Nieta", "Cuñada", "Bisabuelo",
                "Bisabuela", "Biznieto", "Biznieta", "Tío", "Tía", "Sobrino", "Sobrina")
    }
}