package co.gov.and.coronapp.bluetrace.idmanager

import android.content.Context
import co.gov.and.coronapp.bluetrace.BluetraceUtils
import co.gov.and.coronapp.bluetrace.BuildConfig
import co.gov.and.coronapp.bluetrace.Preference
import co.gov.and.coronapp.bluetrace.idmanager.server.TemporaryIdApi
import co.gov.and.coronapp.bluetrace.idmanager.server.TemporaryIdResponse
import co.gov.and.coronapp.bluetrace.logging.CentralLog
import co.gov.and.coronapp.bluetrace.services.BluetoothMonitoringService
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.util.*
import kotlin.collections.ArrayList

object TempIDManager {

    private const val TAG = "TempIDManager"

    fun storeTemporaryIDs(context: Context, packet: ArrayList<TemporaryID>) {
        CentralLog.d(TAG, "[TempID] Storing temporary IDs into internal DB...")
        BluetraceUtils.broadcastTemporaryIdReceived(context, packet)
    }

    fun getTemporaryIDs(context: Context, action: String) {
        val temporaryIdApi = Retrofit.Builder()
                .baseUrl(BuildConfig.URL_BASE)
                .addConverterFactory(GsonConverterFactory.create())
                .build()

        val authorization = BluetraceUtils.getAuthorizationToken(context)
        val call: Call<TemporaryIdResponse?> = temporaryIdApi.create(TemporaryIdApi::class.java).getTemporaryIds(authorization)
        call.enqueue(object : Callback<TemporaryIdResponse?> {
            override fun onResponse(call: Call<TemporaryIdResponse?>, response: Response<TemporaryIdResponse?>) {
                if (response.isSuccessful) {
                    CentralLog.w(TAG, "Retrieved Temporary IDs from Server")
                    val temporaryIdResponse: TemporaryIdResponse? = response.body()
                    val success: String? = temporaryIdResponse?.status?.toLowerCase(Locale.ROOT)
                    if (temporaryIdResponse !== null && !success.isNullOrEmpty() && success.contentEquals("success")) {
                        val tempIDs: List<TemporaryID> = temporaryIdResponse.tempIds.map { it.fromServer() }

                        if (tempIDs.isNotEmpty()) {
                            storeTemporaryIDs(context, ArrayList(tempIDs))

                            val refresh = temporaryIdResponse.refreshTime
                            Preference.putNextFetchTimeInMillis(
                                    context,
                                    refresh * 1000
                            )
                            Preference.putLastFetchTimeInMillis(
                                    context,
                                    System.currentTimeMillis() * 1000
                            )
                        }
                    }
                } else if (response.code() == 401) {
                    val manager = BluetoothMonitoringService.refreshBearerTokenManager
                    if (manager !== null) {
                        manager.toRefreshBearerToken {
                            if(it) {
                                getTemporaryIDs(context, action)
                            } else {
                                CentralLog.d(TAG, "Error generating token")
                            }
                        }
                    }
                } else if (response.code() == 412) {
                    val manager = BluetoothMonitoringService.refreshBearerTokenManager
                    if (manager !== null) {
                        manager.toChangeBearerToken {
                            if(it) {
                                getTemporaryIDs(context, action)
                            } else {
                                CentralLog.d(TAG, "Error generating token")
                            }
                        }
                    }
                } else {
                    CentralLog.d(TAG, "[TempID] Error in response of server")
                }
                BluetraceUtils.broadcastMessageReceived(context, action)
            }

            override fun onFailure(call: Call<TemporaryIdResponse?>, t: Throwable) {
                CentralLog.d(TAG, "[TempID] Error getting Temporary IDs")
                BluetraceUtils.broadcastMessageReceived(context, action)
            }
        })
    }

    fun needToUpdate(context: Context): Boolean {
        val nextFetchTime =
            Preference.getNextFetchTimeInMillis(context)
        val currentTime = System.currentTimeMillis()

        val update = currentTime >= nextFetchTime
        CentralLog.i(
            TAG,
            "Need to update and fetch TemporaryIDs? $nextFetchTime vs $currentTime: $update"
        )
        return update
    }

    fun needToRollNewTempID(context: Context): Boolean {
        val expiryTime =
            Preference.getExpiryTimeInMillis(context)
        val currentTime = System.currentTimeMillis()
        val update = currentTime >= expiryTime
        CentralLog.d(TAG, "[TempID] Need to get new TempID? $expiryTime vs $currentTime: $update")
        return update
    }
}
