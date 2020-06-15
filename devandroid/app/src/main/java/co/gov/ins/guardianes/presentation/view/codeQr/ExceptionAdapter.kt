package co.gov.ins.guardianes.presentation.view.codeQr

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.models.ExceptionDecreto
import co.gov.ins.guardianes.presentation.models.ExceptionResolution
import kotlinx.android.synthetic.main.item_list_exception.view.*


class ExceptionAdapter: RecyclerView.Adapter<ExceptionAdapter.ViewHolder>() {

    var itemsDecreto: MutableList<ExceptionDecreto> = arrayListOf()
        set(value) {
            field = value
            notifyDataSetChanged()
        }
    var itemsResolution: MutableList<ExceptionResolution> = arrayListOf()
        set(value) {
            field = value
            notifyDataSetChanged()
        }


    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int) = run {
        val view =
                LayoutInflater.from(parent.context).inflate(R.layout.item_list_exception, parent, false)
        ViewHolder(view)
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        if (itemsDecreto.isNotEmpty())
            holder.bind(itemsDecreto[position]) else holder.bind(itemsResolution[position])
    }

    inner class ViewHolder(
            private val view: View
    ) : RecyclerView.ViewHolder(view) {
        fun bind(any: Any) = view.run {
            if (itemsDecreto.isNotEmpty()) {
                val decreto = any as ExceptionDecreto
                textVal.text =  context.getString(decreto.body)
                checkVal.isChecked = decreto.isSelect
                checkVal.setOnClickListener {
                    decreto.isSelect = checkVal.isChecked
                    }
            }else {
                val resolution = any as ExceptionResolution
                textVal.text =  context.getString(resolution.body)
                checkVal.isChecked = resolution.isSelect
                checkVal.setOnClickListener {
                    resolution.isSelect = checkVal.isChecked
                }
            }
        }
    }

    override fun getItemCount() = if (itemsDecreto.isNotEmpty()) itemsDecreto.size else itemsResolution.size

    override fun getItemId(position: Int): Long {
        return position.toLong()
    }


}