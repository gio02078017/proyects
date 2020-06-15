package co.gov.ins.guardianes.presentation.view.share_app

import android.content.Intent
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.widget.Toolbar
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.helper.Constants
import co.gov.ins.guardianes.presentation.view.home.HomeActivity
import co.gov.ins.guardianes.view.utils.fromHtml
import kotlinx.android.synthetic.main.shared.*

class ShareAppActivity : AppCompatActivity() {

    private var toolbar: Toolbar? = null

    override fun onCreate(bundle: Bundle?) {
        super.onCreate(bundle)
        setContentView(R.layout.shared)

        toolbar = findViewById(R.id.toolbar)
        setupToolbar()
        getHtml()
        getListeners()
        shareApp()
    }

    private fun setupToolbar() {
        setSupportActionBar(toolbar)
        supportActionBar?.setDisplayHomeAsUpEnabled(true)
        supportActionBar?.setDisplayShowHomeEnabled(true)
    }

    private fun getListeners() {
        button_shared.setOnClickListener {
            goHome()
        }
    }

    private fun shareApp() {
        val shareIntent = Intent().apply {
            action = Intent.ACTION_SEND
            putExtra(Intent.EXTRA_SUBJECT, Constants.General.APP_NAME)
            putExtra(Intent.EXTRA_TEXT, getString(R.string.message_to_share) + getString(R.string.url_to_share))
            type = "text/plain"
        }
        startActivity(shareIntent)
    }

    private fun goHome() {
        startActivity(Intent(this, HomeActivity::class.java))
    }

    private fun getHtml() =
            text_shared_message.fromHtml(getString(R.string.shared_message))

}