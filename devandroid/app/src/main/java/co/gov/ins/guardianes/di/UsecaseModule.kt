package co.gov.ins.guardianes.di

import co.gov.ins.guardianes.domain.uc.*
import com.google.firebase.analytics.FirebaseAnalytics
import org.koin.android.ext.koin.androidContext
import org.koin.dsl.module


val useCaseModule = module {
    factory {
        HealthTipUc(
            healthTipRepository = get(),
            healthTipLocalRepository = get(),
            preferencesUtilRepository = get()
        )
    }
    factory {
        HowIsColombiaUc(
            howIsColombiaRepository = get(),
            howIsColombiaLocalRepository = get()
        )
    }
    factory { PlaceUc(placeRepository = get()) }
    factory { MenuHomeUc(menuHomeRepository = get(), userPreferences = get()) }
    factory { UserUc(userRepository = get(), userPreferences = get()) }
    factory { SplashUc(userPreferences = get()) }
    factory { FirebaseEventUc(firebaseAnalytics = FirebaseAnalytics.getInstance(androidContext())) }
    factory {
        ParticipantsUc(
            participantRepository = get(),
            userPreferences = get(),
            participantLocalRepository = get(),
            preferencesUtilRepository = get(),
            tokenRepository = get()
        )
    }
    factory {
        ScheduleUc(
            scheduleRepository = get(),
            scheduleLocalRepository = get(),
            preferencesUtilRepository = get()
        )
    }
    factory { CheckSmsUc(checkSmsRepository = get(), userPreferences = get()) }
    factory {
        AddParticipantUc(
            addParticipantRepository = get(),
            participantLocalRepository = get(),
            userPreferences = get(),
            tokenRepository = get()
        )
    }
    factory { UserPreferencesUc(userPreferences = get()) }

    factory {
        SymptomUc(
            symptomRepository = get(),
            symptomLocalRepository = get(),
            symptomRulesSymptomUc = get(),
            preferencesUtilRepository = get(),
            tokenRepository = get(),
            userPreferences = get()
        )
    }

    factory { AnswerUc(answerRepository = get(), userPreferences = get(), tokenRepository = get()) }
    factory { ExceptionUc(exceptionRepository = get()) }
    factory {
        HomeLoginUc(
            homeLoginRepository = get(),
            homeLoginLocalRepository = get(),
            userPreferences = get(),
            tokenRepository = get()
        )
    }

    factory {
        ParticipantResponseUc(
            participantResultRepository = get(),
            participantResultLocalRepository = get(),
            userPreferences = get(),
            tokenRepository = get()
        )
    }


    factory {
        StatusCodeUc(
            userPreferences = get(),
            codeQrLocalRepository = get()
        )
    }

    factory {
        CodeQrUc(
            codeQrRepository = get(),
            codeQrLocalRepository = get(),
            userPreferences = get(),
            tokenRepository = get()
        )
    }
    factory { TokenUc(tokenRepository = get(), userPreferences = get()) }

    single { RulesSymptomUc() }

    factory { DecretoUc(exceptionRepository = get(), exceptionLocalRepository = get()) }
    factory { ResolutionUc(exceptionRepository = get(), exceptionLocalRepository = get()) }
    factory { BluetraceUc(bluetraceRepository = get(), userPreferences = get()) }
    factory { PermissionsUc(permissionsRepository = get(), userPreferences = get())
    }
}