using Newtonsoft.Json;
using ProiectPDM.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProiectPDM.Services
{
    public class NutritionService
    {
        private HttpClient _httpClient;
        private string _baseUrl;
        private string _apiKey;

        public NutritionService()
        {
            _httpClient = new HttpClient();
            LoadConfig();
        }

        private async void LoadConfig()
        {
            using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync("apiProperties.json");
            using StreamReader reader = new StreamReader(fileStream);
            string json = reader.ReadToEnd();
            dynamic config = JsonConvert.DeserializeObject<dynamic>(json);
            _baseUrl = config.BaseUrl;
            _apiKey = config.ApiKey;
        }

        public async Task<List<Ingredient>> GetIngredientsNutritionAsync(string itemName)
        {
            var requestUri = $"{_baseUrl}{Uri.EscapeDataString(itemName)}";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("X-Api-Key", _apiKey); 

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(jsonResponse);
            var root = jsonDoc.RootElement;
            var items = root.GetProperty("items");

            var ingredients = new List<Ingredient>();

            foreach (var item in items.EnumerateArray())
            {
                ingredients.Add(new Ingredient
                {
                    Name = item.TryGetProperty("name", out var name) ? name.GetString() : "", 
                    Calories = item.TryGetProperty("calories", out var calories) ? calories.GetDouble() : 0,
                    Protein = item.TryGetProperty("protein_g", out var protein) ? protein.GetDouble() : 0,
                    Fat = item.TryGetProperty("fat_total_g", out var fat) ? fat.GetDouble() : 0,
                    Carbohydrates = item.TryGetProperty("carbohydrates_total_g", out var carbohydrates) ? carbohydrates.GetDouble() : 0,
                    Sugar = item.TryGetProperty("sugar_g", out var sugar) ? sugar.GetDouble() : 0,
                    Sodium = item.TryGetProperty("sodium_mg", out var sodium) ? sodium.GetDouble() : 0,
                    Potassium = item.TryGetProperty("potassium_mg", out var potassium) ? potassium.GetDouble() : 0,
                    Cholesterol = item.TryGetProperty("cholesterol_mg", out var cholesterol) ? cholesterol.GetDouble() : 0,
                    ServingSize = item.TryGetProperty("serving_size_g", out var servingSize) ? servingSize.GetDouble() : 0,
                });
            }

            return ingredients;
        }
    }
}
