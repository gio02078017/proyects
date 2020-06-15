using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using GrupoExito.Utilities.Contracts.Generic;

namespace GrupoExito.Models.ViewModels.Payments
{
    public partial class BaseViewModel : INotifyPropertyChanged
    {
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value, Action onChanged = null, [CallerMemberName]string propertyName = "")
        {
            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null) { return; }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class ObservableList<T> : ObservableCollection<T>
    {
        public void AddRange(IEnumerable<T> range)
        {
            if (range == null) { throw new ArgumentNullException(nameof(range)); }

            var items = range.ToList();
            int index = Items.Count;
            foreach (T item in items) { Items.Add(item); }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items, index));
        }
    }
}
