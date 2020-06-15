package co.gov.ins.guardianes.presentation.view.codeQr

import android.app.Activity
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.Observer
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.models.ExceptionDecreto
import co.gov.ins.guardianes.presentation.models.ExceptionResolution
import co.gov.ins.guardianes.util.Constants.Key.CODE_EXCEPTION
import co.gov.ins.guardianes.util.Constants.Key.EXCEPTION
import co.gov.ins.guardianes.util.Constants.Key.VALUE_EXCEPTION
import kotlinx.android.synthetic.main.activity_exception_list.*
import org.koin.androidx.viewmodel.ext.android.viewModel


class ExceptionListActivity : AppCompatActivity() {

    private val exceptionFormViewModel: ExceptionFormViewModel by viewModel()
    private  var adapterExceptionAdapter = ExceptionAdapter()
    private var exception = 1

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_exception_list)

        initRecycler()
        initObserver()

        exception = intent?.extras?.getInt(EXCEPTION, 1)!!

        if (exception == 1) {
            exceptionFormViewModel.getDecretosDB()
            titleException.text = getString(R.string.text_531)
            subTitle.text = getString(R.string.general_people_531)
        }else {
            exceptionFormViewModel.getResolutionsDB()
            titleException.text = getString(R.string.text_464)
            subTitle.text = getString(R.string.general_people_464)
        }

        close.setOnClickListener {
            finish()
        }

        button.setOnClickListener {
            setData()
        }
    }

    private fun initRecycler() {
        rvException.apply {
            adapter = adapterExceptionAdapter
        }
    }

    private fun updateDecreto(exceptionDecreto: ExceptionDecreto){
        exceptionFormViewModel.updateDecretoSelect(
               exceptionDecreto.id,
               exceptionDecreto.body,
               exceptionDecreto.value,
               exceptionDecreto.isSelect
        )
    }

    private fun updateResolution(resolution: ExceptionResolution){
        exceptionFormViewModel.updateResolutionSelect(
                resolution.id,
                resolution.body,
                resolution.value,
                resolution.isSelect
        )
    }

    private fun initObserver() {
        exceptionFormViewModel.getItemLiveData.observe(this, Observer {
            renderData(it)
        })
    }

    private fun setData(){
        val intent = intent
        val bundle = Bundle()
        val decretosId = ArrayList<String>()
        val resolutionsId = ArrayList<String>()
        adapterExceptionAdapter.itemsDecreto.map {
            if (it.isSelect){
                decretosId.add(it.id.toString())
            }
            updateDecreto(it)
        }
        adapterExceptionAdapter.itemsResolution.map {
            if (it.isSelect){
                resolutionsId.add(it.id.toString())
            }
            updateResolution(it)
        }
        intent.putExtras(bundle)
        val result = if (decretosId.isEmpty()
                && resolutionsId.isEmpty())
            Activity.RESULT_CANCELED else Activity.RESULT_OK
        setResult(result, intent)
        finish()
    }

    private fun renderData(state: ExceptionState?) {
        when(state) {
            is ExceptionState.SuccessDecreto -> {
               adapterExceptionAdapter.itemsDecreto = state.data.toMutableList()
            }
            is ExceptionState.SuccessResolution -> {
                adapterExceptionAdapter.itemsResolution = state.data.toMutableList()
            }
        }

    }
}
