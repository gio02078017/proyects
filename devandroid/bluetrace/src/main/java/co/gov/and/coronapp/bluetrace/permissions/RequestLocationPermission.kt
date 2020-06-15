package co.gov.and.coronapp.bluetrace.permissions

import android.Manifest
import android.content.pm.PackageManager
import android.os.Build
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import co.gov.and.coronapp.bluetrace.BluetraceUtils
import pub.devrel.easypermissions.AfterPermissionGranted
import pub.devrel.easypermissions.EasyPermissions

class RequestLocationPermission : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setupPermissionsAndSettings()
    }

    @AfterPermissionGranted(PERMISSION_REQUEST_ACCESS_LOCATION)
    fun setupPermissionsAndSettings() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            val perms = BluetraceUtils.getRequiredPermissions()

            if (EasyPermissions.hasPermissions(this, *perms)) {
                // Already have permission, do the thing
                BluetraceUtils.startBluetoothMonitoringService(this)
                finish()
            } else {
                // Do not have permissions, request them now
                requestPermissions(
                        arrayOf(
                                Manifest.permission.ACCESS_FINE_LOCATION,
                                Manifest.permission.ACCESS_COARSE_LOCATION
                        ), PERMISSION_REQUEST_ACCESS_LOCATION
                )
            }
        } else {
            BluetraceUtils.startBluetoothMonitoringService(this)
            finish()
        }
    }

    override fun onRequestPermissionsResult(
            requestCode: Int,
            permissions: Array<out String>,
            grantResults: IntArray
    ) {
        when (requestCode) {
            PERMISSION_REQUEST_ACCESS_LOCATION -> {
                if (grantResults.isNotEmpty()
                        && grantResults[0] == PackageManager.PERMISSION_GRANTED
                ) {
                    BluetraceUtils.startBluetoothMonitoringService(this)
                    finish()
                } else {
                    BluetraceUtils.stopBluetoothMonitoringService(this)
                    finish()
                }
            }
        }
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
    }

    companion object {
        private const val TAG = "PermissionBT"
        private const val PERMISSION_REQUEST_ACCESS_LOCATION = 456
    }
}
