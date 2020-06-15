package co.gov.ins.guardianes.presentation.view.place

import android.Manifest
import android.content.ActivityNotFoundException
import android.content.Intent
import android.content.pm.PackageManager
import android.location.Location
import android.net.Uri
import android.os.Bundle
import android.os.Handler
import android.os.Looper
import androidx.core.app.ActivityCompat
import androidx.lifecycle.Observer
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.helper.DialogBuilder
import co.gov.ins.guardianes.presentation.models.Place
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import com.google.android.gms.common.api.GoogleApiClient
import com.google.android.gms.location.LocationListener
import com.google.android.gms.location.LocationRequest
import com.google.android.gms.location.LocationServices
import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.GoogleMap
import com.google.android.gms.maps.model.*
import kotlinx.android.synthetic.main.activity_place.*
import kotlinx.android.synthetic.main.base_toolbar.*
import org.koin.androidx.viewmodel.ext.android.viewModel
import java.util.*

class PlaceActivity : BaseAppCompatActivity(), GoogleApiClient.ConnectionCallbacks,
    LocationListener, GoogleMap.OnMarkerClickListener {

    //region variables
    private val placeViewModel: PlaceViewModel by viewModel()
    private lateinit var mGoogleApiClient: GoogleApiClient
    private var googleMap: GoogleMap? = null
    private lateinit var mLocationRequest: LocationRequest
    private var handler = Handler()
    private var isAnimation = false
    private var placeBottomSheet: PlaceBottomSheet? = null
    private val pointMap: HashMap<Marker, Place> = HashMap()
    private val isPermission: Boolean
        get() {
            return ActivityCompat.checkSelfPermission(
                this,
                Manifest.permission.ACCESS_FINE_LOCATION
            ) == PackageManager.PERMISSION_GRANTED
        }
    //endregion

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_place)
        mapView.onCreate(savedInstanceState)
        iniToolbar()
        initializeApiGoogle()
        initObserver()
        initMap()
        placeViewModel.launchDataLoad()
    }

    private fun iniToolbar() {
        onToolbar(toolbar)
        toolbar.setNavigationOnClickListener {
            onBackPressed()
        }
        txtTitleBar.text = getString(R.string.clinic)
    }

    private fun initMap() {
        mapView?.getMapAsync {
            googleMap = it
            it.setOnMarkerClickListener(this)
            it.setOnCameraIdleListener {
                handler.postDelayed({
                    isAnimation = false
                }, 3000)
            }
            it.setOnCameraMoveStartedListener {
                handler.removeCallbacksAndMessages(null)
                isAnimation = true
            }
        }
    }

    private fun renderLocation() {
        if (isPermission) {
            googleMap?.let {
                it.isMyLocationEnabled = true
                it.uiSettings.isCompassEnabled = true
            }
            val lastLocation =
                LocationServices.FusedLocationApi.getLastLocation(mGoogleApiClient)
            LocationServices.FusedLocationApi.requestLocationUpdates(
                mGoogleApiClient,
                mLocationRequest,
                this@PlaceActivity
            )
            lastLocation?.let {
                animationCameraMap(LatLng(lastLocation.latitude, lastLocation.longitude))
            }
        } else {
            ActivityCompat.requestPermissions(
                this, arrayOf(Manifest.permission.ACCESS_FINE_LOCATION),
                PERMISSION_LOCATION
            )
        }
    }

    override fun onMarkerClick(p0: Marker?): Boolean {
        val place = pointMap[p0]
        placeBottomSheet = place?.let { PlaceBottomSheet.newInstance(it) }
        placeBottomSheet?.show(supportFragmentManager, placeBottomSheet?.tag)
        placeBottomSheet?.getPlaceLiveData?.observe(this, Observer {
            alertConfirm(it)
        })
        return true
    }

    private fun alertConfirm(place: Place) {
        DialogBuilder(this).load()
            .title(R.string.attention)
            .content(R.string.open_google_maps)
            .positiveText(R.string.yes)
            .negativeText(R.string.no)
            .onPositive { _, _ ->
                try {
                    val uri = String.format(
                        Locale.US,
                        "geo:0,0?q=${place.latitude},${place.longitude}"
                    )
                    val intent = Intent(Intent.ACTION_VIEW, Uri.parse(uri)).apply {
                        intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK
                    }
                    startActivity(intent)
                } catch (ignore: ActivityNotFoundException) {
                }
            }
            .onNegative { dialog, _ ->
                dialog.dismiss()
            }.show()
    }

    private fun initObserver() {
        placeViewModel.getPlaceLiveData.observe(this, Observer { list ->
            val handler = Handler(Looper.getMainLooper())
            list.forEach { place ->
                handler.post {
                    val latLng = LatLng(place.latitude, place.longitude)
                    val marker = googleMap?.addMarker(
                        MarkerOptions().icon(
                            BitmapDescriptorFactory.fromResource(R.drawable.pin_map)
                        ).position(latLng)
                    )
                    marker?.let {
                        pointMap[it] = place
                    }
                }
            }
        })
    }

    private fun animationCameraMap(latLng: LatLng) {
        try {
            if (!isAnimation)
                googleMap?.let {
                    val newCamPos = CameraPosition(
                        latLng,
                        12.5f,
                        it.cameraPosition.tilt,
                        it.cameraPosition.bearing
                    )
                    it.animateCamera(CameraUpdateFactory.newCameraPosition(newCamPos), 2000, null)
                }
        } catch (ignore: Exception) {
        }
    }

    private fun initializeApiGoogle() {
        mGoogleApiClient = GoogleApiClient.Builder(this)
            .addConnectionCallbacks(this)
            .addApi(LocationServices.API)
            .build()
        mGoogleApiClient.connect()
        createLocationRequest()
    }

    private fun createLocationRequest() {
        mLocationRequest = LocationRequest().apply {
            interval = INTERVAL
            fastestInterval = FASTEST_INTERVAL
            priority = LocationRequest.PRIORITY_HIGH_ACCURACY
        }
    }

    override fun onLocationChanged(location: Location) {
        animationCameraMap(LatLng(location.latitude, location.longitude))
    }

    override fun onResume() {
        super.onResume()
        mapView.onResume()
    }

    override fun onPause() {
        super.onPause()
        mapView.onPause()
    }

    override fun onLowMemory() {
        super.onLowMemory()
        mapView.onLowMemory()
    }

    override fun onConnected(p0: Bundle?) {
        if (isPermission)
            renderLocation()
    }

    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        when (requestCode) {
            PERMISSION_LOCATION -> {
                if (grantResults.isNotEmpty()
                    && grantResults[0] == PackageManager.PERMISSION_GRANTED
                ) {
                    renderLocation()
                }
            }
        }
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
    }

    override fun onConnectionSuspended(p0: Int) = Unit

    companion object {
        private const val INTERVAL: Long = 1000
        private const val FASTEST_INTERVAL: Long = 5000
        private const val PERMISSION_LOCATION = 1
    }
}
