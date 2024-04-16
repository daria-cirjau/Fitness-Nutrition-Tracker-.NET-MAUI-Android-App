using ProiectPDM.Service;
using ProiectPDM.Services;
using ProiectPDMAndroid.Models;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace ProiectPDM.Views;

public partial class AddExercises : ContentPage
{
    private NutritionService _nutritionService;
    public ObservableCollection<Exercise> Exercises { get; set; }
    private FirebaseService _firebaseService;
    private Exercise _selectedExercise;

    public AddExercises()
    {
        InitializeComponent();
        _nutritionService = new NutritionService();
        _firebaseService = new FirebaseService();
        Exercises = new ObservableCollection<Exercise>();
        this.BindingContext = this;
    }

    public async void OnSearchClicked(object sender, EventArgs e)
    {
        string exercise = SearchExercises.Text;
        if (!string.IsNullOrEmpty(exercise))
        {
            var exercises = await _nutritionService.GetExercisesByNameAsync(exercise);
            Exercises.Clear();
            foreach (var eachExercise in exercises)
            {
                Exercises.Add(eachExercise);
            }
        }
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedExercise = e.CurrentSelection[0] as Exercise;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_selectedExercise != null)
        {
            var duration = (this.FindByName("exerciseDuration") as TimePicker).Time;
            var todayDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
            _selectedExercise.Duration = duration;
            _selectedExercise.Date = todayDate;
            bool result = await _firebaseService.AddExerciseAsync(_selectedExercise);
            if (result)
            {
                await DisplayAlert("Success", "Exercise saved successfully.", "OK");
            }
        }
    }
}
