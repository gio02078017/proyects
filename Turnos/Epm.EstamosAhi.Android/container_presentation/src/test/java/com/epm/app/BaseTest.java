package com.epm.app;

import androidx.arch.core.executor.testing.InstantTaskExecutorRule;
import android.content.Context;
import android.content.res.Resources;

import com.epm.app.turns.RxImmediateSchedulerRule;

import org.junit.After;
import org.junit.Before;
import org.junit.ClassRule;
import org.junit.Rule;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;
import org.mockito.junit.MockitoJUnit;
import org.mockito.junit.MockitoRule;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.utils.Constants;
import io.reactivex.android.plugins.RxAndroidPlugins;
import io.reactivex.schedulers.Schedulers;

import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;

public class BaseTest {

    @Mock
    Context context;
    @Mock
    Resources resources;
    @Mock
    protected ValidateInternet validateInternet;
    @Mock
    protected CustomSharedPreferences customSharedPreferences;
    @Rule
    public MockitoRule mockitoRule = MockitoJUnit.rule();
    @Rule
    public InstantTaskExecutorRule instantRule = new InstantTaskExecutorRule();
    @ClassRule
    public static final RxImmediateSchedulerRule schedulers = new RxImmediateSchedulerRule();

    String token = "";

    @Before
    public void setUp() throws Exception {
        context = mock(Context.class);
        resources = mock(Resources.class);
        RxAndroidPlugins.setInitMainThreadSchedulerHandler(__ -> Schedulers.trampoline());
        MockitoAnnotations.initMocks(this);
        when(customSharedPreferences.getString(Constants.TOKEN)).thenReturn(token);
        when(validateInternet.isConnected()).thenReturn(true);
    }

    @After
    public void tearDown()  {
        context = null;
        resources = null;
        validateInternet = null;
        customSharedPreferences = null;
    }
}
