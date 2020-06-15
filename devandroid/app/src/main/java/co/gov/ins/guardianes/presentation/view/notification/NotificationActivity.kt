package co.gov.ins.guardianes.presentation.view.notification

import android.content.Intent
import android.os.Bundle
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.view.permissions_and_privacy.PermissionsPrivacyActivity
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import kotlinx.android.synthetic.main.activity_notification.*
import kotlinx.android.synthetic.main.base_toolbar.*

class NotificationActivity : BaseAppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_notification)
        onToolbar(toolbar)
        txtTitleBar.text = getString(R.string.noti_title)

        lySelect.setOnClickListener {
            val intent = Intent(this, PermissionsPrivacyActivity::class.java)
            startActivity(intent)
            finish()
        }
    }
}
