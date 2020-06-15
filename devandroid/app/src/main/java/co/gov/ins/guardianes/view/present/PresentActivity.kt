package co.gov.ins.guardianes.view.present

import android.os.Bundle
import androidx.appcompat.widget.Toolbar
import androidx.recyclerview.widget.DividerItemDecoration
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.helper.Constants
import co.gov.ins.guardianes.model.ModelBasics
import co.gov.ins.guardianes.view.base.BaseAppCompatActivity
import co.gov.ins.guardianes.view.web.WebViewActivity

class PresentActivity : BaseAppCompatActivity() {

    lateinit var list: RecyclerView
    lateinit var toolbar: Toolbar
    lateinit var data: ArrayList<ModelBasics>
    lateinit var adapter: PresentAdapter

    override fun onCreate(bundle: Bundle?) {
        super.onCreate(bundle)
        setContentView(R.layout.present)
        toolbar = findViewById(R.id.toolbar)
        list = findViewById(R.id.list)
        setupToolbar()
        loadData()
        adapter = PresentAdapter {
            if (it.id == 1) goCorCol() else goStatus()
        }
        setupRecycler()
    }

    public override fun onResume() = super.onResume()

    private fun setupToolbar() {
        setSupportActionBar(toolbar)
        supportActionBar?.let {
            it.setDisplayHomeAsUpEnabled(true)
            it.setDisplayShowHomeEnabled(true)
        }
    }

    private fun setupRecycler() {
        list.layoutManager = LinearLayoutManager(this)
        list.adapter = adapter
        adapter.data = data
        val dividerItemDecoration = DividerItemDecoration(this, DividerItemDecoration.VERTICAL)
        list.addItemDecoration(dividerItemDecoration)
    }

    private fun loadData() {
        data = arrayListOf()
        data.add(
            ModelBasics(
                1,
                "Coronavirus en Colombia",
                "Acciones tomadas por el Gobierno, información general, mitos, preguntas, entre otros."
            )
        )
        data.add(
            ModelBasics(
                2,
                "Estado de los casos",
                "Consulta la información oficial de la evolución del Coronavirus en Colombia."
            )
        )
    }

    private fun goStatus() {
        val bundle = Bundle()
        bundle.putString("title", "Estado de los casos")
        bundle.putString(Constants.Bundle.EXTRA_URL, Constants.Url.STATUS_MAP)
        navigateTo(WebViewActivity::class.java, bundle)
    }

    private fun goCorCol() {
        val bundle = Bundle()
        bundle.putString("title", "Coronavirus en Colombia")
        bundle.putString(Constants.Bundle.EXTRA_URL, Constants.Url.COR_COL)
        navigateTo(WebViewActivity::class.java, bundle)
    }
}