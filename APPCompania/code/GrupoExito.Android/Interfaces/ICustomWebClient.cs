using Android.Webkit;

namespace GrupoExito.Android.Interfaces
{
    public interface ICustomWebClient
    {
        void ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request);
        void ShouldOverrideUrlLoading(WebView view, string request);
    }
}