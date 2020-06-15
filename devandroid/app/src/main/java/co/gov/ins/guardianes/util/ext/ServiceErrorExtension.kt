package co.gov.ins.guardianes.util.ext

import co.gov.ins.guardianes.util.Constants
import org.json.JSONArray
import org.json.JSONObject
import org.json.JSONTokener
import retrofit2.HttpException
import java.net.HttpURLConnection


fun Throwable.getTypeServiceError(): String? {

    return if (this is HttpException) {
        when (this.code()) {

            HttpURLConnection.HTTP_UNAUTHORIZED -> {
                Constants.Key.UNAUTHORIZED
            }

            in HttpURLConnection.HTTP_BAD_REQUEST until HttpURLConnection.HTTP_INTERNAL_ERROR -> {
                val data = this.response()?.errorBody()
                data?.let {
                    val value = when (val json = JSONTokener(it.string()).nextValue()) {
                        is JSONObject -> {
                            json.getString(Constants.Key.MESSAGE)
                        }
                        is JSONArray -> {
                            val jsonError = JSONObject(json[0].toString())
                            jsonError.getString(Constants.Key.MESSAGE)
                        }
                        else -> "Mapping http body failed!"
                    }
                    value
                } ?: run {
                    this.message
                }
            }

            else -> {
                message
            }
        }
    } else {
        message
    }
}
