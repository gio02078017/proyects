package co.gov.ins.guardianes.di

import android.content.Context
import co.gov.ins.guardianes.data.local.repository.*
import co.gov.ins.guardianes.data.remoto.repository.*
import co.gov.ins.guardianes.domain.repository.*
import co.gov.ins.guardianes.domain.repository.howIsColombia.HowIsColombiaLocalRepository
import co.gov.ins.guardianes.domain.repository.howIsColombia.HowIsColombiaRepository
import co.gov.ins.guardianes.util.Constants
import org.koin.android.ext.koin.androidContext
import org.koin.dsl.module

val repositoryModule = module {
    factory<HealthTipRepository> { HealthTipRepositoryImpl(apiHealthTip = get()) }
    factory<PlaceRepository> { PlaceRepositoryImp(context = androidContext()) }
    factory<MenuHomeRepository> { MenuHomeRepositoryImp(apiMenuHome = get()) }
    factory<HowIsColombiaRepository> { HowIsColombiaRepositoryImpl(apiHowIsColombia = get()) }
    factory<ParticipantRepository> { ParticipantRepositoryIml(apiParticipants = get()) }
    factory<UserPreferences> {
        UserPreferencesImpl(
            sharedPreferences = androidContext().getSharedPreferences(
                Constants.Persistence.PREFERENCES_USER,
                Context.MODE_PRIVATE
            ), context = androidContext()
        )
    }
    factory<ScheduleRepository> { ScheduleRepositoryIml(apiSchedule = get()) }
    factory<HealthTipLocalRepository> { HealthTipLocalRepositoryImpl(healthTipDao = get()) }
    factory<ParticipantLocalRepository> { ParticipantLocalRepositoryImpl(participantsDao = get()) }
    factory<PreferencesUtilRepository> {
        PreferencesUtilRepositoryImpl(
            androidContext().getSharedPreferences(
                Constants.Persistence.PREFERENCES_API,
                Context.MODE_PRIVATE
            )
        )
    }
    factory<HowIsColombiaLocalRepository> { HowIsColombiaLocalRepositoryImpl(howIsColombiaDao = get()) }
    factory<ScheduleLocalRepository> { ScheduleLocalRepositoryImpl(scheduleDao = get()) }
    factory<CheckSmsRepository> { CheckSmsRepositoryImpl(apiCheckSms = get()) }
    factory<AddParticipantRepository> { AddParticipantRepositoryIml(apiAddParticipants = get()) }
    factory<SymptomLocalRepository> {
        SymptomLocalRepositoryImpl(
            diagnosticDao = get(),
            questionDao = get(),
            ruleDiagnosticDao = get(),
            ruleQuestionDao = get()
        )
    }
    factory<SymptomRepository> { SymptomRepositoryImpl(apiSymptom = get()) }
    factory<UserRepository> { UserRepositoryIml(apiUser = get()) }
    factory<AnswerRepository> { AnswerRepositoryImpl(apiAnswer = get()) }
    factory<ExceptionRepository> { ExceptionRepositoryImp() }
    factory<HomeLoginLocalRepository> { HomeLoginLocalRepositoryImpl(homeLoginDao = get()) }
    factory<HomeLoginRepository> { HomeLoginRepositoryImp(apiHomeLogin = get()) }

    factory<ParticipantResultLocalRepository> {
        ParticipantResultLocalRepositoryImp(
            participantResultDao = get()
        )
    }
    factory<CodeQrRepository> { CodeQrRepositoryImpl(apiCodeQr = get()) }
    factory<CodeQrLocalRepository> { CodeQrLocalRepositoryImpl(codeQrDao = get()) }
    factory<TokenRepository> { TokenRepositoryImpl(apiToken = get()) }
    factory<ParticipantResultRepository> { ParticipantResultImp(apiParticipantResult = get()) }
    factory<ExceptionLocalRepository> { ExceptionLocalRepositoryImpl(decretoDao = get(), resolutionDao = get()) }
    factory<BluetraceRepository> { BluetraceRepositoryImpl(apiBluetrace = get()) }
    factory<PermissionsRepository> { PermissionsRepositoryIml(apiPermission = get()) }
}