package co.gov.ins.guardianes.di

import co.gov.ins.guardianes.presentation.view.add_participant.AddParticipantViewModel
import co.gov.ins.guardianes.presentation.view.bluetrace.BluetraceViewModel
import co.gov.ins.guardianes.presentation.view.codeQr.CodeQrViewModel
import co.gov.ins.guardianes.presentation.view.codeQr.ExceptionFormViewModel
import co.gov.ins.guardianes.presentation.view.codeQr.mobilityStatus.StatusCodeViewModel
import co.gov.ins.guardianes.presentation.view.diagnosticTip.DiagnosticTipViewModel
import co.gov.ins.guardianes.presentation.view.dialogs.SearchListViewModel
import co.gov.ins.guardianes.presentation.view.healthTip.HealthTipViewModel
import co.gov.ins.guardianes.presentation.view.home.HomeLoginViewModel
import co.gov.ins.guardianes.presentation.view.home.HomeViewModel
import co.gov.ins.guardianes.presentation.view.how_is_colombia.HowIsColombiaViewModel
import co.gov.ins.guardianes.presentation.view.info.CallHomeViewModel
import co.gov.ins.guardianes.presentation.view.participant.ParticipantsViewModel
import co.gov.ins.guardianes.presentation.view.permissions_and_privacy.PermissionsViewModel
import co.gov.ins.guardianes.presentation.view.place.PlaceViewModel
import co.gov.ins.guardianes.presentation.view.quarantineHome.QuarantineHomeViewModel
import co.gov.ins.guardianes.presentation.view.scheduled.ScheduleViewModel
import co.gov.ins.guardianes.presentation.view.semaphone.SemaphoneViewModel
import co.gov.ins.guardianes.presentation.view.smsCheck.CheckSmsViewModel
import co.gov.ins.guardianes.presentation.view.survey.SymptomViewModel
import co.gov.ins.guardianes.presentation.view.user.UserViewModel
import co.gov.ins.guardianes.presentation.view.welcome.WelcomeViewModel
import org.koin.androidx.viewmodel.dsl.viewModel
import org.koin.dsl.module


/**
 * Module where load viewModel
 */
val viewModelModule = module {
    viewModel { ScheduleViewModel(scheduleUc = get()) }
    viewModel { PlaceViewModel(placeUc = get()) }
    viewModel { HealthTipViewModel(healthTipUc = get()) }
    viewModel { HowIsColombiaViewModel(howIsColombiaUc = get(), firebaseEventUc = get()) }
    viewModel { HomeViewModel(menuHomeUc = get(), firebaseEventUc = get(), userPreferences = get(), tokenRepository = get()) }
    viewModel {
        ParticipantsViewModel(
            participantsUc = get(),
            participantsResponseUc = get(),
            userPreferencesUc = get(),
            firebaseEventUc = get(),
            homeLoginUc = get()
        )
    }
    viewModel { QuarantineHomeViewModel(firebaseEventUc = get()) }
    viewModel { CallHomeViewModel(firebaseEventUc = get()) }
    viewModel {
        HomeLoginViewModel(
            firebaseEventUc = get(),
            userPreferencesUc = get(),
            homeLoginUc = get()
        )
    }
    viewModel { UserViewModel(userUc = get()) }
    viewModel { AddParticipantViewModel(addParticipantUc = get(), firebaseEventUc = get()) }
    viewModel { CheckSmsViewModel(checkSmsUc = get(), userPreferencesUc = get()) }
    viewModel {
        SymptomViewModel(
            symptomUc = get(),
            answerUc = get(),
            firebaseEventUc = get(),
            userPreferencesUc = get(),
            participantsUc = get()
        )
    }
    viewModel { DiagnosticTipViewModel(symptomUc = get(), firebaseEventUc = get()) }
    viewModel { StatusCodeViewModel(statusCodeUc = get()) }
    viewModel { CodeQrViewModel(codeQrUc = get(), userPreferencesUc = get()) }
    viewModel {
        ExceptionFormViewModel(
            userPreferencesUc = get(),
            exceptionUc = get(),
            decretoUc = get(),
            resolutionUc = get()
        )
    }
    viewModel { WelcomeViewModel(splashUc = get(), userPreferencesUc = get(), tokenUc = get()) }
    viewModel { SearchListViewModel() }
    viewModel { SemaphoneViewModel(participantsUc = get(), userPreferencesUc = get()) }
    viewModel { BluetraceViewModel(bluetraceUc = get()) }
    viewModel { PermissionsViewModel(permissionsUc = get()) }
}