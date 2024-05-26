using ProiectPDM.Service;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace ProiectPDM.Views
{
    public partial class UserProfile : ContentPage, INotifyPropertyChanged
    {
        private int _currentExerciseTarget;
        private int _currentCalorieTarget;
        private FirebaseService _firebaseService = new FirebaseService();

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // binding calorie and exercise text

        public int CurrentExerciseTarget
        {
            get { return _currentExerciseTarget; }
            set
            {
                if (_currentExerciseTarget != value)
                {
                    _currentExerciseTarget = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int CurrentCalorieTarget
        {
            get { return _currentCalorieTarget; }
            set
            {
                if (_currentCalorieTarget != value)
                {
                    _currentCalorieTarget = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public UserProfile()
        {
            InitializeComponent();
            this.BindingContext = this;
            LoadUserTarget();
        }

        private async Task LoadUserTarget()
        {
            Tuple<int, int> target = await _firebaseService.GetTarget();
            CurrentExerciseTarget = target.Item1;
            CurrentCalorieTarget = target.Item2;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            int exerciseTarget = string.IsNullOrWhiteSpace(NewExerciseTargetEntry.Text) ? CurrentExerciseTarget : int.Parse(NewExerciseTargetEntry.Text);
            int calorieTarget = string.IsNullOrWhiteSpace(NewCalorieTargetEntry.Text) ? CurrentCalorieTarget : int.Parse(NewCalorieTargetEntry.Text);

            var result = await _firebaseService.AddTargetAsync(exerciseTarget, calorieTarget);
            if (result)
            {
                await DisplayAlert("Success", "Target saved successfully.", "OK");
                NewCalorieTargetEntry.Text = string.Empty;
                NewExerciseTargetEntry.Text = string.Empty;
                FirstPage._targetMinutes = exerciseTarget;
                CurrentExerciseTarget = exerciseTarget;
                CurrentCalorieTarget = calorieTarget;
            }
        }

    }
}