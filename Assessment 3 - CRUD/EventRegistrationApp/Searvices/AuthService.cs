using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

public class AuthService
{
    private readonly CustomAuthenticationStateProvider _authenticationStateProvider;
    private readonly HttpClient _httpClient;

    public AuthService(CustomAuthenticationStateProvider authenticationStateProvider, HttpClient httpClient)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _httpClient = httpClient;
    }

    public async Task<bool> Login(string email, string password)
    {
        var loginRequest = new { Email = email, Password = password };
        var response = await _httpClient.PostAsJsonAsync("https://yourapiurl.com/api/auth/login", loginRequest);

        if (response.IsSuccessStatusCode)
        {
            await _authenticationStateProvider.MarkUserAsAuthenticated(email);
            return true;
        }

        return false;
    }

    public Task Logout()
    {
        return _authenticationStateProvider.MarkUserAsLoggedOut();
    }
}
