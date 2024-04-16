using ProiectPDM.Models;
using ProiectPDM.Service;
using ProiectPDM.Services;
using System.Collections.ObjectModel;

namespace ProiectPDM.Views;

public partial class AddMealPage : ContentPage
{
    private NutritionService _nutritionService;
    private ObservableCollection<Ingredient> _ingredients;
    private FirebaseService _firebaseService;
    public AddMealPage()
    {
        InitializeComponent();
        _nutritionService = new NutritionService();
        _firebaseService = new FirebaseService();
        _ingredients = new ObservableCollection<Ingredient>();
        IngredientsList.ItemsSource = _ingredients;
    }

    private async void OnAddIngredientClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(IngredientEntry.Text))
        {
            var ingredientName = IngredientEntry.Text;
            var ingredientInfo = await _nutritionService.GetIngredientsNutritionAsync(ingredientName);
            if (ingredientInfo != null && ingredientInfo.Any())
            {
                foreach (var ingredient in ingredientInfo)
                {
                    _ingredients.Add(ingredient);
                }
                var meal = new Meal
                {
                    MealType = MealTypeEntry.Text,
                    Date = DateTime.UtcNow.ToString("yyyy-MM-dd"), 
                    Ingredients = _ingredients.ToList()
                };

                var success = await _firebaseService.AddMealAsync(meal);
                if (success)
                {
                    IngredientEntry.Text = string.Empty;
                }
            }
        }
    }

}