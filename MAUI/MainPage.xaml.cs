using Auth0.OidcClient;
using MAUI.Services;

namespace MAUI;

public partial class MainPage : ContentPage
{
    private readonly Auth0Client _auth0Client;
    private readonly TokenManager _tokenManager;
    private readonly HttpClient _httpClient;

    public MainPage(Auth0Client client, TokenManager tokenManager, HttpClient httpClient)
    {
        InitializeComponent();
        _auth0Client = client;
        
        _tokenManager = tokenManager;
        _httpClient = httpClient;
    }
    
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var loginResult = await _auth0Client.LoginAsync();

        if (!loginResult.IsError)
        {
            AccessTokenEditor.Text = loginResult.AccessToken;
            
            await SecureStorage.SetAsync("access_token", loginResult.AccessToken);
            await SecureStorage.SetAsync("refresh_token", loginResult.RefreshToken);
            
            UsernameLbl.Text = loginResult.User.Identity.Name;
            UserPictureImg.Source = loginResult.User
                .Claims.FirstOrDefault(c => c.Type == "picture")?.Value;
            
            LoginView.IsVisible = false;
            HomeView.IsVisible = true;
        }
        else
        {
            await DisplayAlert("Error", loginResult.ErrorDescription, "OK");
        }
    }
    
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        SecureStorage.Remove("access_token");
        SecureStorage.Remove("refresh_token");
        
        var logoutResult = await _auth0Client.LogoutAsync();
        HomeView.IsVisible = false;
        LoginView.IsVisible = true;
    }
    
    private async void OnMakeRequestClicked(object sender, EventArgs e)
    {
        try
        {
            var response = await _httpClient.GetAsync("/weatherforecast");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Response", content, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", _httpClient.DefaultRequestHeaders.ToString(), ex.Message, "OK");
        }
    }
    
    private void OnCopyAccessTokenClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(AccessTokenEditor.Text))
        {
            Clipboard.Default.SetTextAsync(AccessTokenEditor.Text); // Kopiowanie tokenu do schowka
            DisplayAlert("Copied", "Access token copied to clipboard.", "OK");
        }
        else
        {
            DisplayAlert("Error", "Access token is empty.", "OK");
        }
    }
}