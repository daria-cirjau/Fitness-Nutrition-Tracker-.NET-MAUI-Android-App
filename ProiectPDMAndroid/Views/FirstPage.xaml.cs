using ProiectPDM.Service;
using ProiectPDMAndroid.Models;
namespace ProiectPDM.Views;

public partial class FirstPage : ContentPage
{
    private FirebaseService _firebaseService = new FirebaseService();
    public static int _waterIntake { get; set; } // Default 
    public static int _targetMinutes { get; set; } = 90; // Default
    private double _totalMinutes;
    public string ProgressText => $"{_totalMinutes} min / {_targetMinutes} min";


    public FirstPage()
    {
        InitializeComponent();
        LoadUserTarget();
        LoadTodayUserMeals();
        LoadTodayUserExercises();
        LoadTodayUserWaterIntake();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTodayUserMeals();
        await LoadTodayUserExercises();
        UpdateWaterIntake();
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

    private async Task LoadTodayUserWaterIntake()

    {
        var waterIntake = await _firebaseService.GetTodayWaterIntakeAsync();
        _waterIntake = waterIntake;
        UpdateWaterIntake();
    }

    private void UpdateWaterIntake()
    {
        waterIntakeEntry.Text = _waterIntake.ToString();
    }

    private async Task LoadUserTarget()

    {
        var target = await _firebaseService.GetTarget();
        _targetMinutes = target.Item1;
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
