using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using ProiectPDM.Models;
using ProiectPDM.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPDM.Service
{
    public class FirebaseService
    {
        private readonly HttpClient _httpClient;
        private string _baseUrl;
        private string _apiKey;
        private string _userId;

        public FirebaseService()
        {
            _httpClient = new HttpClient();
            LoadConfig();
        }

        private async void LoadConfig()
        {
            using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync("dbProperties.json");
            using StreamReader reader = new StreamReader(fileStream);
            string json = reader.ReadToEnd();
            dynamic config = JsonConvert.DeserializeObject<dynamic>(json);
            _baseUrl = config.BaseUrl;
            _apiKey = config.ApiKey;
        }



        public async Task<bool> AuthenticateUserAsync(string email, string password, bool signIn)
        {
            var url = signIn ? $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_apiKey}"
                             : $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={_apiKey}";

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, new { email, password, returnSecureToken = true });
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<dynamic>(content);
                    _userId = (string)data.localId; 
                    return true;
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<dynamic>(content);
                    string errorMessage = "An error occurred during authentication.";
                    if (error != null && error.error != null && error.error.message != null)
                    {
                        errorMessage = error.error.message;
                    }
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await Application.Current.MainPage.DisplayAlert("Authentication Failed", errorMessage, "OK");
                    });
                    return false;
                }
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Unexpected error: {ex.Message}", "OK");
                });
                return false;
            }
        }


        public async Task<List<Meal>> GetUserMealsAsync()
        {
            var meals = new List<Meal>();
            var response = await _httpClient.GetAsync($"{_baseUrl}users/{_userId}/meals.json");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var mealsDict = JsonConvert.DeserializeObject<Dictionary<string, Meal>>(content);
                if (mealsDict != null)
                {
                    foreach (var mealEntry in mealsDict.Values)
                    {
                        meals.Add(mealEntry);
                    }
                }
            }
            return meals;
        }

        public async Task<List<Meal>> GetTodayUserMealsAsync()
        {
            var todayMeals = new List<Meal>();
            var todayDate = DateTime.UtcNow.ToString("yyyy-MM-dd"); // Ensure this matches the date format used in your Meal model

            var response = await _httpClient.GetAsync($"{_baseUrl}users/{_userId}/meals.json");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var mealsDict = JsonConvert.DeserializeObject<Dictionary<string, Meal>>(content);
                if (mealsDict != null)
                {
                    foreach (var mealEntry in mealsDict.Values)
                    {
                        // Check if the meal date matches today's date
                        if (mealEntry.Date == todayDate)
                        {
                            todayMeals.Add(mealEntry);
                        }
                    }
                }
            }

            return todayMeals;
        }


        public async Task<bool> AddMealAsync(Meal meal)
        {
            var url = $"{_baseUrl}users/{_userId}/meals.json"; 
            var json = JsonConvert.SerializeObject(meal);

            var response = await _httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                _userId = (string)data.localId;
                return true;
            }
            else
            {
                var error = JsonConvert.DeserializeObject<dynamic>(content);
                string errorMessage = "An error occurred while adding the meal to the database.";
                if (error != null && error.error != null && error.error.message != null)
                {
                    errorMessage = error.error.message;
                }
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Failed to add the meal", errorMessage, "OK");
                });
                return false;
            }
        }

    }
}
