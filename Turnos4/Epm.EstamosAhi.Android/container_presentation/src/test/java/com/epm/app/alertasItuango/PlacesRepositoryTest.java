package com.epm.app.alertasItuango;

import android.arch.core.executor.testing.InstantTaskExecutorRule;
import android.arch.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.network.SuscriptionServices;
import com.epm.app.mvvm.comunidad.repository.PlacesRepository;

import org.junit.Before;
import org.junit.Rule;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.MockitoJUnit;
import org.mockito.junit.MockitoRule;

import app.epm.com.utilities.helpers.ValidateInternet;
import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;


public class PlacesRepositoryTest {

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();
    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();
    @Mock
    private SuscriptionServices webService;
    @Mock
    private ValidateInternet validateInternet;
    @InjectMocks
    PlacesRepository placesRepository;

    MutableLiveData<Integer> data;

    @Before
    public void setup(){
        data = new MutableLiveData<>();
        data.setValue(0);
    }


    /*
    @Test
    public void   = 200;
        int titleError = R.string.title_error;
        int error = R.string.text_error_municipio;
        boolean response = placesRepository.captureServiceCode(code,titleError,error);
        assertTrue(response);
    }

    @Test
    public void testCaptureServiceErrorCodeIsNot200() {
        int code = 401;
        int titleError = R.string.title_error;
        int error = R.string.text_error_municipio;
        boolean response = placesRepository.captureServiceCode(code,titleError,error);
        assertFalse(response);
    }

    @Test
    public void testCaptureServiceErrorCodeIs401(){
        int code = 401;
        placesRepository.captureServiceCode(code,R.string.title_error,R.string.error_400);
        int error = placesRepository.showError().getValue();
        Assert.assertEquals(error,R.string.text_unauthorized);
    }

    @Test
    public void testCaptureServiceErrorCodeIsNot401(){
        int code = 400;
        placesRepository.setError(data);
        placesRepository.captureServiceCode(code,R.string.title_error,R.string.error_400);
        int error = placesRepository.showError().getValue();
        assertFalse(error == R.string.text_unauthorized );
    }

    @Test
    public void testCaptureServiceErrorCodeIs404(){
        int code = 404;
        placesRepository.captureServiceCode(code,R.string.title_error,R.string.text_error_municipio);
        int error = placesRepository.showError().getValue();
        Assert.assertEquals(error,R.string.text_error_municipio);
    }

    @Test
    public void testCaptureServiceErrorCodeIsNot404(){
        int code = 200;
        placesRepository.setError(data);
        placesRepository.captureServiceCode(code,R.string.title_error,R.string.text_error_municipio);
        int error = placesRepository.showError().getValue();
        assertFalse(error == R.string.text_error_municipio );
    }

    @Test
    public void testCaptureServiceErrorCodeis400(){
        int code = 400;
        placesRepository.captureServiceCode(code,R.string.title_error,R.string.error_400);
        int error = placesRepository.showError().getValue();
        Assert.assertEquals(error,R.string.error_400);
    }

    @Test
    public void testCaptureServiceErrorCodeisNot400(){
        int code = 200;
        placesRepository.setError(data);
        placesRepository.captureServiceCode(code,R.string.title_error,R.string.error_400);
        int error = placesRepository.showError().getValue();
        assertFalse(error == R.string.error_400 );
    }*/


}