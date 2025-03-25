using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CountMyDartMaui;

public partial class GamePageWithDartboard : ContentPage
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

    public GamePageWithDartboard(int typeOfGame, ObservableCollection<Player> players)
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
            if (!int.TryParse(player.Throw1, out Throw1) || !int.TryParse(player.Throw2, out Throw2) || !int.TryParse(player.Throw3, out Throw3) || Throw1 < 0 || Throw1 > 60 || Throw2 < 0 || Throw2 > 60 || Throw3 < 0 || Throw3 > 60)
            {
                await DisplayAlert("Error", "Please enter a valid number", "OK");
                return;
            }

            if (player.TargetPoints < (Throw1 + Throw2 + Throw3))
            {
                await DisplayAlert("Error", $"Player {player.Name} threw too much!", "Ok");
            }

            if (player.TargetPoints >= (Throw1 + Throw2 + Throw3))
            {
                player.TargetPoints -= (Throw1 + Throw2 + Throw3);
            }

        }

        foreach (var player in Players) // setting Throws as default - 0
        {
            player.Throw1 = "0";
            player.Throw2 = "0";
            player.Throw3 = "0";
            if (player.TargetPoints == 0) // Communicate if player ended
            {
                await DisplayAlert("Success", $"Player {player.Name} won game!", "Ok");
                player.EndCommunicate = "Ended game";
                PlayerScoreboard.Add(player, roundCounter);
                PlayersToRemove.Add(player);

            }
        }

        RoundText = $"Round {++roundCounter}";
        OnPropertyChanged(nameof(RoundText));

        if (PlayersToRemove.Count() > 0)
        {
            foreach (var p in PlayersToRemove) // Removes players if he ended game
            {
                Players.Remove(p);
            }
            PlayersToRemove.Clear();

            var tempPlayers = Players;
            Players = null;
            OnPropertyChanged(nameof(Players));
            Players = tempPlayers;
            OnPropertyChanged(nameof(Players));
        }

    }

    private async void ThrowFocused(object sender, EventArgs e)
    {
        if (sender is Entry entry)
        {
            entry.Text = string.Empty;
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


    /*
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
    */

    private async void EndGameButtonClicked(object sender, EventArgs e)
    {
        Players.OrderByDescending(p => p.TargetPoints);
        foreach (var player in Players)
        {
            player.EndCommunicate = $"[didn't end, left to score {player.TargetPoints}]";
            PlayerScoreboard.Add(player, roundCounter);
        }
        await Navigation.PushAsync(new ScoreboardPage(PlayerScoreboard));


    }



    #region ProperyChanged Stuff
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion 
}