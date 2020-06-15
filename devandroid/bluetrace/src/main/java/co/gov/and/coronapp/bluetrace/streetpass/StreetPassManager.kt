package co.gov.and.coronapp.bluetrace.streetpass

import android.content.Context
import android.widget.Toast
import co.gov.and.coronapp.bluetrace.BluetraceUtils
import co.gov.and.coronapp.bluetrace.BuildConfig
import co.gov.and.coronapp.bluetrace.R
import co.gov.and.coronapp.bluetrace.logging.CentralLog
import co.gov.and.coronapp.bluetrace.services.BluetoothMonitoringService
import co.gov.and.coronapp.bluetrace.streetpass.persistence.StreetPassRecord
import co.gov.and.coronapp.bluetrace.streetpass.server.StreetPassApi
import co.gov.and.coronapp.bluetrace.streetpass.server.StreetPassRequest
import co.gov.and.coronapp.bluetrace.streetpass.server.StreetPassResponse
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.util.*

object StreetPassManager {

    private const val TAG = "StreetPassManager"

    fun sendRecords(context: Context, request: StreetPassRequest) {
        val streetPassApiApi = Retrofit.Builder()
                .baseUrl(BuildConfig.URL_BASE)
                .addConverterFactory(GsonConverterFactory.create())
                .build()

        val authorization = BluetraceUtils.getAuthorizationToken(context)
        val call: Call<StreetPassResponse?> = streetPassApiApi.create(StreetPassApi::class.java).sendRecords(authorization, request)
        call.enqueue(object : Callback<StreetPassResponse?> {
            override fun onResponse(call: Call<StreetPassResponse?>, response: Response<StreetPassResponse?>) {
                if (response.isSuccessful) {
                    val body: StreetPassResponse? = response.body()

                    if (body !== null && body.status.toLowerCase(Locale.ROOT) == "success") {
                        CentralLog.w(TAG, "Sending StreetPassRecords to Server successful")

                        Toast.makeText(context, context.getText(R.string.SEND_RECORDS_OK_MSG), Toast.LENGTH_LONG).show()
                    } else if (response.code() == 401) {
                        val manager = BluetoothMonitoringService.refreshBearerTokenManager
                        if (manager !== null) {
                            manager.toRefreshBearerToken {
                                if(it) {
                                    sendRecords(context, request)
                                } else {
                                    Toast.makeText(context, context.getText(R.string.SEND_RECORDS_NOT_OK_MSG), Toast.LENGTH_LONG).show()
                                    CentralLog.d(TAG, "Error generating token")
                                }
                            }
                        }
                    } else if (response.code() == 412) {
                        val manager = BluetoothMonitoringService.refreshBearerTokenManager
                        if (manager !== null) {
                            manager.toChangeBearerToken {
                                if(it) {
                                    sendRecords(context, request)
                                } else {
                                    Toast.makeText(context, context.getText(R.string.SEND_RECORDS_NOT_OK_MSG), Toast.LENGTH_LONG).show()
                                    CentralLog.d(TAG, "Error generating token")
                                }
                            }
                        }
                    } else {
                        Toast.makeText(context, context.getText(R.string.SEND_RECORDS_NOT_OK_MSG), Toast.LENGTH_LONG).show()
                        CentralLog.d(TAG, "Error in body of service")
                    }
                } else {
                    Toast.makeText(context, context.getText(R.string.SEND_RECORDS_NOT_OK_MSG), Toast.LENGTH_LONG).show()
                    CentralLog.d(TAG, "Error in response of server")
                }

            }

            override fun onFailure(call: Call<StreetPassResponse?>, t: Throwable) {
                Toast.makeText(context, context.getText(R.string.SEND_RECORDS_NOT_OK_MSG), Toast.LENGTH_LONG).show()
                CentralLog.d(TAG, "Error sending StreetPassRecords")
            }
        })
    }

    fun validateRecord(context: Context, record: StreetPassRecord) {
        val task = BluetoothMonitoringService.ValidateStreetPassRecordsTask(record)
        task.execute(context)
    }
}