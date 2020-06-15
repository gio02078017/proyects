package co.gov.ins.guardianes.presentation.view.healthTip

import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.lifecycle.Observer
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.util.Constants
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import kotlinx.android.synthetic.main.activity_health_tip.*
import kotlinx.android.synthetic.main.base_toolbar.*
import org.koin.androidx.viewmodel.ext.android.viewModel

class HealthTipActivity : BaseAppCompatActivity() {

    private val healthTipViewModel: HealthTipViewModel by viewModel()
    private val healthTipAdapter = HealthTipAdapter()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_health_tip)
        initToolbar()
        initObserver()
        initRecycler()
        healthTipViewModel.getHealthTip()
    }

    private fun initToolbar() {
        onToolbar(toolbar)
        toolbar.setNavigationOnClickListener {
            onBackPressed()
        }
        txtTitleBar.text = getString(R.string.tip_home_care)
    }

    private fun initObserver() {
        healthTipViewModel.getTipLiveDataState.observe(this, Observer {
            renderState(it)
        })

        healthTipAdapter.getTipLiveData.observe(this, Observer {
            val intent = Intent(this, DetailHealthTipActivity::class.java).apply {
                putExtra(Constants.Key.TIP, it)
            }
            startActivity(intent)
        })
    }

    private fun renderState(state: HealthTipState) {
        srlHealthTip.isRefreshing = false
        when (state) {
            is HealthTipState.Loading -> {
                srlHealthTip.isRefreshing = true
            }

            is HealthTipState.Success -> {
                healthTipAdapter.items = state.data
            }

            is HealthTipState.Error -> {
                state.msg?.let {
                    Toast.makeText(this, it, Toast.LENGTH_LONG).show()
                } ?: run {
                    Toast.makeText(this, getString(R.string.error_default), Toast.LENGTH_LONG)
                        .show()
                }
            }
        }
    }

    private fun initRecycler() {
        rvHealthTip.apply {
            adapter = healthTipAdapter
        }

        srlHealthTip.setOnRefreshListener {
            healthTipViewModel.getHealthTip()
        }
    }
}
