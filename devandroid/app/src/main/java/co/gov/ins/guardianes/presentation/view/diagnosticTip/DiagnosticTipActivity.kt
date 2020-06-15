package co.gov.ins.guardianes.presentation.view.diagnosticTip

import android.content.Intent
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.ContextCompat
import androidx.lifecycle.Observer
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.view.home.HomeActivity
import co.gov.ins.guardianes.util.Constants
import co.gov.ins.guardianes.util.ext.toEvent
import kotlinx.android.synthetic.main.activity_diagnotic_tip.*
import org.koin.androidx.viewmodel.ext.android.viewModel

class DiagnosticTipActivity : AppCompatActivity() {

    private val diagnosticTipViewModel: DiagnosticTipViewModel by viewModel()
    private val diagnosticTipAdapter by lazy { DiagnosticTipAdapter() }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_diagnotic_tip)
        initRecycler()
        initObserver()
        initListener()
        diagnosticTipViewModel.getDiagnostic(intent.getStringExtra(Constants.Key.DIAGNOSTIC) ?: "")
    }

    private fun initListener() {
        btnCloset.setOnClickListener {
            val intent = Intent(this, HomeActivity::class.java).apply {
                flags = Intent.FLAG_ACTIVITY_CLEAR_TOP
                flags = Intent.FLAG_ACTIVITY_CLEAR_TASK
                flags = Intent.FLAG_ACTIVITY_NEW_TASK
            }
            startActivity(intent)
            finish()
        }
    }

    private fun initObserver() {
        diagnosticTipViewModel.getDiagnosticLiveData.observe(this, Observer {
            when (it) {
                is DiagnosticState.Success -> {
                    diagnosticTipViewModel.createEvent(it.data.value.toEvent())
                    diagnosticTipAdapter.diagnosticColor = it.data.colorText

                    txvTipTitle.text = getString(it.data.title)
                    txvTipDescription.text = getString(it.data.description)

                    txvTipTitle.setTextColor(ContextCompat.getColor(this, it.data.colorText))
                    diagnosticTipAdapter.submitList(it.data.categories)
                }
                else -> {
                }
            }
        })
    }

    private fun initRecycler() {
        rcvTip.adapter = diagnosticTipAdapter
    }
}
