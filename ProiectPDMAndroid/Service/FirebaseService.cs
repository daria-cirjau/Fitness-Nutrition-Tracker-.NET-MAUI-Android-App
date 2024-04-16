using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using ProiectPDM.Models;
using ProiectPDM.Views;
using ProiectPDMAndroid.Models;
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
        private static string _userId;

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
            var todayDate = DateTime.UtcNow.ToString("yyyy-MM-dd"); 

            var response = await _httpClient.GetAsync($"{_baseUrl}users/{_userId}/meals.json");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var mealsDict = JsonConvert.DeserializeObject<Dictionary<string, Meal>>(content);
                if (mealsDict != null)
                {
                    foreach (var mealEntry in mealsDict.Values)
                    {
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

        public async Task<bool> AddExerciseAsync(Exercise exercise)
        {
            var url = $"{_baseUrl}users/{_userId}/exercises.json";
            var json = JsonConvert.SerializeObject(exercise);
            var response = await _httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var error = JsonConvert.DeserializeObject<dynamic>(content);
                string errorMessage = "An error occurred while adding the exercise to the database.";
                if (error != null && error.error != null && error.error.message != null)
                {
                    errorMessage = error.error.message;
                }
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Failed to add the exercise", errorMessage, "OK");
                });
                return false;
            }
        }

        public async Task<List<Exercise>> GetUserExercisesAsync()
        {
            var exercises = new List<Exercise>();
            var response = await _httpClient.GetAsync($"{_baseUrl}users/{_userId}/exercises.json");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var exercisesDict = JsonConvert.DeserializeObject<Dictionary<string, Exercise>>(content);
                if (exercisesDict != null)
                {
                    foreach (var exerciseEntry in exercisesDict.Values)
                    {
                        exercises.Add(exerciseEntry);
                    }
                }
            }
            return exercises;
        }

        public async Task<List<Exercise>> GetTodayUserExercisesAsync()
        {
            var todayExxercises = new List<Exercise>();
            var todayDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

            var response = await _httpClient.GetAsync($"{_baseUrl}users/{_userId}/exercises.json");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var exercisesDict = JsonConvert.DeserializeObject<Dictionary<string, Exercise>>(content);
                if (exercisesDict != null)
                {
                    foreach (var exerciseEntry in exercisesDict.Values)
                    {
                        if (exerciseEntry.Date == todayDate)
                        {
                            todayExxercises.Add(exerciseEntry);
                        }
                    }
                }
            }

            return todayExxercises;
        }

        public async Task<bool> UpdateWaterIntakeAsync(int glasses)
        {
            var url = $"{_baseUrl}users/{_userId}/waterIntake.json";
            var json = JsonConvert.SerializeObject(new { Date = DateTime.UtcNow.ToString("yyyy-MM-dd"), Glasses = glasses });

            var response = await _httpClient.PutAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode;
        }

        public async Task<int> GetTodayWaterIntakeAsync()
        {
            var todayDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var todayWaterIntake = 0;
            var response = await _httpClient.GetAsync($"{_baseUrl}users/{_userId}/waterIntake.json");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var waterDict = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(content);
                if (waterDict != null)
                {
                    foreach (var waterEntry in waterDict.Values)
                    {
                        if (waterEntry.Date == todayDate)
                        {
                            todayWaterIntake = waterEntry.Glasses;
                        }
                    }
                }
            }

            return todayWaterIntake;
        }



    }

}
