package co.gov.ins.guardianes.presentation.view.add_participant

import android.content.Intent
import android.os.Bundle
import android.view.View
import android.widget.ArrayAdapter
import android.widget.Toast
import androidx.appcompat.app.AlertDialog
import androidx.lifecycle.Observer
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.view.add_participant.AddParticipantState.*
import co.gov.ins.guardianes.presentation.view.dialogs.SearchList
import co.gov.ins.guardianes.presentation.view.dialogs.SearchList.SelectItemListener
import co.gov.ins.guardianes.presentation.view.terms_use.TermsOfUseActivity
import co.gov.ins.guardianes.util.Constants.EVENT.ROAD_REPORT_SAVE_PROFILE
import co.gov.ins.guardianes.util.ext.fromHtml
import co.gov.ins.guardianes.util.ext.getTypeDocument
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import com.jakewharton.rxbinding2.view.RxView
import io.reactivex.disposables.CompositeDisposable
import io.reactivex.rxkotlin.addTo
import kotlinx.android.synthetic.main.user.*
import org.koin.androidx.viewmodel.ext.android.viewModel
import java.util.concurrent.TimeUnit

class AddParticipantActivity : BaseAppCompatActivity(), SelectItemListener {

    private val addParticipantViewModel: AddParticipantViewModel by viewModel()
    private val compositeDisposable = CompositeDisposable()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.user)
        ccp.registerPhoneNumberTextView(editPhone)

        initView()
        initListener()
        initObserver()
        addParticipantViewModel.validateParticipant(this)
    }

    private fun initListener() {
        radioText.setOnClickListener {
            val intent = Intent(this, TermsOfUseActivity::class.java)
            startActivityForResult(intent, TERMS_USE)
        }
        btnAdd.setOnClickListener {
            addParticipantViewModel.createEvent(ROAD_REPORT_SAVE_PROFILE)
            addParticipantViewModel.registerParticipant(
                    editParent.text.toString(),
                    editName.text.toString(),
                    editLastName.text.toString(),
                    ccp.selectedCountryCode,
                    ccp.phoneNumber.nationalNumber.toString(),
                    editNumberDocument.text.toString(),
                    spinnerTypeDocument.selectedItemPosition.getTypeDocument()
            )
        }

        RxView.clicks(editParent)
                .debounce(200, TimeUnit.MILLISECONDS)
                .subscribe {

                    val fm = supportFragmentManager
                    val searchDialog = SearchList()
                    searchDialog.show(fm, "fragment_parents")
                }.addTo(compositeDisposable)
    }

    private fun initObserver() {
        addParticipantViewModel.getParticipantLiveDataState.observe(this, Observer {
            renderState(it)
        })
    }

    private fun renderState(state: AddParticipantState) {
        progressBar.visibility = View.GONE
        when (state) {

            is Loading -> {
                progressBar.visibility = View.VISIBLE
            }

            is Success -> {
                AlertDialog.Builder(this, R.style.AlertDialogTheme).apply {
                    setTitle(getString(R.string.register_complete))
                    setMessage(getString(R.string.register_alert_text))
                    setCancelable(false)
                    setNegativeButton(getString(R.string.add)) {_, _ ->
                        clearForm()
                    }
                    setPositiveButton(getString(R.string.finish)) { _, _ ->
                        finish()
                    }
                }.show()
            }

            is Error -> {
                state.msg?.let {
                    Toast.makeText(this, it, Toast.LENGTH_LONG).show()
                } ?: run {
                    Toast.makeText(this, getString(R.string.error_default), Toast.LENGTH_LONG)
                            .show()
                }
            }

            is ChangeListeners -> {
                val parentSelect = editParent.text.toString()
                val documentType = spinnerTypeDocument.selectedItem as String
                showAlert(documentType, parentSelect)
            }
        }
    }

    private fun clearForm() {
        editParent.setText("")
        editName.setText("")
        editLastName.setText("")
        editPhone.setText("")
        editNumberDocument.setText("")
        spinnerTypeDocument.setSelection(0)
        radioTerm.isChecked = false
    }

    private fun initView() {
        onToolbar(toolbar)
        radioText.text = this.fromHtml(getString(R.string.message_terms))
        spinnerTypeDocument.adapter = ArrayAdapter(
                this,
                R.layout.spinner_layout,
                R.id.text,
                resources.getStringArray(R.array.type_document_array_participants)
        )
    }

    private fun showAlert(documentType: String, parent: String) {
        if (documentType == REGISTRO_CIVIL && (parent != HIJA && parent != HIJO)) {
            AlertDialog.Builder(this, R.style.AlertDialogTheme).apply {
                setCancelable(false)
                setTitle("ALERTA")
                setMessage("Recuerda que solo padres o\nrepresentantes legales\npueden registrar a un menor.")
                setPositiveButton("Listo") { _, _ -> }
            }.show()
        }
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (requestCode == TERMS_USE) {
            radioTerm.isChecked = true
        }
    }

    override fun onDestroy() {
        super.onDestroy()
        compositeDisposable.clear()
    }

    companion object {
        const val TERMS_USE = 1020
        const val REGISTRO_CIVIL = "Registro Civil"
        const val HIJO = "Hijo"
        const val HIJA = "Hija"
    }

    override fun selectItem(item: String) =
            editParent.setText(item)
}
