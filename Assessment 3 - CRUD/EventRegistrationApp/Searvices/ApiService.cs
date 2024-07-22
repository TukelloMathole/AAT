using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using EventRegistrationApp.Models;
using static EventRegistrationApp.Pages.LogIn;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<MyDataType>> GetDataAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<MyDataType>>("/api/Events");
        return response;
    }
    public async Task DeleteEventAsync(int eventId)
    {
        var response = await _httpClient.DeleteAsync($"api/Events/{eventId}");
        response.EnsureSuccessStatusCode();
    }
    public async Task<HttpResponseMessage> CreateEventAsync(MultipartFormDataContent content)
    {
        
        var response = await _httpClient.PostAsync("/api/Events", content);
        return response;
    }
    public async Task<EventModel> GetEventByIdAsync(int eventId)
    {
        var response = await _httpClient.GetFromJsonAsync<EventModel>($"api/Events/{eventId}");
        return response;
    }
    
    public async Task<HttpResponseMessage> UpdateEventAsync(int eventId, MultipartFormDataContent content)
    {
        Console.WriteLine("API Service - UpdateEventAsync");

        // Log each part of the MultipartFormDataContent
        foreach (var part in content)
        {
            var contentName = part.Headers.ContentDisposition.Name.Trim('"');
            var contentValue = await part.ReadAsStringAsync();
            Console.WriteLine($"Content Name: {contentName}, Value: {contentValue}");
        }

        // Use the eventId in the route
        var response = await _httpClient.PutAsync($"api/Events/{eventId}", content);

        // Log the response for debugging
        Console.WriteLine($"Response Status Code: {response.StatusCode}");
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response Content: {responseContent}");

        return response;
    }
    public async Task<HttpResponseMessage> BookEventAsync(int eventId, BookingModel bookingModel)
    {
        Console.WriteLine("at book event");
        var content = new MultipartFormDataContent();
        content.Add(new StringContent(bookingModel.FullName), "FullName");
        content.Add(new StringContent(bookingModel.Email), "Email");
        content.Add(new StringContent(bookingModel.PhoneNumber), "PhoneNumber");
        content.Add(new StringContent(bookingModel.NumberOfTickets.ToString()), "NumberOfTickets");
        // Remove SpecialRequests if not needed

        // Update URL to match the new route
        return await _httpClient.PostAsync($"api/Booking/{eventId}/Book", content);
    }

    public async Task<List<CategoryModel>> GetCategoriesAsync()
    {
        var response = await _httpClient.GetAsync("api/categories");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CategoryModel>>();
    }

    public async Task<string> LoginAsync(LoginModel loginModel)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginModel);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new HttpRequestException($"Login failed. Status code: {response.StatusCode}");
        }
    }

}
