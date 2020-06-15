package co.gov.ins.guardianes.presentation.view.bluetrace

import android.os.Bundle
import androidx.appcompat.widget.Toolbar
import androidx.lifecycle.Observer
import co.gov.and.coronapp.bluetrace.BluetraceUtils
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import co.gov.ins.guardianes.view.utils.fromHtml
import kotlinx.android.synthetic.main.activity_bluetrace_send.*
import org.koin.androidx.viewmodel.ext.android.viewModel

class BluetraceSendActivity : BaseAppCompatActivity() {

    private val bluetraceViewModel: BluetraceViewModel by viewModel()
    private var toolbar: Toolbar? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_bluetrace_send)

        setupToolbar()
        setupView()
        initViewModel()
    }

    override fun onResume() {
        super.onResume()
        bluetraceViewModel.getState()
    }

    private fun setupToolbar() {
        toolbar = findViewById(R.id.toolbar)
        setSupportActionBar(toolbar)
        supportActionBar!!.setDisplayHomeAsUpEnabled(true)
        supportActionBar!!.setDisplayShowHomeEnabled(true)
    }

    private fun setupView() {
        txvContent.fromHtml(getString(R.string.send_traces_content))
        btnSend.setOnClickListener {
            BluetraceUtils.sendTraces(applicationContext)
        }
    }

    private fun initViewModel() {
        bluetraceViewModel.getBluetraceLiveDataState.observe(this, Observer {
            renderState(it)
        })
    }

    private fun renderState(state: BluetraceState) {
        when (state) {
            is BluetraceState.SuccessState -> {
                btnSend.isEnabled = state.data
            }
        }
    }
}