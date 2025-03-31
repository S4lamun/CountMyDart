using System.Collections.ObjectModel;

namespace CountMyDartMaui;

public partial class ScoreboardPage : ContentPage
{
    Dictionary<Player, int> PlayerScoreboard;
    public new ObservableCollection<Player> Players { get; set; }
    int Counter;
    public ScoreboardPage(Dictionary<Player, int> playerScoreboard)
    {
        InitializeComponent();


        PlayerScoreboard = playerScoreboard;
        Counter = 2;
        Players = new();

        if (!playerScoreboard.Keys.Any(x => x.EndCommunicate == "Ended game") && !playerScoreboard.Keys.Any(x => x.EndCommunicate == "Zakoñczy³ grê") 
            && !playerScoreboard.Keys.Any(x => x.EndCommunicate == "Er beendete das Spiel"))
        {
            if (Preferences.Get("AppLanguage", "") == "en")
            {
                WinnerText.Text = "No winner!";
            }
            else if (Preferences.Get("AppLanguage", "") == "pl")
            {
                WinnerText.Text = "Brak zwyciêzcy!";
            }
            else if (Preferences.Get("AppLanguage", "") == "de")
            {
                WinnerText.Text = "Kein Gewinner!";
            }
            WinnerRounds.Text = string.Empty;
            return;
        }

        var winner = playerScoreboard.OrderBy(x => x.Value).First();
        WinnerText.Text = winner.Key.Name;
        if (Preferences.Get("AppLanguage", "") == "en")
        {
            WinnerRounds.Text = $"Won in {winner.Value} rounds!";
        }
        else if (Preferences.Get("AppLanguage", "") == "pl")
        {
            WinnerRounds.Text = $"Wygra³ w {winner.Value} rund!";
        }
        else if (Preferences.Get("AppLanguage", "") == "de")
        {
            WinnerRounds.Text = $"Gewonnen in {winner.Value} Runden!";
        }

        PlayerScoreboard.Remove(winner.Key);


        foreach (var a in PlayerScoreboard)
        {
            a.Key.Name = a.Key.Name + $" ({a.Value})";
            Players.Add(a.Key);
            a.Key.Place = $"{Counter}.";
            Counter++;
        }

        BindingContext = this;
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
    private async void StartNewGameButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new MainPage());
    }
}