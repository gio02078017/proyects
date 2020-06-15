package co.gov.ins.guardianes.domain.repository

import co.gov.ins.guardianes.domain.models.MenuHome
import co.gov.ins.guardianes.domain.models.VersionApp
import io.reactivex.Single

interface MenuHomeRepository {

    fun getMenuHome(): List<MenuHome>

    fun getAppVersion(): Single<VersionApp>
}