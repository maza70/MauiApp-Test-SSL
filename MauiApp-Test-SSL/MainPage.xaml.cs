using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime;
using System.Threading;

namespace MauiApp_Test_SSL;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        //PROPFIND works only on android when in project <UseNativeHttpHandler>false</UseNativeHttpHandler>
        try
        {
            var webdavUrl = new Uri("https://ubuntu/remote.php/dav/files/zpush");
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                //no callback to accept self signed cetrificates
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

            if (response != null)
            {
                count++;
            }
        }
        catch (Exception ex)
        {
            //on self singed certificates exeption throws 
            /*
            System.Net.Http.HttpRequestException: The SSL connection could not be established, see inner exception.
            ---> System.Security.Authentication.AuthenticationException: Authentication failed, see inner exception.
            ---> Interop+AndroidCrypto+SslException: Exception of type 'Interop+AndroidCrypto+SslException' was thrown.
            --- End of inner exception stack trace ---
            at System.Net.Security.SslStream.<ForceAuthenticationAsync>d__146`1[[System.Net.Security.AsyncReadWriteAdapter, System.Net.Security, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a]].MoveNext()
            at System.Net.Http.ConnectHelper.EstablishSslConnectionAsync(SslClientAuthenticationOptions sslOptions, HttpRequestMessage request, Boolean async, Stream stream, CancellationToken cancellationToken)
            --- End of inner exception stack trace ---
            at System.Net.Http.ConnectHelper.EstablishSslConnectionAsync(SslClientAuthenticationOptions sslOptions, HttpRequestMessage request, Boolean async, Stream stream, CancellationToken cancellationToken)
            at System.Net.Http.HttpConnectionPool.ConnectAsync(HttpRequestMessage request, Boolean async, CancellationToken cancellationToken)
            at System.Net.Http.HttpConnectionPool.CreateHttp11ConnectionAsync(HttpRequestMessage request, Boolean async, CancellationToken cancellationToken)
            at System.Net.Http.HttpConnectionPool.AddHttp11ConnectionAsync(QueueItem queueItem)
            at System.Threading.Tasks.TaskCompletionSourceWithCancellation`1.<WaitWithCancellationAsync>d__1[[System.Net.Http.HttpConnection, System.Net.Http, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a]].MoveNext()
            at System.Net.Http.HttpConnectionPool.HttpConnectionWaiter`1.<WaitForConnectionAsync>d__5[[System.Net.Http.HttpConnection, System.Net.Http, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a]].MoveNext()
            at System.Net.Http.HttpConnectionPool.SendWithVersionDetectionAndRetryAsync(HttpRequestMessage request, Boolean async, Boolean doRequestAuth, CancellationToken cancellationToken)
            at System.Net.Http.AuthenticationHelper.SendWithAuthAsync(HttpRequestMessage request, Uri authUri, Boolean async, ICredentials credentials, Boolean preAuthenticate, Boolean isProxyAuth, Boolean doRequestAuth, HttpConnectionPool pool, CancellationToken cancellationToken)
            at System.Net.Http.RedirectHandler.SendAsync(HttpRequestMessage request, Boolean async, CancellationToken cancellationToken)
            at System.Net.Http.HttpClient.<SendAsync>g__Core|83_0(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationTokenSource cts, Boolean disposeCts, CancellationTokenSource pendingRequestsCts, CancellationToken originalCancellationToken)
            at MauiApp_Test_SSL.MainPage.OnCounterClicked(Object sender, EventArgs e) in S:\no_backup\C#-Tests\MauiApp-Test-SSL\MauiApp-Test-SSL\MainPage.xaml.cs:line 39
             */

            count++;
        }
    }
}

