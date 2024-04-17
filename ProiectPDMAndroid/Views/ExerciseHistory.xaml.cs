using ProiectPDM.Service;

namespace ProiectPDM.Views;

public partial class ExerciseHistory : ContentPage
{
    private FirebaseService _firebaseService = new FirebaseService();
    public ExerciseHistory()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadUserExercises();
    }

    private async Task LoadUserExercises()
    {
        var exercises = await _firebaseService.GetUserExercisesAsync();
        ExercisesList.ItemsSource = exercises;
    }

}