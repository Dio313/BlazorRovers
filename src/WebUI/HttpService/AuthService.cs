using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Common.AuthViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using WebUI.AuthProvider;

namespace WebUI.HttpService;

public interface IAuthService
{
    Task<LoginResponse> Login(LoginVm loginModel);
    Task<ResetPasswordResponse> ResetPassword(ChangePasswordVm request);
    Task Logout();
}

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly string url = "/api/employee";
    
    public AuthService(HttpClient httpClient,
        ILocalStorageService localStorage, 
        AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<LoginResponse> Login(LoginVm loginModel)
    {
        var result = await _httpClient.PostAsJsonAsync( url +"/login", loginModel);

        var response = await result.Content.ReadFromJsonAsync<LoginResponse>();

        if (response.Successful) {
            await _localStorage.SetItemAsync("authToken", response.Token);
            ((CustomAuthenticationStateProvider)_authStateProvider).MarkUserAsAuthenticated(response.Token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", response.Token);

            return response;
        }

        return response;
    }

    public async Task<ResetPasswordResponse> ResetPassword(ChangePasswordVm request)
    {
        var result = await _httpClient.PutAsJsonAsync(url + "/ResetPassword", request);

        return await result.Content.ReadFromJsonAsync<ResetPasswordResponse>();
    }

    
    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((CustomAuthenticationStateProvider) _authStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}