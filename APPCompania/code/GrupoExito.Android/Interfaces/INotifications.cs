using GrupoExito.Entities;

namespace GrupoExito.Android.Adapters
{
    public interface INotifications
    {
        void OnViewItemClicked(AppNotification notification);

        void OnDelateItemClicked(AppNotification notification);

    }
}