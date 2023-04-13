using System.Net;
using Android.Util;

namespace MauiNet8Preview3_ssl_test
{
    public partial class MainPage : ContentPage
    {
        string tag = "Maui-Net8-Test";
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {

            //to bild and run: dotnet build -t:Run -f net8.0-android
            Log.Info(tag, "Start test");
            //PROPFIND works only on android when in project <UseNativeHttpHandler>false</UseNativeHttpHandler>
            try
            {
                var webdavUrl = new Uri("https://ubuntu/remote.php/dav/files/zpush");
                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    //no callback to accept self signed cetrificates
                    Log.Info(tag, "callback is called");
                    return true;
                };

                //Create a propfind request on a webdav server
                var passwordCache = new CredentialCache();
                passwordCache.Add(webdavUrl, "Basic", new NetworkCredential("zpush", "zpush"));
                handler.Credentials = passwordCache;
                handler.UseDefaultCredentials = false;
                var webclient = new HttpClient(handler, true);
                var request = new HttpRequestMessage(new HttpMethod("PROPFIND"), webdavUrl);
                var response = await webclient.SendAsync(request, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false); ;
                Log.Info(tag, "No exception");
                if (response != null)
                {
                    count++;
                }
            }
            catch (Exception ex)
            {
                Log.Info(tag, $"exception {ex}");
                
            }
        }
    }
}