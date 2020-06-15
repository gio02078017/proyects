package com.epm.app.alertasItuango;

import androidx.arch.core.executor.testing.InstantTaskExecutorRule;
import androidx.lifecycle.MutableLiveData;


import com.epm.app.R;
import com.epm.app.mvvm.comunidad.network.response.webViews.InformationInterest;
import com.epm.app.mvvm.comunidad.repository.WebViewRepository;

import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import com.epm.app.mvvm.comunidad.viewModel.InformationViewModel;

import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnit;
import org.mockito.junit.MockitoRule;

import static org.mockito.Mockito.times;
import static org.mockito.Mockito.verify;
import static org.junit.Assert.*;
import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;

import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;

public class InformationInterestViewModelTest {

    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();

    @Mock
    public WebViewRepository interestRepository;

    private InformationInterest informationInterest;
    public InformationViewModel informationViewModel;
    public MutableLiveData<InformationInterest> informationInterestMutableLiveData;

    @Mock
    public CustomSharedPreferences customSharedPreferences;

    public ErrorMessage errorMessage;

    @Before
    public void setUp() throws Exception {
        informationInterest = new InformationInterest();
        errorMessage = new ErrorMessage(0,0);
        informationInterestMutableLiveData = new MutableLiveData<>();
        interestRepository = mock(WebViewRepository.class);
        customSharedPreferences = mock(CustomSharedPreferences.class);
        informationViewModel = Mockito.spy(new InformationViewModel(customSharedPreferences, interestRepository));
    }

    @Test
    public void testgetUrlInformationisNotEmpty() {
        String url= "URL";
        falseRepositories();
        informationInterest.setUrlInformacionDeInteres(url);
        informationInterestMutableLiveData.setValue(informationInterest);
        when(interestRepository.getUrlInformationInterest(customSharedPreferences.getString(Constants.TOKEN))).thenReturn(informationInterestMutableLiveData);
        informationViewModel.getUrlInformation();
        verify(informationViewModel).loadUrl(url);
    }

    @Test
    public void testgetUrlInformationisEmpty() {
        String url= "URL";
        falseRepositories();
        when(interestRepository.getUrlInformationInterest(customSharedPreferences.getString(Constants.TOKEN))).thenReturn(informationInterestMutableLiveData);
        informationViewModel.getUrlInformation();
        verify(informationViewModel,times(0)).loadUrl(url);
    }

    @Test
    public void testLoadUrlIsNotNullOrEmpty(){
        String url="URL";
        informationViewModel.loadUrl(url);
        assertEquals(informationViewModel.getUrl().getValue(),url);
    }


    @Test
    public void testLoadUrlIsNull(){
        String url=null;
        informationViewModel.loadUrl(url);
        assertNull(informationViewModel.getUrl().getValue());
    }

    @Test
    public void testLoadUrlIsEmpty(){
        String url="";
        informationViewModel.loadUrl(url);
        assertNull(informationViewModel.getUrl().getValue());
    }

    @Test
    public void testTryAgainService(){
        falseRepositories();
        when(interestRepository.getUrlInformationInterest(customSharedPreferences.getString(Constants.TOKEN))).thenReturn(informationInterestMutableLiveData);
        informationViewModel.tryAgain();
        verify(informationViewModel).getUrlInformation();
    }

    @Test
    public void testValidateShowError401(){
        ValidateServiceCode.captureServiceErrorCode(Constants.UNAUTHORIZED_ERROR_CODE);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_unauthorized);
        informationViewModel.validateError(errorMessage);
        assertEquals(R.string.text_unauthorized,informationViewModel.getIntError());
    }

    @Test
    public void testValidateShowError404(){
        ValidateServiceCode.captureServiceErrorCode(Constants.NOT_FOUND_ERROR_CODE);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_unauthorized);
        informationViewModel.validateError(errorMessage);
        assertNotNull(informationViewModel.visibilityNotFound.getValue());
    }

    @Test
    public void testValidateShowError500(){
        ValidateServiceCode.captureServiceErrorCode(500);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        informationViewModel.validateError(errorMessage);
        assertNotNull(informationViewModel.visibilityNotFound.getValue());
    }


    private void falseRepositories() {
        String token = "token";
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
    }

}
