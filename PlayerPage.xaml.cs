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
            {
                await DisplayAlert("Error", "Write Nickname!", "Ok");
                return;
            }
        }

        string nickname = EnterPlayerName.Text.Trim();

        if (nickname.Length < 3 || nickname.Length > 8)
        {
            await DisplayAlert("Error", "Nickname is invalid (3-8 letters)!", "Ok");
            return;
        }

        if (nickname.All(char.IsUpper))
        {
            nickname = nickname.ToLower();
        }
        nickname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nickname);

        if (Players.Any(t => t.Name == nickname))
        {
            await DisplayAlert("Error", "This Player was already addded!", "Ok");
            return;
        }

        if (Counter >= PlayerAmount)
        {
            await DisplayAlert("Error", "All players already added", "Ok");
            return;
        }

        Players.Add(new Player(nickname, GameType));
        Counter++;
        EnterPlayerName.Text = string.Empty;

    }

    private async void StartGameButtonClicked(object sender, EventArgs e)
    {
        if (Counter < PlayerAmount)
        {
            await DisplayAlert("Error", $"To start game you need to add {PlayerAmount - Counter} Players", "Ok");
            return;
        }

        /*string inputMode = Preferences.Get("Input Mode", "");
        if(inputMode == "Inserting manually")
        {
            await Navigation.PushModalAsync(new GamePage(GameType, Players));
        }
        else if(inputMode == "Clicking on Board")
        {
            await Navigation.PushModalAsync(new GamePageWithDartboard(GameType, Players));
        }
        */
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