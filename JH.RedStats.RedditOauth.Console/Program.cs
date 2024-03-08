using System.Diagnostics;
using JH.RedStats.Interfaces;

namespace JH.RedStats.RedditOauth.Console;
using Reddit.AuthTokenRetriever;

class Program
{
    private const string BROWSER_EXE_PATH = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
    
    static void Main(string[] args)
    {
        var refreshToken = AuthorizeUser(RedditConnectionSettings.AppId, RedditConnectionSettings.AppSecret);
        System.Console.WriteLine(refreshToken);
    }
    
    public static string AuthorizeUser(string appId, string appSecret = null, int port = 8080)
    {
        // Create a new instance of the auth token retrieval library.  --Kris
        AuthTokenRetrieverLib authTokenRetrieverLib = new AuthTokenRetrieverLib(appId, appSecret, port);
	
        // Start the callback listener.  --Kris
        // Note - Ignore the logging exception message if you see it.  You can use Console.Clear() after this call to get rid of it if you're running a console app.
        authTokenRetrieverLib.AwaitCallback();
	
        // Open the browser to the Reddit authentication page.  Once the user clicks "accept", Reddit will redirect the browser to localhost:8080, where AwaitCallback will take over.  --Kris
        OpenBrowser(authTokenRetrieverLib.AuthURL());
	
        // Replace this with whatever you want the app to do while it waits for the user to load the auth page and click Accept.  --Kris
        while (authTokenRetrieverLib.RefreshToken == null)
        {
            Thread.Sleep(1000);
        }
	
        // Cleanup.  --Kris
        authTokenRetrieverLib.StopListening();
	
        return (authTokenRetrieverLib.RefreshToken);
    }

    public static void OpenBrowser(string authUrl, string browserPath = BROWSER_EXE_PATH)
    {
        try
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(authUrl);
            Process.Start(processStartInfo);
        }
        catch (System.ComponentModel.Win32Exception)
        {
            // This typically occurs if the runtime doesn't know where your browser is.  Use BrowserPath for when this happens.  --Kris
            ProcessStartInfo processStartInfo = new ProcessStartInfo(browserPath)
            {
                Arguments = authUrl
            };
            Process.Start(processStartInfo);
        }
    }
    
}