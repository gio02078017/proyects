package co.gov.ins.guardianes.domain.uc

import android.os.Bundle
import com.google.firebase.analytics.FirebaseAnalytics

class FirebaseEventUc(
    private val firebaseAnalytics: FirebaseAnalytics
) {

    fun createEvent(key: String) {
        firebaseAnalytics.logEvent(key, Bundle())
    }

    fun createEvent(key: String, hashMap: HashMap<String, String>) {
        val bundle = Bundle().apply {
            hashMap.mapKeys {
                putString(it.key, it.value)
            }
        }
        firebaseAnalytics.logEvent(key, bundle)
    }
}