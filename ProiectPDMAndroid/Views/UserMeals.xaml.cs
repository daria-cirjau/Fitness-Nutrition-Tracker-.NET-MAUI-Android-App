using ProiectPDM.Service;

namespace ProiectPDM.Views;

public partial class UserMeals : ContentPage
{
    private FirebaseService _firebaseService = new FirebaseService();

    public UserMeals()
    {
        InitializeComponent();
        LoadUserMeals();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadUserMeals();
    }

    private async Task LoadUserMeals()
    {
        var meals = await _firebaseService.GetUserMealsAsync();
        MealsList.ItemsSource = meals;
    }

    private void OnAddMealClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddMealPage());
    }
}