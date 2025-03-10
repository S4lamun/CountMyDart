using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;

namespace CountMyDartMaui;

public partial class GamePage : ContentPage, INotifyPropertyChanged
{
    #region GlobalVariables
    int PointsToGain; // points to gain needed to end game
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
            if (player.Throw1 < 0 || player.Throw1 > 60 ||
              player.Throw2 < 0 || player.Throw2 > 60 ||
              player.Throw3 < 0 || player.Throw3 > 60)
            {
                await DisplayAlert("Error", "Invalid input (min. 0, max 60)!", "Ok");
                return;
            }

            if (player.TargetPoints < (player.Throw1 + player.Throw2 + player.Throw3))
            {
                await DisplayAlert("Error", $"Player {player.Name} threw too much!", "Ok");
            }
        }


        foreach (var player in Players)
        {
            if (player.TargetPoints >= (player.Throw1 + player.Throw2 + player.Throw3))
            {
                player.TargetPoints -= (player.Throw1 + player.Throw2 + player.Throw3);
            }



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

        foreach (var player in Players) // setting Throws as default - 0
        {
            player.Throw1 = 0;
            player.Throw2 = 0;
            player.Throw3 = 0;
        }

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

    private void ThrowFocused(object sender, EventArgs e) // if throwEntry clicked then entry.Text is empty
    {
        if (sender is Entry entry)
        {
            entry.Text = string.Empty;
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
            player.EndCommunicate = $"[didn't end, left to score {player.TargetPoints}]";
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