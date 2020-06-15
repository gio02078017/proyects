package co.gov.ins.guardianes.presentation.view.permissions_and_privacy

import android.content.ActivityNotFoundException
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import android.view.View
import androidx.appcompat.app.AlertDialog
import androidx.lifecycle.Observer
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.models.Permission
import co.gov.ins.guardianes.presentation.models.PermissionPost
import co.gov.ins.guardianes.presentation.models.Permissions
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import kotlinx.android.synthetic.main.activity_permissions_privacy.*
import kotlinx.android.synthetic.main.base_toolbar.*
import org.koin.androidx.viewmodel.ext.android.viewModel
import java.util.*
import kotlin.collections.ArrayList

class PermissionsPrivacyActivity : BaseAppCompatActivity() {

    private val permissionsViewModel: PermissionsViewModel by viewModel()
    private val permissionAdapter = PermissionAdapter()
    private var listSelect = ArrayList<Boolean>()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_permissions_privacy)
        onToolbar(toolbar)
        permissionsViewModel.getPermissions()
        initObservables()
        initRecyclerView()
        initListeners()
        txtTitleBar.text = getString(R.string.permi_privacy)
    }

    private fun initListeners() {
        button.setOnClickListener {
            val send = PermissionPost(Permissions = ArrayList())
              permissionAdapter.items.forEachIndexed { index, permission ->
                  if (listSelect[index] != permission.accept){
                      send.Permissions.add(Permissions(permission.accept.toString(), permission.id.toInt()))
                  }
              }
            permissionsViewModel.postPermissions(send)
        }

        clShareInfo.setOnClickListener {
            try {
                val intent = Intent(Intent.ACTION_VIEW).apply {
                    setDataAndType(Uri.parse(getString(R.string.url_permissions_pdf)), "application/pdf")
                }
                startActivity(intent)
            } catch (ignore: ActivityNotFoundException) {
                val intent = Intent(Intent.ACTION_VIEW, Uri.parse(getString(R.string.url_permissions_pdf)))
                startActivity(intent)
            }
        }
    }


    private fun initRecyclerView() {
        rvException.apply {
            adapter = permissionAdapter
        }
    }

    private fun initObservables() {
        permissionsViewModel.getPermissionData.observe(this, Observer {state ->
            renderData(state)
        })

        permissionAdapter.getPermissionLiveData.observe(this, Observer {
            button.isEnabled = changeData()
        })
    }

    private fun changeData(): Boolean {
        var change = false
        permissionAdapter.items.forEachIndexed { index, permission ->
            if (listSelect[index] != permission.accept){
                change = true
            }
        }
        return change
    }

    private fun renderData(state: PermissionsState?) {
        when(state){
            is PermissionsState.Loading -> {
                progressBar.visibility = View.VISIBLE
            }

            is PermissionsState.Success -> {
                progressBar.visibility = View.GONE
                permissionAdapter.items = state.data
                state.data.forEach {
                  listSelect.add(it.accept)
                }
            }

            is PermissionsState.SuccessComplete -> {
                progressBar.visibility = View.GONE
                alertDialog()
            }

            is PermissionsState.Error -> {
                progressBar.visibility = View.GONE
            }
        }
    }

    private fun alertDialog() {
        AlertDialog.Builder(this, R.style.AlertDialogTheme).apply {
            setTitle(getString(R.string.title_check))
            setMessage(getString(R.string.permissions_response))
            setCancelable(false)
            setPositiveButton(getString(R.string.button_symptom_alert)) { _, _ ->
                onBackPressed()
            }
        }.show()
    }
}
