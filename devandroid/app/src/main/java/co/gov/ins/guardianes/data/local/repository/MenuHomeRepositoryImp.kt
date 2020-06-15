package co.gov.ins.guardianes.data.local.repository

import co.gov.ins.guardianes.data.local.models.listMenu
import co.gov.ins.guardianes.data.remoto.api.ApiMenuHome
import co.gov.ins.guardianes.data.remoto.models.VersionApp
import co.gov.ins.guardianes.domain.models.MenuHome
import co.gov.ins.guardianes.domain.models.VersionApp as Domain
import co.gov.ins.guardianes.domain.repository.MenuHomeRepository
import io.reactivex.Single
import co.gov.ins.guardianes.data.local.models.MenuHome as Data

class MenuHomeRepositoryImp(private val apiMenuHome: ApiMenuHome) : MenuHomeRepository {

    override fun getMenuHome() =
        listMenu.map {
            it.fromDomain()
        }

    override fun getAppVersion(): Single<Domain> =
            apiMenuHome.getAppVersion().map {
                it.fromDomain()
            }

    private fun Data.fromDomain() = MenuHome(title, icon)

    fun VersionApp.fromDomain() = co.gov.ins.guardianes.domain.models.VersionApp(
            id, platform, minimumVersion
    )
}