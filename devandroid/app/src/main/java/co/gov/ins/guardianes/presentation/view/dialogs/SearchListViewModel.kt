package co.gov.ins.guardianes.presentation.view.dialogs

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import co.gov.ins.guardianes.util.Constants.Parents.parentsList
import co.gov.ins.guardianes.util.ext.filterWord
import co.gov.ins.guardianes.util.ext.isValidateName
import co.gov.ins.guardianes.view.base.BaseViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch

class SearchListViewModel : BaseViewModel() {

    private val _searchListMutableList = MutableLiveData<SearchListState>()
    val searchListLiveData: LiveData<SearchListState>
            get() = _searchListMutableList

    fun getParents(isFilter: Boolean, query: String = "") {
        GlobalScope.launch(Dispatchers.Main) {
            if (isFilter) {
                val isValid = query.isValidateName()
                _searchListMutableList.value = SearchListState.ShowParents(query.filterWord(parentsList), isValid)
            } else {
                _searchListMutableList.value = SearchListState.ShowParents(parentsList)
            }
        }
    }

    override fun onCleared() {
        super.onCleared()
        disposeBag.clear()
    }

    sealed class SearchListState {
        class ShowParents(val parents: List<String>, val isValid: Boolean = true) : SearchListState()
    }
}