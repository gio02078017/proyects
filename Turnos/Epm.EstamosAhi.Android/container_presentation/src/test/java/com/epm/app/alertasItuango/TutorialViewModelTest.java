package com.epm.app.alertasItuango;


import androidx.arch.core.executor.testing.InstantTaskExecutorRule;
import androidx.appcompat.app.AppCompatActivity;

import com.epm.app.mvvm.comunidad.repository.RepositoryShared;
import com.epm.app.mvvm.comunidad.viewModel.TutorialViewModel;

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
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;


public class TutorialViewModelTest {


    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();

    TutorialViewModel tutorialViewModel;

    @Mock
    CustomSharedPreferences customSharedPreferences;

    @InjectMocks
    RepositoryShared repositoryShared;

    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();

    @Before
    public void setUp() throws Exception {
        customSharedPreferences = mock(CustomSharedPreferences.class);
        tutorialViewModel = Mockito.spy(new TutorialViewModel(repositoryShared,customSharedPreferences));
    }


    @Test
    public void testCheckedSaltarIntroIsTrue(){
        when(customSharedPreferences.getString(Constants.SUSCRIPTION_ALERTAS)).thenReturn(Constants.TRUE);
       tutorialViewModel.saltarIntro();
       assertTrue(tutorialViewModel.checked.getValue() == Constants.TRUE);
    }

    @Test
    public void testCheckedSaltarIntroisFalse(){
        when(customSharedPreferences.getString(Constants.SUSCRIPTION_ALERTAS)).thenReturn(Constants.FALSE);
        tutorialViewModel.saltarIntro();
        assertTrue(tutorialViewModel.checked.getValue() == Constants.FALSE);
    }

    @Test
    public void testgetChecked(){
        tutorialViewModel.checked.setValue(Constants.TRUE);
        assertTrue(tutorialViewModel.getChecked().getValue() == Constants.TRUE);
    }

    @Test
    public void testValidateSusbcriptionisNull(){
        when(customSharedPreferences.getString(Constants.SUSCRIPTION_ALERTAS)).thenReturn(null);
        assertTrue(tutorialViewModel.validateSubscription() == Constants.FALSE);
    }


    @Test
    public void testValidateSusbcriptionisTrue(){
        when(customSharedPreferences.getString(Constants.SUSCRIPTION_ALERTAS)).thenReturn(Constants.TRUE);
        assertTrue(tutorialViewModel.validateSubscription() == Constants.TRUE);
    }

    @Test
    public void testValidateSusbcriptionisFalse(){
        when(customSharedPreferences.getString(Constants.SUSCRIPTION_ALERTAS)).thenReturn(Constants.FALSE);
        assertTrue(tutorialViewModel.validateSubscription() == Constants.FALSE);
    }


}
