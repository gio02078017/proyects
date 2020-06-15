package co.gov.ins.guardianes.data.local.repository

import android.annotation.SuppressLint
import android.content.Context
import android.content.SharedPreferences
import android.provider.Settings
import co.gov.and.coronapp.bluetrace.BluetraceUtils
import co.gov.ins.guardianes.data.remoto.mappers.fromData
import co.gov.ins.guardianes.data.remoto.mappers.fromDomain
import co.gov.ins.guardianes.domain.models.TokenResponse
import co.gov.ins.guardianes.domain.models.UserResponse
import co.gov.ins.guardianes.domain.repository.UserPreferences
import co.gov.ins.guardianes.util.Constants
import com.google.gson.Gson
import co.gov.ins.guardianes.data.remoto.models.UserResponse as UserData

class UserPreferencesImpl(
        private val sharedPreferences: SharedPreferences,
        private val context: Context
) : UserPreferences {

    private val userResponse: UserData?
        get() {
            val userString = sharedPreferences.getString(Constants.Persistence.USER, "")
            return Gson().fromJson(userString, UserData::class.java)
        }

    override fun getUserId(): String = run {
        val userString = sharedPreferences.getString(Constants.Persistence.USER, "")
        val data: UserData = Gson().fromJson(userString, UserData::class.java)
        data.id
    }

    override fun getUser() = userResponse?.fromDomain()

    override fun setUser(user: UserResponse) =
            sharedPreferences.edit()
                    .putString(Constants.Persistence.USER, Gson().toJson(user.fromData())).commit()

    override fun getUserToken() = userResponse?.token ?: ""

    @SuppressLint("HardwareIds")
    override fun getDeviceId(): String =
            Settings.Secure.getString(context.contentResolver, Settings.Secure.ANDROID_ID)

    override fun clear() {
        sharedPreferences.edit().clear().apply()
    }

    override fun getAuthorization() = run {
        val userString = sharedPreferences.getString(Constants.Persistence.TOKEN, "")
        val token = Gson().fromJson(userString, TokenResponse::class.java)
        "Bearer ${token?.token?.trim()}"
    }

    override fun setToken(tokenResponse: TokenResponse): Boolean {
        BluetraceUtils.setAuthorizationToken(context, "Bearer ${tokenResponse.token.trim()}")
        return sharedPreferences.edit()
                .putString(Constants.Persistence.TOKEN, Gson().toJson(tokenResponse)).commit()
    }

    override fun setPermissions(boolean: Boolean): Boolean {
        return sharedPreferences.edit().putBoolean(Constants.Persistence.PEMISSION, boolean).commit()
    }

    override fun setCoachMark(boolean: Boolean): Boolean {
        return sharedPreferences.edit().putBoolean(Constants.Persistence.COACH, boolean).commit()
    }

    override fun getPermissions() = sharedPreferences.getBoolean(Constants.Persistence.PEMISSION, false)
    override fun getCoachMark() = sharedPreferences.getBoolean(Constants.Persistence.COACH, false)

    override fun getToken(): TokenResponse? {
        val userString = sharedPreferences.getString(Constants.Persistence.TOKEN, "")
        return Gson().fromJson(userString, TokenResponse::class.java)
    }
}