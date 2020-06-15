package com.epm.app.services;

import android.annotation.SuppressLint;
import android.app.Service;
import android.arch.lifecycle.MutableLiveData;
import android.content.Context;
import android.content.Intent;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Bundle;
import android.os.IBinder;
import android.util.Log;

import com.epm.app.R;
import app.epm.com.utilities.helpers.ErrorMessage;
import com.epm.app.mvvm.util.IError;

import java.util.Timer;
import java.util.TimerTask;

import javax.inject.Inject;

import app.epm.com.utilities.utils.Constants;

public class ServiceWithControlErrorGPS extends Service implements IError {

    private Context context;
    private MutableLiveData<Location> responseLocation;
    private MutableLiveData<ErrorMessage>  error;
    private boolean askGPS =  false;
    private Timer myTimer;
    private Location lastLocation;

    // The minimum distance to change updates in metters
    private static final long MIN_DISTANCE_CHANGE_FOR_UPDATES = 10; // 10
    // metters

    // The minimum time beetwen updates in milliseconds
    private static final long MIN_TIME_BW_UPDATES = (long) (1000 * 60 * 1); // 1 minute

    private static final String TAG ="Error : Location";

    // Declaring a Location Manager
    protected LocationManager locationManager;

    // flag for GPS Status
    private boolean isGPSEnabled = false;

    // flag for network status
    private boolean isNetworkEnabled = false;

    private boolean startTimeOut;

    @Inject
    public ServiceWithControlErrorGPS(Context context) {
        this.context = context;
        responseLocation = new MutableLiveData<Location>();
        error = new MutableLiveData<>();
     }

    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    @SuppressLint("MissingPermission")
    public boolean askLocation(){

        boolean askSuccessLocation = false;

        try{

            startTimeOut = false;

            locationManager = (LocationManager) context
                    .getSystemService(LOCATION_SERVICE);

            lastLocation = null;

            if(myTimer != null){
                myTimer.cancel();
            }

            stopUpdate();

            // getting GPS status
            isGPSEnabled = locationManager
                    .isProviderEnabled(LocationManager.GPS_PROVIDER);

            // getting network status
            isNetworkEnabled = locationManager
                    .isProviderEnabled(LocationManager.NETWORK_PROVIDER);

            if(isGPSEnabled || isNetworkEnabled){

                askGPS = true;

                    if(isNetworkEnabled && askGPS){

                        lastLocation = locationManager.getLastKnownLocation(LocationManager.NETWORK_PROVIDER);
                        if(lastLocation != null) {
                            locationManager.requestLocationUpdates(
                                    LocationManager.NETWORK_PROVIDER,
                                    MIN_TIME_BW_UPDATES,
                                    MIN_DISTANCE_CHANGE_FOR_UPDATES, locationListenerNetwork);
                        }
                    }

                    if(lastLocation == null && isGPSEnabled && askGPS){
                        lastLocation = locationManager.getLastKnownLocation(LocationManager.GPS_PROVIDER);
                        if(lastLocation == null) {
                            locationManager.requestLocationUpdates(
                                    LocationManager.GPS_PROVIDER,
                                    MIN_TIME_BW_UPDATES,
                                    MIN_DISTANCE_CHANGE_FOR_UPDATES, locationListenerGPS);
                        }

                    }

                    if(lastLocation == null) {
                        startTimeOut = true;
                        timeoutGPS();
                    }else{
                        responseLocation.setValue(lastLocation);
                        Log.e(TAG,
                                "lastLocation "+lastLocation);
                    }

                   askSuccessLocation =  true;
                }

        }catch(Exception e){
            Log.e(TAG,
                    "Impossible to connect to LocationManager metodo getLocation", e);
            sendTimeout(true);
            askSuccessLocation = false;
        }

        return askSuccessLocation;
    }


    private final LocationListener locationListener = new LocationListener() {
        public void onLocationChanged(Location location) {
            askGPS = false;
            cleanUpdate();
            responseLocation.setValue(location);
        }

        @Override
        public void onStatusChanged(String s, int i, Bundle bundle) {
        }

        @Override
        public void onProviderEnabled(String s) {
        }

        @Override
        public void onProviderDisabled(String s) {
        }
    };


    private final LocationListener locationListenerNetwork  = locationListener;

    private final LocationListener locationListenerGPS  = locationListener;

    private void cleanUpdate(){
        stopUpdate();
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
        stopUpdate();
    }

    public void timeoutGPS(){
        myTimer = new Timer();
        myTimer.schedule(new TimerTask() {
            @Override
            public void run() {
                if(!startTimeOut){
                    if(askGPS){
                        myTimer.cancel();
                        sendTimeout(true);
                        askGPS = false;
                        Log.i(TAG,
                                "Timeout GPS");
                    }else{
                       myTimer.cancel();
                    }
                }

                startTimeOut = false;
            }
        }, Constants.DELAY_TIME_OUT_GPS, Constants.TIME_OUT_GPS);
    }

      private void sendTimeout(boolean timeout){
        if(timeout){
            error.postValue(new ErrorMessage(R.string.title_error_gps,R.string.error_gps));
            finishTime();
            stopUpdate();
        }
    }

    @SuppressLint("MissingPermission")
    private void stopUpdate(){
        if(locationListenerNetwork != null ) {
            locationManager.removeUpdates(locationListenerNetwork);
        }
        if(locationListenerGPS != null) {
            locationManager.removeUpdates(locationListenerGPS);
        }
    }

    public void finishTime(){
        myTimer.cancel();
    }

    @Override
    public MutableLiveData<ErrorMessage> showError() {
        return error;
    }



    public MutableLiveData<Location> getResponseLocation() {
        return responseLocation;
    }

    public void setResponseLocation(MutableLiveData<Location> responseLocation) {
        this.responseLocation = responseLocation;
    }

    public Location getLastLocation() {
        return lastLocation;
    }

    public void setLastLocation(Location lastLocation) {
        this.lastLocation = lastLocation;
    }

    public boolean isGPSEnabled() {
        return isGPSEnabled;
    }

    public void setGPSEnabled(boolean GPSEnabled) {
        isGPSEnabled = GPSEnabled;
    }

    public boolean isNetworkEnabled() {
        return isNetworkEnabled;
    }

    public void setNetworkEnabled(boolean networkEnabled) {
        isNetworkEnabled = networkEnabled;
    }
}
