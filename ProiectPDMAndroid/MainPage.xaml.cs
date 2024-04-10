namespace ProiectPDM;
using ProiectPDM.Service;
using ProiectPDM.Views;

public partial class MainPage : ContentPage
{
    private Boolean signIn = false;
    private string _email;
    private string _password;
    private FirebaseService _firebaseService = new FirebaseService();

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        signIn = true;
        SetEmailPass();
        Boolean result = await _firebaseService.AuthenticateUserAsync(_email, _password, signIn);
        LogIn(result);
    }

    private async void OnSignUpClicked(object sender, EventArgs e)
    {
        signIn = false;
        SetEmailPass();
        Boolean result = await _firebaseService.AuthenticateUserAsync(_email, _password, signIn);
        LogIn(result);
    }

    private void LogIn(Boolean result)
    {
        if (result)
        {
            Application.Current.MainPage = new AppShell();
        }

    }

    private void SetEmailPass()
    {
        _email = EmailEntry.Text;
        _password = PasswordEntry.Text;
    }
}

