package com.epm.app.alertasItuango;

import androidx.arch.core.executor.testing.InstantTaskExecutorRule;
import androidx.lifecycle.MutableLiveData;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.network.response.webViews.PrivacyPolicy;
import com.epm.app.mvvm.comunidad.repository.WebViewRepository;

import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.ValidateServiceCode;
import com.epm.app.mvvm.comunidad.viewModel.PrivacyPolicyViewModel;

import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnit;
import org.mockito.junit.MockitoRule;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;
import static org.junit.Assert.*;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;

public class PrivacyViewModelTest {

    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();

    @Mock
    public WebViewRepository webViewRepository;

    @InjectMocks
    public PrivacyPolicy privacyPolicy;

    private PrivacyPolicyViewModel privacyPolicyViewModel;
    private MutableLiveData<PrivacyPolicy> policyPrivacyMutableLiveData;

    @Mock
    public CustomSharedPreferences customSharedPreferences;
    public ErrorMessage errorMessage;

    @Before
    public void setUp() throws Exception {
        errorMessage = new ErrorMessage(0,0);
        policyPrivacyMutableLiveData = new MutableLiveData<>();
        webViewRepository = mock(WebViewRepository.class);
        customSharedPreferences = mock(CustomSharedPreferences.class);
        privacyPolicyViewModel = Mockito.spy(new PrivacyPolicyViewModel(webViewRepository, customSharedPreferences));
    }

    @Test
    public void testUrlAvailable(){
        String url = "url";
        privacyPolicy.setUrlPoliticaDePrivacidad(url);
        policyPrivacyMutableLiveData.setValue(privacyPolicy);
        falseRepositories();
        privacyPolicyViewModel.loadUrl();
        assertEquals(url,privacyPolicyViewModel.getUrl());
    }


    @Test
    public void testUrlNotAvailable() {
        falseRepositories();
        privacyPolicyViewModel.loadUrl();
        assertNull(privacyPolicyViewModel.getSuccessful().getValue());
    }

    @Test
    public void testValidateShowError401(){
        ValidateServiceCode.captureServiceErrorCode(Constants.UNAUTHORIZED_ERROR_CODE);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_unauthorized);
        privacyPolicyViewModel.validateShowError(errorMessage);
        assertEquals(R.string.text_unauthorized,privacyPolicyViewModel.getErrorUnauthorized());
    }

    @Test
    public void testValidateShowError404(){
        ValidateServiceCode.captureServiceErrorCode(Constants.NOT_FOUND_ERROR_CODE);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_unauthorized);
        privacyPolicyViewModel.validateShowError(errorMessage);
        assertNotNull(privacyPolicyViewModel.visibilityNotFound.getValue());
    }

    @Test
    public void testValidateShowError500(){
        ValidateServiceCode.captureServiceErrorCode(500);
        errorMessage.setTitle(R.string.title_error);
        errorMessage.setMessage(R.string.text_error_500);
        privacyPolicyViewModel.validateShowError(errorMessage);
        assertNotNull(privacyPolicyViewModel.visibilityNotFound.getValue());
    }

    private void falseRepositories() {
        String token = "token";
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(webViewRepository.getUrlPolicyPrivacy(customSharedPreferences.getString(Constants.TOKEN))).thenReturn(policyPrivacyMutableLiveData);
    }

}
