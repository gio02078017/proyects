package co.gov.ins.guardianes.presentation.view.dialogs

import android.content.Context
import android.graphics.Point
import android.os.Bundle
import android.view.*
import android.view.View.*
import android.view.inputmethod.InputMethodManager
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import androidx.fragment.app.DialogFragment
import androidx.lifecycle.Observer
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.view.dialogs.SearchListViewModel.SearchListState.ShowParents
import co.gov.ins.guardianes.util.ext.showError
import co.gov.ins.guardianes.util.ext.showNormal
import co.gov.ins.guardianes.util.ext.textWatcherObserver
import io.reactivex.disposables.CompositeDisposable
import io.reactivex.rxkotlin.addTo
import org.koin.androidx.viewmodel.ext.android.viewModel
import java.util.concurrent.TimeUnit

class SearchList : DialogFragment() {

    private val viewModel: SearchListViewModel by viewModel()
    private lateinit var searchListener: SelectItemListener
    private val compositeDisposable = CompositeDisposable()
    private lateinit var adapter: SearchListAdapter
    lateinit var views: View

    private lateinit var tvSearchEditError: TextView
    private lateinit var listSearch: RecyclerView
    private lateinit var editSearch: EditText
    private lateinit var bnClose: Button

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View {

        views = inflater.inflate(R.layout.search_list, container, false)

        instancesViews(views)
        observables()
        onClicks()
        configureRecycler()
        viewModel.getParents(false)

        return views
    }

    private fun configureRecycler() {
        listSearch.layoutManager = LinearLayoutManager(activity)

        adapter = SearchListAdapter {
            dismiss()
            searchListener.selectItem(it)
        }

        listSearch.adapter = adapter
    }

    private fun onClicks() {
        bnClose.setOnClickListener {
            dismiss()
        }
    }

    private fun instancesViews(view: View) {
        tvSearchEditError = view.findViewById(R.id.tv_search_edit_error)
        editSearch = view.findViewById(R.id.search_edit)
        listSearch = view.findViewById(R.id.search_list)
        bnClose = view.findViewById(R.id.bn_close)

        editSearch.onFocusChangeListener = OnFocusChangeListener { _, _ ->
            editSearch.post(Runnable {
                val imm = activity?.getSystemService(Context.INPUT_METHOD_SERVICE) as InputMethodManager
                imm.showSoftInput(editSearch, InputMethodManager.SHOW_IMPLICIT)
            })
        }
        editSearch.requestFocus()
    }

    private fun observables() {
        editSearch.textWatcherObserver()
                .debounce(100, TimeUnit.MILLISECONDS)
                .subscribe {
                    when {
                        it.isEmpty() -> {
                            viewModel.getParents(false)
                        }
                        it.isNotEmpty() -> {
                            viewModel.getParents(true, it)
                        }
                    }
                }.addTo(compositeDisposable)

        viewModel.searchListLiveData.observe(viewLifecycleOwner, Observer {
            when(it) {
                is ShowParents -> {
                    adapter.list = it.parents

                    if (it.isValid) {
                        context?.showNormal(
                                null,
                                editSearch,
                                null
                        )

                        tvSearchEditError.visibility = INVISIBLE
                    } else {
                        context?.showError(
                                null,
                                editSearch,
                                null
                        )
                        tvSearchEditError.visibility = VISIBLE
                    }
                }
            }
        })
    }

    override fun onResume() {
        super.onResume()

        val window: Window? = dialog?.window
        val size = Point()

        val display: Display = window?.windowManager!!.defaultDisplay
        display.getSize(size)

        val width: Int = size.x
        val height: Int = size.y

        window.setLayout((width * 0.80).toInt(), (height * 0.45).toInt())
        window.setGravity(Gravity.CENTER)
    }

    override fun onDestroy() {
        super.onDestroy()
        compositeDisposable.clear()
    }

    override fun onAttach(context: Context) {
        super.onAttach(context)
        searchListener = context as SelectItemListener
    }

    interface SelectItemListener {
        fun selectItem(item: String)
    }
}