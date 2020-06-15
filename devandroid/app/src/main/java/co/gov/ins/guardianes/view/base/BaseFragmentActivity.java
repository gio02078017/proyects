/*
 * Copyright 2015 Igor Morais
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package co.gov.ins.guardianes.view.base;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.view.ViewGroup;

import androidx.fragment.app.FragmentActivity;

import org.jetbrains.annotations.NotNull;


/**
 * @author Igor Morais
 */
public abstract class BaseFragmentActivity extends FragmentActivity implements INavigate, ActivityListener {

    @Override
    public void setContentView(final int layout) {

        onTheme();

        super.setContentView(layout);

    }

    @Override
    public void setContentView(final View view) {

        onTheme();

        super.setContentView(view);

    }

    @Override
    public void setContentView(final View view, final ViewGroup.LayoutParams layoutParam) {

        onTheme();

        super.setContentView(view, layoutParam);

    }

    @Override
    public void onTheme() {

    }

    @Override
    public void navigateTo(@NotNull final Class<? extends Activity> activityClass) {
        startActivity(new Intent(this, activityClass));
    }

    @Override
    public void navigateTo(@NotNull final Class<? extends Activity> activityClass, final int flags) {

        final Intent intent = new Intent(this, activityClass);

        intent.setFlags(flags);

        startActivity(intent);
    }

    @Override
    public void navigateTo(final Class<? extends Activity> activityClass, final Bundle bundle) {

        final Intent intent = new Intent(this, activityClass);

        intent.putExtras(bundle);

        startActivity(intent);
    }

    @Override
    public void navigateNewTaskTo(@NotNull final Class<? extends Activity> activityClass) {
        startActivity(new Intent(this, activityClass));
    }

}
