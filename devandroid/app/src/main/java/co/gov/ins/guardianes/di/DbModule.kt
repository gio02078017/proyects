package co.gov.ins.guardianes.di

import co.gov.ins.guardianes.data.local.db.Injection
import org.koin.android.ext.koin.androidContext
import org.koin.dsl.module

val dbModule = module {
    factory { Injection.tipDataSource(androidContext()) }
    factory { Injection.participantsDataSource(androidContext()) }
    factory { Injection.howIsColombiaDataSource(androidContext()) }
    factory { Injection.scheduleDataSource(androidContext()) }
    factory { Injection.diagnosticDataSource(androidContext()) }
    factory { Injection.questionDataSource(androidContext()) }
    factory { Injection.ruleDiagnosticDataSource(androidContext()) }
    factory { Injection.ruleQuestionDataSource(androidContext()) }
    factory { Injection.homeLoginDataSource(androidContext()) }
    factory { Injection.participantResultDataSource(androidContext()) }
    factory { Injection.codeQrDataSource(androidContext()) }
    single { Injection.decretoDataSource(androidContext()) }
    single { Injection.resolutionDataSource(androidContext()) }
}