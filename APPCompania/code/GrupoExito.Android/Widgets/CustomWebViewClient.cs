using Android.Graphics;
using Android.Net.Http;
using Android.Runtime;
using Android.Webkit;
using GrupoExito.Android.Interfaces;
using System;

namespace GrupoExito.Android.Widgets
{
    public class CustomWebViewClient : WebViewClient
    {
        ICustomWebClient customWebClient;

        public CustomWebViewClient(ICustomWebClient customWebClient)
        {
            this.customWebClient = customWebClient;
        }

        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            customWebClient.ShouldOverrideUrlLoading(view, request);
            return base.ShouldOverrideUrlLoading(view, request);
        }

        [Obsolete]
        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            customWebClient.ShouldOverrideUrlLoading(view, url);
            return base.ShouldOverrideUrlLoading(view, url);
        }

        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);
        }

        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);
        }

        public override void OnReceivedError(WebView view, IWebResourceRequest request, WebResourceError error)
        {
            base.OnReceivedError(view, request, error);
        }
    }
}