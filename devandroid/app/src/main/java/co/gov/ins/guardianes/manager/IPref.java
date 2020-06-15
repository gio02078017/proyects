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

import com.fasterxml.jackson.core.type.TypeReference;

/**
 * @author Igor Morais
 */
public interface IPref {

    boolean getBoolean(String key, boolean defValue);

    String getString(String key, String defValue);

    <T> T get(String key, Class<T> type);

    <T> T get(String key, TypeReference<T> type);

    boolean putBoolean(String key, boolean value);

    <T> boolean put(String key, T entity);

    boolean remove(String key);

    boolean clear();
}