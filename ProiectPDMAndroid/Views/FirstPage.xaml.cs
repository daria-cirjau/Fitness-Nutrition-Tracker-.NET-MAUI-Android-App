using ProiectPDM.Service;
using ProiectPDMAndroid.Models;
namespace ProiectPDM.Views;

public partial class FirstPage : ContentPage
{
    private FirebaseService _firebaseService = new FirebaseService();
    public static int _waterIntake { get; set; } // Default 
    private double _targetMinutes = 90; // TO BE CHANGED
    private double _totalMinutes;
    public string ProgressText => $"{_totalMinutes} min / {_targetMinutes} min";


    public FirstPage()
    {
        InitializeComponent();
        LoadTodayUserMeals();
        LoadTodayUserExercises();
        LoadTodayUserWaterIntake();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTodayUserMeals();
        await LoadTodayUserExercises();
    }

    private async Task LoadTodayUserMeals()
    {
        var meals = await _firebaseService.GetTodayUserMealsAsync();
        TodayMealsList.ItemsSource = meals;
        NoMealsLabel.IsVisible = meals == null || meals.Count == 0;
    }

    private async Task LoadTodayUserExercises()
    {
        var exercises = await _firebaseService.GetTodayUserExercisesAsync();
        UpdateExerciseProgressBar(exercises);
    }

    // TO BE CHECKED
    private async Task LoadTodayUserWaterIntake()
    {
        var waterIntake = await _firebaseService.GetTodayWaterIntakeAsync();
        _waterIntake = waterIntake;
        waterIntakeEntry.Text = waterIntake.ToString();
    }

    private void OnAddMealClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddMealPage());
    }

     private void IncrementWaterIntake(object sender, EventArgs e)
    {
        _waterIntake++;
        waterIntakeEntry.Text = _waterIntake.ToString();
    }

    private void DecrementWaterIntake(object sender, EventArgs e)
    {
        if (_waterIntake > 0)
        {
            _waterIntake--;
            waterIntakeEntry.Text = _waterIntake.ToString();
        }
    }

    private async void UpdateWaterIntakeInDatabase(object sender, EventArgs e)
    {
        await _firebaseService.UpdateWaterIntakeAsync(_waterIntake);
        await DisplayAlert("Success", "Water intake saved successfully.", "OK");
    }

    private void UpdateExerciseProgressBar(List<Exercise> exercises)
    {
        double totalMinutes = exercises.Sum(e => e.Duration.TotalMinutes);
        _totalMinutes = totalMinutes;
        double progress = totalMinutes / _targetMinutes;
        progressLabel.Text = ProgressText;
        exerciseProgress.Progress = progress;
    }


}
