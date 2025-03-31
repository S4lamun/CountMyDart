using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace CountMyDartMaui;

public partial class PlayerPage : ContentPage
{
    #region GlobalVariables
    private ObservableCollection<Player> players = new ObservableCollection<Player>();
    public ObservableCollection<Player> Players
    {
        get => players;
        set
        {
            if (players != value)
            {
                players = value;
                OnPropertyChanged(nameof(Players));
            }
        }
    }


    int GameType; // typeofgame as int (101,201...)
    int PlayerAmount; // how many players needed to start game
    int Counter; //how many players already added 
    #endregion

    public PlayerPage(int gameType, int playerAmount)
    {
        BindingContext = this;
        InitializeComponent();

        Counter = 0;
        this.GameType = gameType;
        this.PlayerAmount = playerAmount;

    }


    private async void AddPlayerButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(EnterPlayerName.Text))
        {
            if(Preferences.Get("AppLanguage", "") == "en")
            {
                await DisplayAlert("Error", "Write Nickname!", "Ok");
                return;
            }
            else if (Preferences.Get("AppLanguage", "") == "pl")
            {
                await DisplayAlert("B��d", "Wpisz Imi�", "Ok");
                return;
            }
            else if (Preferences.Get("AppLanguage", "") == "de")
            {
                await DisplayAlert("Fehler", "Schreiben Sie den Spitznamen!", "Ok");
                return;
            }
        }

        string nickname = EnterPlayerName.Text.Trim();

        if (nickname.Length < 3 || nickname.Length > 8)
        {
            if(Preferences.Get("AppLanguage", "") == "en")
            {
                await DisplayAlert("Error", "Nickname is invalid (3-8 letters)!", "Ok");
                return;
            }
            else if (Preferences.Get("AppLanguage", "") == "pl")
            {
                await DisplayAlert("B��d", "Imi� jest niepoprawne (3-8 liter)!", "Ok");
                return;
            }
            else if (Preferences.Get("AppLanguage", "") == "de")
            {
                await DisplayAlert("Fehler", "Spitzname ist ung�ltig (3-8 Buchstaben)!", "Ok");
                return;
            }
            
        }

        if (nickname.All(char.IsUpper))
        {
            nickname = nickname.ToLower();
        }
        nickname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nickname);

        if (Players.Any(t => t.Name == nickname))
        {
            if(Preferences.Get("AppLanguage", "") == "en")
            {
                await DisplayAlert("Error", "This Player was already addded!", "Ok");
                return;
            }
            else if (Preferences.Get("AppLanguage", "") == "pl")
            {
                await DisplayAlert("B��d", "Ten gracz zosta� ju� dodany!", "Ok");
                return;
            }
            else if (Preferences.Get("AppLanguage", "") == "de")
            {
                await DisplayAlert("Fehler", "Dieser Spieler wurde bereits hinzugef�gt!", "Ok");
                return;
            }
            
        }

        if (Counter >= PlayerAmount)
        {
            if(Preferences.Get("AppLanguage", "") == "en")
            {
                await DisplayAlert("Error", "All players already added", "Ok");
                return;
            }
            else if (Preferences.Get("AppLanguage", "") == "pl")
            {
                await DisplayAlert("B��d", "Wszyscy gracze zostali ju� dodani", "Ok");
                return;
            }
            else if (Preferences.Get("AppLanguage", "") == "de")
            {
                await DisplayAlert("Fehler", "Alle Spieler wurden bereits hinzugef�gt", "Ok");
                return;
            }
           
        }

        Players.Add(new Player(nickname, GameType));
        Counter++;
        EnterPlayerName.Text = string.Empty;

    }

    private async void StartGameButtonClicked(object sender, EventArgs e)
    {
        if (Counter < PlayerAmount)
        {
            if (Preferences.Get("AppLanguage", "") == "en")
            {
                await DisplayAlert("Error", $"To start game you need to add {PlayerAmount - Counter} Players", "Ok");
                return;
            }
            else if (Preferences.Get("AppLanguage", "") == "pl")
            {
                await DisplayAlert("B��d", $"Aby rozpocz�� gr� musisz doda� {PlayerAmount - Counter} graczy", "Ok");
                return;
            }

            else if (Preferences.Get("AppLanguage", "") == "de")
            {
                await DisplayAlert("Fehler", $"Um das Spiel zu starten, m�ssen Sie {PlayerAmount - Counter} Spieler hinzuf�gen", "Ok");
            }
                       
        }

        await Navigation.PushModalAsync(new GamePage(GameType, Players));

    }

    private async void GoBackToModeChoiceButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new MainPage());
    }

    private void EnterPlayerName_Completed(object sender, EventArgs e) // if enter clicked then add player
    {
        AddPlayerButtonClicked(sender, e);
    }
    private void OnDeletePlayer(object sender, EventArgs e)
    {
        if (sender is SwipeItem swipeItem && swipeItem.BindingContext is Player player)
        {
            Players.Remove(player);
            Counter--;
        }
    } // if swiped then delete player

    #region ProperyChanged Stuff
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion 
}