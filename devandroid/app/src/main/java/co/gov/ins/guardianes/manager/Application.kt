package co.gov.ins.guardianes.manager

import android.Manifest
import android.annotation.SuppressLint
import android.content.Context
import android.content.pm.PackageManager
import android.location.Location
import android.location.LocationListener
import android.location.LocationManager
import android.os.Build
import android.os.Bundle
import android.provider.Settings
import android.util.Log
import androidx.appcompat.app.AppCompatDelegate
import co.gov.ins.guardianes.BuildConfig
import co.gov.ins.guardianes.di.*
import com.crashlytics.android.Crashlytics
import com.crashlytics.android.core.CrashlyticsCore
import io.fabric.sdk.android.Fabric
import org.koin.android.ext.koin.androidContext
import org.koin.android.ext.koin.androidLogger
import org.koin.core.context.startKoin

open class Application : android.app.Application() {
    private var latitude = ""
    private var longitude = ""
    private lateinit var locationManager: LocationManager
    private var hasGps = false
    private var locationGps: Location? = null


    private val listModulesKoin = listOf(
        apiModule,
        repositoryModule,
        useCaseModule,
        viewModelModule,
        dbModule
    )



    private fun initKoin() {
        startKoin {
            androidLogger()
            androidContext(this@Application)
            modules(listModulesKoin)
        }
    }



    override fun onCreate() {
        super.onCreate()
        if (checkPermission()) {
            getLocation()
        }
        androidID()
        AppCompatDelegate.setDefaultNightMode(AppCompatDelegate.MODE_NIGHT_NO)
        initKoin()
        initFabric()

    }

    private fun initFabric() {
        val crashlyticsCore = CrashlyticsCore.Builder()
            .disabled(BuildConfig.DEBUG)
            .build()
        Fabric.with(this, Crashlytics.Builder().core(crashlyticsCore).build())
    }



    companion object {
        private val TAG =
            Application::class.java.simpleName
        private var isConfigured = false
    }

    @SuppressLint("MissingPermission")
    private fun getLocation() {


        locationManager = getSystemService(Context.LOCATION_SERVICE) as LocationManager
        hasGps = locationManager.isProviderEnabled(LocationManager.GPS_PROVIDER)
        if (hasGps) {
            if (hasGps) {
                locationManager.requestLocationUpdates(
                    LocationManager.GPS_PROVIDER,
                    5000,
                    0F,
                    object : LocationListener {
                        override fun onLocationChanged(location: Location?) {
                            if (location != null) {
                                locationGps = location
                            }
                        }

                        override fun onStatusChanged(
                            provider: String?,
                            status: Int,
                            extras: Bundle?
                        ) {

                        }

                        override fun onProviderEnabled(provider: String?) {

                        }

                        override fun onProviderDisabled(provider: String?) {

                        }

                    })

                val localGpsLocation =
                    locationManager.getLastKnownLocation(LocationManager.GPS_PROVIDER)
                if (localGpsLocation != null)
                    locationGps = localGpsLocation
            }


            if (locationGps != null) {
                latitude = locationGps!!.latitude.toString()
                longitude = locationGps!!.longitude.toString()
            }

        }
    }

    private fun checkPermission(): Boolean {
        return if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            !(checkSelfPermission(Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED && checkSelfPermission(
                Manifest.permission.ACCESS_COARSE_LOCATION
            ) != PackageManager.PERMISSION_GRANTED)
        } else {
            false
        }

    }

    @SuppressLint("HardwareIds")
    fun androidID(): String {
        val deviceId = Settings.Secure.getString(contentResolver, Settings.Secure.ANDROID_ID)
        Log.e("DEVICEID", deviceId)
        return deviceId
    }
}