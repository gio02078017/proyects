package co.gov.ins.guardianes.presentation.view.survey

import android.annotation.SuppressLint
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.ListAdapter
import androidx.recyclerview.widget.RecyclerView
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.models.Answer
import kotlinx.android.synthetic.main.recycler_symptom.view.*

class SymptomAdapter : ListAdapter<Answer, SymptomAdapter.SymptomViewHolder>(DiffCallback()) {

    private val symptomLiveData = MutableLiveData<Boolean>()
    val geSymptomLiveData: LiveData<Boolean>
        get() = symptomLiveData

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): SymptomViewHolder {
        return SymptomViewHolder(
            LayoutInflater.from(parent.context).inflate(R.layout.recycler_symptom, parent, false)
        )
    }

    override fun onBindViewHolder(holder: SymptomViewHolder, position: Int) {
        holder.bind(getItem(position))
    }

    override fun submitList(list: List<Answer>?) {
        super.submitList(list)
        symptomLiveData.value = false
    }

    inner class SymptomViewHolder(val view: View) :
        RecyclerView.ViewHolder(view) {

        @SuppressLint("ResourceAsColor")
        fun bind(answer: Answer) = view.run {
            txvNameSymptom.text = answer.text
            checkSymptom.isChecked = answer.isSelect
            if (answer.isSelect) {
                contentSymptom.setBackgroundResource(R.drawable.layout_background)
            } else {
                contentSymptom.setBackgroundResource(R.drawable.layout_background2)
            }
            contentSymptom.setOnClickListener {
                renderCheck(answer)
                notifyDataSetChanged()
            }
            checkSymptom.setOnClickListener {
                renderCheck(answer)
                notifyDataSetChanged()
            }
        }
    }

    private fun renderCheck(answer: Answer) {
        if (answer.isSingle) {
            currentList.forEach {
                if (it.id == answer.id) {
                    it.isSelect = !it.isSelect
                } else {
                    it.isSelect = false
                }
            }
        } else {
            val symptomNone = currentList.firstOrNull { it.isSingle && it.isSelect }
            symptomNone?.let {
                it.isSelect = false
            }
            answer.isSelect = !answer.isSelect
        }
        symptomLiveData.value = currentList.any { it.isSelect }
    }

    class DiffCallback : DiffUtil.ItemCallback<Answer>() {
        override fun areItemsTheSame(oldItem: Answer, newItem: Answer): Boolean {
            return oldItem.id == newItem.id
        }

        override fun areContentsTheSame(oldItem: Answer, newItem: Answer): Boolean {
            return oldItem == newItem
        }
    }
}