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

package co.gov.ins.guardianes.manager;

import android.content.Context;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;

import java.io.IOException;

/**
 * @author Igor Morais
 */
public final class PrefManager extends BaseManager implements IPref {

    private static final String TAG = PrefManager.class.getSimpleName();

    private ObjectMapper mapper;

    private SharedPreferences pref;

    public PrefManager(final Context context) {
        super(context);
    }

    private ObjectMapper getMapper() {

        if (mapper == null) {
            mapper = new ObjectMapper();
        }

        return mapper;
    }

    private SharedPreferences getPref() {

        if (pref == null) {
            pref = PreferenceManager.getDefaultSharedPreferences(getContext());
        }

        return pref;
    }


    @Override
    public boolean getBoolean(final String key, final boolean defValue) {
        return getPref().getBoolean(key, defValue);
    }

    @Override
    public String getString(final String key, final String defValue) {
        return getPref().getString(key, defValue);
    }

    @Override
    public <T> T get(final String key, final Class<T> type) {

        final String json = getPref().getString(key, null);

        if (json == null) {
            System.out.println(key);
            return null;
        }

        System.out.println(json);

        try {

            return getMapper().readValue(json, type);

        } catch (final IOException e) {
        }

        return null;
    }

    @Override
    public <T> T get(final String key, final TypeReference<T> type) {

        final String json = getPref().getString(key, null);

        if (json == null) {
            return null;
        }

        try {

            return getMapper().readValue(json, type);

        } catch (final IOException e) {
        }

        return null;
    }

    @Override
    public boolean putBoolean(final String key, final boolean value) {

        return getPref().edit()
                .putBoolean(key, value)
                .commit();
    }


    @Override
    public <T> boolean put(final String key, final T entity) {

        try {

            final String json = getMapper().writeValueAsString(entity);

            return getPref().edit()
                    .putString(key, json)
                    .commit();

        } catch (JsonProcessingException e) {
        }

        return false;
    }

    @Override
    public boolean remove(final String key) {

        return getPref().edit()
                .remove(key)
                .commit();
    }

    @Override
    public boolean clear() {

        return getPref().edit()
                .clear()
                .commit();
    }
}