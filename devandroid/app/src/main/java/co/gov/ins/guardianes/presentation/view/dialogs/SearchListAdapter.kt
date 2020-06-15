package co.gov.ins.guardianes.presentation.view.dialogs

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.constraintlayout.widget.ConstraintLayout
import androidx.recyclerview.widget.RecyclerView
import co.gov.ins.guardianes.R
import kotlin.properties.Delegates

class SearchListAdapter(private val listener: (String) -> Unit) : RecyclerView.Adapter<SearchListAdapter.ViewHolder>() {

    var list: List<String> by Delegates.observable(
            listOf(),
            { _, _, _ -> notifyDataSetChanged() }
    )

    class ViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        private lateinit var itemName: TextView
        private lateinit var container: ConstraintLayout

        fun bind(item: String, listener: (String) -> Unit) {

            itemName = itemView.findViewById(R.id.item_name)
            container = itemView.findViewById(R.id.container)

            itemName.text = item

            itemView.setOnClickListener { listener(item) }
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder = run {
        ViewHolder(LayoutInflater.from(parent.context).inflate(R.layout.item_search_list, parent, false))
    }

    override fun getItemCount(): Int = list.size

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        holder.bind(list[position], listener)
    }
}