using ProiectPDM.Service;
namespace ProiectPDM.Views;

public partial class FirstPage : ContentPage
{
    private FirebaseService _firebaseService = new FirebaseService();

    public FirstPage()
    {
        InitializeComponent();
        LoadTodayUserMeals();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTodayUserMeals();
    }

    private async Task LoadTodayUserMeals()
    {
        var meals = await _firebaseService.GetTodayUserMealsAsync();
        TodayMealsList.ItemsSource = meals;
        NoMealsLabel.IsVisible = meals == null || meals.Count == 0;
    }

    private void OnAddMealClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddMealPage());
    }
}
