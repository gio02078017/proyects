package co.gov.ins.guardianes.presentation.view.survey

import android.Manifest
import android.content.Intent
import android.content.pm.PackageManager
import android.location.Location
import android.os.Bundle
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ActivityCompat
import androidx.lifecycle.Observer
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.data.remoto.models.QuestionRequest
import co.gov.ins.guardianes.presentation.models.Answer
import co.gov.ins.guardianes.presentation.models.Question
import co.gov.ins.guardianes.presentation.view.diagnosticTip.DiagnosticTipActivity
import co.gov.ins.guardianes.util.Constants
import co.gov.ins.guardianes.util.ext.getDateNow
import com.google.android.gms.common.api.GoogleApiClient
import com.google.android.gms.location.LocationListener
import com.google.android.gms.location.LocationRequest
import com.google.android.gms.location.LocationServices
import kotlinx.android.synthetic.main.activity_symptom.*
import kotlinx.android.synthetic.main.base_toolbar.*
import org.koin.androidx.viewmodel.ext.android.viewModel


class SymptomActivity : AppCompatActivity(), GoogleApiClient.ConnectionCallbacks,
    LocationListener {

    private val symptomViewModel: SymptomViewModel by viewModel()
    private val symptomAdapter by lazy { SymptomAdapter() }
    private val listAnswers: ArrayList<Answer> = arrayListOf()
    private var posQuestion: Int = 0
    private var listQuestion: List<Question> = emptyList()

    private lateinit var mGoogleApiClient: GoogleApiClient
    private lateinit var mLocationRequest: LocationRequest

    private var latitude = ""
    private var longitude = ""
    private var name: String = ""

    private val isPermission: Boolean
        get() {
            return ActivityCompat.checkSelfPermission(
                this,
                Manifest.permission.ACCESS_FINE_LOCATION
            ) == PackageManager.PERMISSION_GRANTED
        }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_symptom)
        getUser()
        initListener()
        setupToolbar()
        observablesLiveData()
        setupRecycler()
        symptomViewModel.getDataForm()
        initializeApiGoogle()
    }

    private fun getUser() {
        val idMember = intent.getStringExtra("id_user") ?: ""
        if (idMember.isEmpty()) {
            symptomViewModel.getUser()
        } else {
            symptomViewModel.getFamily(idMember)
        }
    }

    private fun initListener() {
        bntNext.setOnClickListener {
            listAnswers.addAll(symptomAdapter.currentList.filter { it.isSelect })
            posQuestion++
            callData()
        }
    }

    private fun callData() {
        when (posQuestion) {
            Constants.Risk.RULES_MOTOR_1 -> {
                symptomViewModel.getRulesSymptoms(listAnswers, Constants.RULES.RULES_1)
            }

            Constants.Risk.FORM_BACKGROUND -> {
                symptomViewModel.getRulesSymptoms(listAnswers, Constants.RULES.RULES_2)
            }

            Constants.Risk.RULES_MOTOR_2 -> {
                symptomViewModel.getRulesSymptoms(listAnswers, Constants.RULES.RULES_3)
            }

            Constants.Risk.RULES_MOTOR_3 -> {
                symptomViewModel.getRulesSymptoms(listAnswers, Constants.RULES.RULES_4)
            }

            else -> {
                setDataAdapter()
            }
        }
    }

    private fun setDataAdapter() {
        if (listQuestion.size > posQuestion) {
            val question = listQuestion[posQuestion]
            txtTitleBar.text = "${question.title}\n$name"
            txvDescriptionSymptom.text = question.description
            question.answers.forEach {
                it.form = posQuestion + 1
                it.type = question.field
            }
            symptomAdapter.submitList(question.answers.sortedBy { it.order })
        }
    }

    private fun observablesLiveData() {
        symptomViewModel.symptomLiveData.observe(this, Observer { state ->
            when (state) {
                is SymptomState.Success -> {
                    if (state.data.isNotEmpty() && listQuestion.isEmpty()) {
                        listQuestion = state.data
                        callData()
                    }
                }

                is SymptomState.GetRulesRisk -> {
                    manageRisk(state.risk)
                }

                is SymptomState.SuccessName -> {
                    txtTitleBar.text = "${txtTitleBar.text}\n$name"
                    name = state.name
                }
            }
        })
        symptomAdapter.geSymptomLiveData.observe(this, Observer {
            bntNext.isEnabled = it
        })
    }

    private fun manageRisk(risk: Int) {
        when (risk) {
            Constants.AnswerRisk.GOOD -> {
                resultDiagnostic(Constants.Diagnostic.NORMAL_RULE_1)
            }

            Constants.AnswerRisk.ALERT_1 -> {
                setDataAdapter()
            }

            Constants.AnswerRisk.ALERT_2 -> {
                setDataAdapter()
            }

            Constants.AnswerRisk.ALERT_3 -> {
                setDataAdapter()
            }

            Constants.AnswerRisk.ALERT_4 -> {
                resultDiagnostic(Constants.Diagnostic.ALERT_RULE_3_1)
            }

            Constants.AnswerRisk.ALERT_5 -> {
                resultDiagnostic(Constants.Diagnostic.ALERT_RULE_3_2)
            }

            Constants.AnswerRisk.WARNING_1 -> {
                resultDiagnostic(Constants.Diagnostic.WARNING_RULE_2_1)
            }

            Constants.AnswerRisk.WARNING_2 -> {
                resultDiagnostic(Constants.Diagnostic.WARNING_RULE_2_2)
            }

            Constants.AnswerRisk.WARNING_3 -> {
                resultDiagnostic(Constants.Diagnostic.WARNING_RULE_2_3)
            }

            Constants.AnswerRisk.WARNING_4 -> {
                resultDiagnostic(Constants.Diagnostic.WARNING_RULE_2_4)
            }

            else -> {
                resultDiagnostic(Constants.Diagnostic.ERROR)
            }
        }
    }

    private fun resultDiagnostic(id: String) {
        requestAnswer(id)
        showSuccessfulAlert(id)
    }

    private fun requestAnswer(idDiagnostic: String) {
        val idMember = intent.getStringExtra("id_user") ?: ""
        val questionList = ArrayList<QuestionRequest>()
        val answers: MutableList<String> = mutableListOf()
        var question: QuestionRequest
        for (value in listQuestion) {
            value.answers.forEach { _answer ->
                if (_answer.isSelect) {
                    answers.add(_answer.id)
                    question =
                        QuestionRequest(
                            id = value.id,
                            answer = answers
                        )
                    questionList.add(question)
                }

            }
        }
        symptomViewModel.registerAnswer(
            idMember,
            this.getDateNow(),
            questionList,
            idDiagnostic,
            latitude, longitude
        )
    }

    private fun showSuccessfulAlert(idDiagnostic: String) {
        val builder = AlertDialog.Builder(this, R.style.AlertDialogTheme)
        builder.setTitle(R.string.title_symptom_alert)
        builder.setMessage(R.string.content_symptom_alert)
        builder.setCancelable(false)
        builder.setPositiveButton(R.string.button_symptom_alert) { _, _ ->
            val intent = Intent(this, DiagnosticTipActivity::class.java).apply {
                putExtra(Constants.Key.DIAGNOSTIC, idDiagnostic)
            }
            startActivity(intent)
            finish()
        }
        builder.show()
    }

    private fun setupRecycler() {
        rcvForm.adapter = symptomAdapter
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
        toolbar.title = getString(R.string.autodiagnostic)
    }

    private fun renderLocation() {
        if (isPermission) {
            val lastLocation =
                LocationServices.FusedLocationApi.getLastLocation(mGoogleApiClient)
            LocationServices.FusedLocationApi.requestLocationUpdates(
                mGoogleApiClient,
                mLocationRequest,
                this
            )
            lastLocation?.let {
                latitude = lastLocation.latitude.toString()
                longitude = lastLocation.longitude.toString()
            }
        } else {
            ActivityCompat.requestPermissions(
                this, arrayOf(Manifest.permission.ACCESS_FINE_LOCATION),
                PERMISSION_LOCATION
            )
        }
    }

    private fun initializeApiGoogle() {
        mGoogleApiClient = GoogleApiClient.Builder(this)
            .addConnectionCallbacks(this)
            .addApi(LocationServices.API)
            .build()
        mGoogleApiClient.connect()
        createLocationRequest()
    }

    private fun createLocationRequest() {
        mLocationRequest = LocationRequest().apply {
            interval = INTERVAL
            fastestInterval = FASTEST_INTERVAL
            priority = LocationRequest.PRIORITY_HIGH_ACCURACY
        }
    }

    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        when (requestCode) {
            PERMISSION_LOCATION -> {
                if (grantResults.isNotEmpty()
                    && grantResults[0] == PackageManager.PERMISSION_GRANTED
                ) {
                    renderLocation()
                }
            }
        }
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
    }

    override fun onLocationChanged(location: Location) {
        latitude = location.latitude.toString()
        longitude = location.longitude.toString()
    }

    override fun onConnected(p0: Bundle?) {
        renderLocation()
    }

    override fun onConnectionSuspended(p0: Int) = Unit

    companion object {
        private const val INTERVAL: Long = 1000
        private const val FASTEST_INTERVAL: Long = 5000
        private const val PERMISSION_LOCATION = 1
    }
}
