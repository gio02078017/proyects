package co.gov.ins.guardianes.di

import co.gov.ins.guardianes.BuildConfig
import co.gov.ins.guardianes.data.remoto.api.*
import co.gov.ins.guardianes.network.RetrofitBuild
import org.koin.dsl.module

val apiModule = module {
    factory { RetrofitBuild.getInstance(BuildConfig.URL_BASE).create(ApiHealthTip::class.java) }
    factory { RetrofitBuild.getInstance(BuildConfig.URL_BASE).create(ApiParticipants::class.java) }
    factory { RetrofitBuild.getInstance(BuildConfig.URL_BASE).create(ApiHowIsColombia::class.java) }
    factory { RetrofitBuild.getInstance(BuildConfig.URL_BASE).create(ApiSchedule::class.java) }
    factory { RetrofitBuild.getInstance(BuildConfig.URL_BASE).create(ApiUser::class.java) }
    factory {
        RetrofitBuild.getInstance(BuildConfig.URL_BASE).create(ApiAddParticipants::class.java)
    }
    factory { RetrofitBuild.getInstance(BuildConfig.API_URL).create(ApiMenuHome::class.java) }
    factory { RetrofitBuild.getInstance(BuildConfig.URL_BASE).create(ApiCheckSms::class.java) }
    factory { RetrofitBuild.getInstance(BuildConfig.URL_BASE).create(ApiToken::class.java) }
    factory {
        RetrofitBuild.getInstance(BuildConfig.URL_BASE_REPORT).create(ApiSymptom::class.java)
    }
    factory {
        RetrofitBuild.getInstance(BuildConfig.URL_BASE_REPORT).create(ApiHomeLogin::class.java)
    }
    factory { RetrofitBuild.getInstance(BuildConfig.URL_BASE_REPORT).create(ApiAnswer::class.java) }
    factory {
        RetrofitBuild.getInstance(BuildConfig.URL_BASE_REPORT)
            .create(ApiParticipantResult::class.java)
    }
    factory {
        RetrofitBuild.getInstance(BuildConfig.URL_BASE_PASSPORT).create(ApiCodeQr::class.java)
    }
    factory { RetrofitBuild.getInstance(BuildConfig.URL_BASE_BLUETRACE).create(ApiBluetrace::class.java) }
    factory { RetrofitBuild.getInstance(BuildConfig.URL_BASE_REPORT).create(ApiPermission::class.java) }
}