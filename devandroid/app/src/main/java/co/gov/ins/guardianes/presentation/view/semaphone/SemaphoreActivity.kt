package co.gov.ins.guardianes.presentation.view.semaphone

import android.os.Bundle
import androidx.lifecycle.Observer
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.view.survey.SymptomActivity
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import kotlinx.android.synthetic.main.activity_semaphore.*
import kotlinx.android.synthetic.main.base_toolbar.*
import org.koin.androidx.viewmodel.ext.android.viewModel

class SemaphoreActivity : BaseAppCompatActivity() {

    private val semaphoneViewModel: SemaphoneViewModel by viewModel()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_semaphore)
        getUser()
        setupToolbar()
        initObserver()
        bnStart.setOnClickListener {
            val bundle = Bundle().apply {
                putString("id_user", intent.getStringExtra("id_user"))
            }
            navigateTo(SymptomActivity::class.java, bundle)
        }
    }

    private fun initObserver() {
        semaphoneViewModel.getNameUserLiveData.observe(this, Observer {
            txtTitleBar.text = getString(R.string.autodiagnostic) + "\n${it}"
        })
    }

    private fun getUser() {
        val idMember = intent.getStringExtra("id_user") ?: ""
        if (idMember.isEmpty()) {
            semaphoneViewModel.getUser()
        } else {
            semaphoneViewModel.getFamily(idMember)
        }
    }

    private fun setupToolbar() {
        setSupportActionBar(toolbar)
        supportActionBar?.run {
            setDisplayHomeAsUpEnabled(true)
            setDisplayShowHomeEnabled(true)
        }
        toolbar.setNavigationOnClickListener {
            onBackPressed()
        }
    }
}
