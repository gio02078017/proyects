package co.gov.ins.guardianes.util.ext

import android.os.Handler
import android.text.*
import android.text.method.LinkMovementMethod
import android.text.style.ClickableSpan
import android.view.View
import android.view.ViewGroup
import android.widget.*
import android.widget.AdapterView.OnItemSelectedListener
import androidx.annotation.DrawableRes
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout
import androidx.transition.Fade
import androidx.transition.Transition
import androidx.transition.TransitionManager
import com.bumptech.glide.Glide
import io.reactivex.Observable


fun ImageView.loadImage(@DrawableRes drawableRes: Int) {
    Glide.with(this.context).load(drawableRes).into(this)
}

fun ImageView.loadImage(url: String) {
    Glide.with(this.context).load(url).into(this)
}

fun SwipeRefreshLayout.progressHiddenDelay() {
    Handler().postDelayed({
        this.isRefreshing = false
    }, 200)
}


fun View.goneDelay() {
    Handler().postDelayed({
        this.visibility = View.GONE
    }, 200)
}

fun View.goneDelayAnimate() {
    Handler().postDelayed({
        visibilityFade(false)
    }, 2000)
}

fun View.visibilityFade(
        show: Boolean
) {
    val transition: Transition = Fade()
    transition.duration = 600
    transition.addTarget(this)
    TransitionManager.beginDelayedTransition(parent as ViewGroup, transition)
    visibility = if (show) View.VISIBLE else View.GONE
}

fun View.expand() {
    animate().alpha(1f).start()
}

fun View.collapse() {
    animate().alpha(0f).start()
}

fun EditText.textWatcherObserver(): Observable<String> {
    return Observable.create { emitter ->
        val textWatcher = object : TextWatcher {

            override fun afterTextChanged(s: Editable?) = Unit

            override fun beforeTextChanged(s: CharSequence?, start: Int, count: Int, after: Int) =
                Unit

            override fun onTextChanged(s: CharSequence?, start: Int, count: Int, after: Int) {
                s?.toString()?.let {
                    emitter.onNext(it)
                }
            }
        }

        this.addTextChangedListener(textWatcher)

        emitter.setCancellable {
            this.removeTextChangedListener(textWatcher)
        }
    }
}

fun RadioButton.radioButtonObservable(): Observable<Boolean> {
    return Observable.create { emitter ->
        this.setOnCheckedChangeListener { _, isChecked -> emitter.onNext(isChecked) }
    }
}

fun CheckBox.checkBoxButtonObservable(): Observable<Boolean> {
    return Observable.create { emitter ->
        this.setOnCheckedChangeListener { _, isChecked -> emitter.onNext(isChecked) }
    }
}

fun Spinner.spinnerObservable(): Observable<Int> {
    return Observable.create { emitter ->
        this.onItemSelectedListener = object : OnItemSelectedListener {
            override fun onItemSelected(
                parentView: AdapterView<*>?,
                selectedItemView: View?,
                position: Int,
                id: Long
            ) {
                emitter.onNext(position)
            }

            override fun onNothingSelected(parentView: AdapterView<*>?) = Unit
        }
    }
}


fun TextView.makeLinks(vararg links: Pair<String, View.OnClickListener>) {
    val spannableString = SpannableString(this.text)
    for (link in links) {
        val clickableSpan = object : ClickableSpan() {
            override fun onClick(view: View) {
                Selection.setSelection((view as TextView).text as Spannable, 0)
                view.invalidate()
                link.second.onClick(view)
            }
        }
        val startIndexOfLink = this.text.toString().indexOf(link.first)
        spannableString.setSpan(
            clickableSpan, startIndexOfLink, startIndexOfLink + link.first.length,
            Spanned.SPAN_EXCLUSIVE_EXCLUSIVE
        )
    }
    this.movementMethod =
        LinkMovementMethod.getInstance() // without LinkMovementMethod, link can not click
    this.setText(spannableString, TextView.BufferType.SPANNABLE)
}



