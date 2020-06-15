package co.gov.ins.guardianes.presentation.view.home

import android.annotation.SuppressLint
import android.content.Context
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import co.gov.ins.guardianes.R
import co.gov.ins.guardianes.presentation.models.MenuHome
import co.gov.ins.guardianes.util.ext.loadImage
import co.gov.ins.guardianes.view.utils.inflate
import kotlinx.android.synthetic.main.home_item.view.*

class HomeAdapter(context: Context, menuArray: List<MenuHome>) :
    ArrayAdapter<MenuHome>(context, R.layout.home_item, menuArray) {

    @SuppressLint("ViewHolder")
    override fun getView(position: Int, convertView: View?, parent: ViewGroup): View {
        val rowView = parent.inflate(R.layout.home_item, false)
        rowView.run {
            getItem(position)?.let { home ->
                name?.setText(home.title)
                icon?.loadImage(home.icon)
            }
        }
        return rowView
    }
}