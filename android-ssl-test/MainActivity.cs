using Android.Views;
using System.Net;

namespace android_ssl_test;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : Activity
{
    TextView _myTextView;
    Button _myButton;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.activity_main);

        _myTextView = (TextView)FindViewById(Resource.Id.MyTextView);
        _myButton = (Button)FindViewById(Resource.Id.MyButton);
        _myButton.Click += MyButton_Click; 
    }

    private async void MyButton_Click(object? sender, EventArgs e)
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
            _myTextView.Text = "OK";

        }
        catch (Exception ex)
        {
            _myTextView.Text = ex.Message;
            
        }
    }


}