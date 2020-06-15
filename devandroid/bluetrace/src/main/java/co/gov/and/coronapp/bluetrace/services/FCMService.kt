package co.gov.and.coronapp.bluetrace.services

import android.app.NotificationChannel
import android.app.NotificationManager
import android.content.Context
import android.media.RingtoneManager
import android.os.Build
import androidx.core.app.NotificationCompat
import androidx.core.content.ContextCompat
import co.gov.and.coronapp.bluetrace.BuildConfig
import co.gov.and.coronapp.bluetrace.Preference
import co.gov.and.coronapp.bluetrace.R
import co.gov.and.coronapp.bluetrace.logging.CentralLog
import co.gov.and.coronapp.bluetrace.services.BluetoothMonitoringService.Companion.PUSH_NOTIFICATION_ID
import com.google.firebase.messaging.FirebaseMessagingService
import com.google.firebase.messaging.RemoteMessage

private const val TAG = "FCMService"

class FCMService : FirebaseMessagingService() {

    override fun onMessageReceived(remoteMessage: RemoteMessage) {

        CentralLog.d(TAG, "From: ${remoteMessage.from}")

        // Check if message contains a data payload.
        if (remoteMessage.data.isNotEmpty()) {
            CentralLog.d(TAG, "Message data payload: " + remoteMessage.data)

            //Put the announcement into Shared Preferences
            val body = remoteMessage.data["body"] ?: ""
            Preference.putAnnouncement(baseContext, body)
        }

        // Check if message contains a notification payload.
        remoteMessage.notification?.let {
            CentralLog.d(TAG, "Message Notification Body: ${it.body}")
            it.body?.let { msg ->

                //check the notification for data
                sendNotification(it.title, msg)
            }
        }
    }

    override fun onNewToken(token: String) {
        CentralLog.d(TAG, "Refreshed token: $token")
    }

    private fun sendNotification(
        title: String?,
        messageBody: String
    ) {
        val defaultChannelId = BuildConfig.PUSH_NOTIFICATION_CHANNEL_NAME
        val defaultSoundUri = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION)

        val notificationBuilder = NotificationCompat.Builder(this, defaultChannelId)
            .setSmallIcon(R.drawable.ic_notification_service)
            .setContentTitle(title)
            .setContentText(messageBody)
            .setStyle(NotificationCompat.BigTextStyle().bigText(messageBody))
            .setAutoCancel(true)
            .setSound(defaultSoundUri)
            .setColor(ContextCompat.getColor(this, R.color.notification_tint))

        val notificationManager =
            getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager

        // Since android Oreo notification channel is needed.
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {

            //should create channels as needed.
            //e.g next time got channel X? need to create here too
            val channel = NotificationChannel(
                defaultChannelId,
                BuildConfig.PUSH_NOTIFICATION_CHANNEL_NAME,
                NotificationManager.IMPORTANCE_DEFAULT
            )
            notificationManager.createNotificationChannel(channel)
        }

        notificationManager.notify(
            PUSH_NOTIFICATION_ID,
            notificationBuilder.build()
        )
    }
}
