package co.gov.ins.guardianes.view.menu

import android.os.Bundle
import androidx.appcompat.widget.Toolbar
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.view.terms_use.TermsOfUseActivity
import co.gov.ins.guardianes.util.ext.fromHtml
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import kotlinx.android.synthetic.main.about.*

class CoronappAbout : BaseAppCompatActivity() {

    private var toolbar: Toolbar? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.about)
        termText.text = this.fromHtml(getString(R.string.message_terms_about))

        toolbar = findViewById(R.id.toolbar)
        setupToolbar()

        termText.setOnClickListener {
            navigateTo(TermsOfUseActivity::class.java)
        }
    }

    private fun setupToolbar() {
        setSupportActionBar(toolbar)
        supportActionBar!!.setDisplayHomeAsUpEnabled(true)
        supportActionBar!!.setDisplayShowHomeEnabled(true)
    }
}