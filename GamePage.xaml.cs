using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;

namespace CountMyDartMaui;

public partial class GamePage : ContentPage, INotifyPropertyChanged
{
    #region GlobalVariables
    int PointsToGain; // points to gain needed to end game
    int Throw1;
    int Throw2;
    int Throw3;
    public ObservableCollection<Player> Players { get; set; } // Collection of Players (for XAML)

    Dictionary<Player, int> PlayerScoreboard; // int - number of rounds in which player ended game

    List<Player> PlayersToRemove; // list of players who ended game in this round
    #region roundText 
    string roundText;

    public string RoundText
    {
        get => roundText;
        set
        {
            if (roundText != value)
            {
                roundText = value;
                OnPropertyChanged(nameof(RoundText));
            }
        }
    }

    int roundCounter; // counts how many rounds already passed
    #endregion // round number

    #endregion
    public GamePage(int typeOfGame, ObservableCollection<Player> players)
    {
        InitializeComponent();


        PointsToGain = typeOfGame;
        roundCounter = 1;
        RoundText = $"Round {roundCounter}";
        OnPropertyChanged(nameof(RoundText));
        Players = players;
        PlayerScoreboard = new();
        PlayersToRemove = new();
        foreach (var player in players)
        {
            player.Throw1 = "0";
            player.Throw2 = "0";
            player.Throw3 = "0";
        }
        BindingContext = this;
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }

    private async void AcceptAllThrowsButtonClicked(object sender, EventArgs e)
    {
        foreach (var player in Players) // Validation of Input
        {
            if (!int.TryParse(player.Throw1, out Throw1) || !int.TryParse(player.Throw2, out Throw2) ||
                !int.TryParse(player.Throw3, out Throw3)
                || Throw1 < 0 || Throw1 > 60 || Throw2 < 0 || Throw2 > 60 || Throw3 < 0 || Throw3 > 60)
            {
                if(Preferences.Get("AppLanguage", "")=="en")
                {
                    await DisplayAlert("Error", "Please enter a valid number", "OK");
                    return;
                }
                else if(Preferences.Get("AppLanguage", "") == "pl")
                {
                    await DisplayAlert("B³¹d", "Wpisz prawid³ow¹ liczbê", "OK");
                    return;
                }
                else if(Preferences.Get("AppLangauge", "") == "de")
                {
                    await DisplayAlert("Fehler", "Bitte geben Sie die richtige Nummer ein", "Ok");
                }
            }

            if (player.TargetPoints < (Throw1 + Throw2 + Throw3))
            {
                if (Preferences.Get("AppLanguage", "") == "en")
                {
                    await DisplayAlert("Error", $"Player {player.Name} threw too much!", "OK");
                }
                else if (Preferences.Get("AppLanguage", "") == "pl")
                {
                    await DisplayAlert("B³¹d", $"Gracz {player.Name} rzuci³ za du¿o", "OK");
                }
                else if (Preferences.Get("AppLangauge", "") == "de")
                {
                    await DisplayAlert("Fehler", $"Spieler {player.Name} hat zu viel geworfen", "Ok");
                }
                
            }

            if (player.TargetPoints >= (Throw1 + Throw2 + Throw3))
            {
                player.TargetPoints -= (Throw1 + Throw2 + Throw3);
            }

        }

        PlayersToRemove.Clear();

        foreach (var player in Players) // setting Throws as default - 0
        {
            player.Throw1 = "0";
            player.Throw2 = "0";
            player.Throw3 = "0";
            if (player.TargetPoints == 0) // Communicate if player ended
            {
                if (Preferences.Get("AppLanguage", "") == "en")
                {
                    await DisplayAlert("Success", $"Player {player.Name} ended game", "Ok");
                    player.EndCommunicate = "Ended game";
                }
                else if (Preferences.Get("AppLanguage", "") == "pl")
                {
                    await DisplayAlert("Sukces", $"Gracz {player.Name} skoñczy³ grê", "Ok");
                    player.EndCommunicate = "Zakoñczy³ grê";
                }
                else if (Preferences.Get("AppLanguage", "") == "de")
                {
                    await DisplayAlert("Erfolg", $"Spieler {player.Name} hat das Spiel beendet", "Ok");
                    player.EndCommunicate = "Er beendete das Spiel";
                }
                PlayerScoreboard.Add(player, roundCounter);
                PlayersToRemove.Add(player);

            }
        }

        if (Preferences.Get("AppLanguage", "") == "en")
        {
            RoundText = $"Round {++roundCounter}";
        }
        else if (Preferences.Get("AppLanguage", "") == "pl")
        {
            RoundText = $"Runda {++roundCounter}";
        }
        else if (Preferences.Get("AppLanguage", "") == "de")
        {
            RoundText = $"Runde {++roundCounter}";
        }

        OnPropertyChanged(nameof(RoundText));


        if (PlayersToRemove.Any())
        {
            foreach (var p in PlayersToRemove) // Removes players if he ended game
            {
                Players.Remove(p);
            }



            var tempPlayers = Players;
            Players = null;
            OnPropertyChanged(nameof(Players));
            Players = tempPlayers;
            OnPropertyChanged(nameof(Players));


        }

    }

   
    private async void OnEntryTapped(object sender, EventArgs e)
    {
        if (sender is Entry entry)
        {
            entry.Text = string.Empty;
            if (Preferences.Get("Input Mode", "") == "Clicking on Board")
            {
                GlobalSettings.CurrentThrow = -1;
                try
                {
                    await Shell.Current.GoToAsync(nameof(InteractiveDartboard));
                    while (GlobalSettings.CurrentThrow == -1)
                    {
                        await Task.Delay(100);
                    }
                    entry.Text = GlobalSettings.CurrentThrow.ToString();
                    
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Navigation Error", ex.Message, "OK");
                }
            }
           
        }
    }

    private void ThrowTextChanged(object sender, EventArgs e) // converting Throw Input to be only digits
    {
        if (sender is Entry entry)
        {
            string validInput = new string(entry.Text.Where(char.IsDigit).ToArray());

            if ((string)entry.Text != validInput) // We changed only if validInput is different
            {
                entry.Text = validInput;
            }
        }
    }

    private async void EndGameButtonClicked(object sender, EventArgs e)
    {
        Players.OrderByDescending(p => p.TargetPoints);
        foreach (var player in Players)
        {
            if(Preferences.Get("AppLanguage", "") == "en")
            {
                player.EndCommunicate = $"[didn't end, left to score: {player.TargetPoints}]";
            }
            else if (Preferences.Get("AppLanguage", "") == "pl")
            {
                player.EndCommunicate = $"[nie zakoñczy³, pozosta³o: {player.TargetPoints}]";
            }
            else if (Preferences.Get("AppLanguage", "") == "de")
            {
                player.EndCommunicate = $"[nicht beendet, links: {player.TargetPoints}]";
            }
            
            PlayerScoreboard.Add(player, roundCounter);
        }
        await Navigation.PushModalAsync(new ScoreboardPage(PlayerScoreboard));


    }



    #region ProperyChanged Stuff
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion 

}